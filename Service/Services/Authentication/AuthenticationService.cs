using AutoMapper;
using Contracts.Base;
using Contracts.Logging;
using Entities.ConfigurationModels;
using Entities.Exceptions.Authentication;
using Entities.Exceptions.General;
using Entities.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts.Interfaces.Authentication;
using Shared.DTOs.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service.Services.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IUserProfileImageService _profileImageService;
    private readonly UserManager<User> _userManager;
    private readonly IOptions<JwtConfiguration> _configuration;
    private readonly JwtConfiguration _jwtConfiguration;

    private User? _user;

    public AuthenticationService(ILoggerManager logger,
                                 IMapper mapper,
                                 UserManager<User> userManager,
                                 IOptions<JwtConfiguration> configuration,
                                 IUserProfileImageService profileImageService,
                                 IRepositoryManager repository)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _jwtConfiguration = _configuration.Value;
        _profileImageService = profileImageService;
        _repository = repository;
    }


    public async Task<IdentityResult> RegisterUser
        (UserForRegistrationDto userForRegistration, CancellationToken cancellationToken = default)
    {
        var anyUserExists = await _repository.User.ExistsAsync(false, cancellationToken);

        if (anyUserExists)
            throw new RegistrationForbiddenException();

        var usernameResult =
            await CheckDuplicateUsername(userForRegistration);

        if (!usernameResult.Succeeded)
            return usernameResult;

        var emailResult =
            await CheckDuplicateEmail(userForRegistration);

        if (!emailResult.Succeeded)
            return emailResult;

        var user =
            _mapper.Map<User>(userForRegistration);

        var result =
            await _userManager.CreateAsync(user, userForRegistration.Password!);

        if (!result.Succeeded)
            _logger.LogWarn($"{nameof(RegisterUser)}: Registration failed. Wrong user name or password.");

        if (result.Succeeded && userForRegistration.Roles?.Any() == true)
        {
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles!);
        }
        else if (!anyUserExists)
        {
            await _userManager.AddToRoleAsync(user, "Admin");
        }

        return result;
    }


    private async Task<IdentityResult> CheckDuplicateEmail(UserForRegistrationDto userForRegistration)
    {
        if (!string.IsNullOrWhiteSpace(userForRegistration.Email))
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(userForRegistration.Email);
            if (existingUserByEmail != null)
            {
                _logger.LogWarn($"{nameof(RegisterUser)}: Email '{userForRegistration.Email}' is already registered.");
                var error = new IdentityError
                {
                    Code = "DuplicateEmail",
                    Description = "Email is already registered."
                };
                return IdentityResult.Failed(error);
            }
        }

        return IdentityResult.Success;
    }

    private async Task<IdentityResult> CheckDuplicateUsername(UserForRegistrationDto userForRegistration)
    {
        var existingUserByName = await _userManager.FindByNameAsync(userForRegistration.UserName!);
        if (existingUserByName != null)
        {
            _logger.LogWarn($"{nameof(RegisterUser)}: Username '{userForRegistration.UserName}' is already taken.");
            var error = new IdentityError
            {
                Code = "DuplicateUserName",
                Description = "Username is already taken."
            };
            return IdentityResult.Failed(error);
        }

        return IdentityResult.Success;
    }

    public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuth, CancellationToken cancellationToken = default)
    {
        _user =
            await _userManager.FindByNameAsync(userForAuth.UserName!);

        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password!));

        if (!result)
            _logger.LogWarn($"{nameof(ValidateUser)}: Authentication failed. Wrong user name or password.");

        return result;
    }


    public async Task<TokenDto> CreateToken(bool populateExp, CancellationToken cancellationToken = default)
    {
        var signingCredentials = GetSigningCredentials();

        var claims = await GetClaims();

        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();

        _user!.RefreshToken = refreshToken;

        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _userManager.UpdateAsync(_user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto(accessToken, refreshToken);
    }


    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8
            .GetBytes(Environment.GetEnvironmentVariable("SECRET")!);

        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret,
                                 SecurityAlgorithms.HmacSha256);
    }


    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user?.UserName!),
            new Claim(ClaimTypes.GivenName, _user?.FirstName!),
            new Claim(ClaimTypes.Surname, _user?.LastName!),
            new Claim(ClaimTypes.NameIdentifier, _user?.Id.ToString()!),
            new Claim("IsActive", _user!.IsActive.ToString())
        };

        var roles =
            await _userManager.GetRolesAsync(_user!);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }


    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now
            .AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }


    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }


    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(Environment.GetEnvironmentVariable("SECRET")!)),
            ValidateLifetime = true,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(token,
                                               tokenValidationParameters,
                                                    out securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }


    public async Task<TokenDto> RefreshToken(TokenDto tokenDto, CancellationToken cancellationToken = default)
    {
        var principal =
            GetPrincipalFromExpiredToken(tokenDto.AccessToken);

        var user =
            await _userManager.FindByNameAsync(principal.Identity!.Name!);

        if (user == null ||
            user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new RefreshTokenBadRequest();
        }

        _user = user;

        return await CreateToken(populateExp: false);
    }

    public async Task<TokenDto> RefreshToken(HttpContext context, CancellationToken cancellationToken = default)
    {
        context.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);

        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new RefreshTokenBadRequest();

        var user = await _userManager.Users.FirstOrDefaultAsync(u =>
            u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.Now);

        if (user == null)
            throw new RefreshTokenBadRequest();

        _user = user;

        return await CreateToken(populateExp: false);
    }

    public async Task<UserDto?> GetCurrentUserAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
        .Include(u => u.ProfileImage)
        .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
        .FirstOrDefaultAsync(u => u.UserName == username);

        if (user == null)
            return null;

        var userDto = _mapper.Map<UserDto>(user);

        userDto.Roles = user.UserRoles?
            .Select(ur => ur.Role?.Name)
            .Where(name => !string.IsNullOrEmpty(name))
            .ToList();

        return userDto;
    }

    public void SetTokensInsideCookie(TokenDto tokenDto, HttpContext context, CancellationToken cancellationToken = default)
    {
        context.Response.Cookies.Append("accessToken", tokenDto.AccessToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(15),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });

        context.Response.Cookies.Append("refreshToken", tokenDto.RefreshToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });
    }

    public bool IsCurrentUser(string userId, HttpContext context, CancellationToken cancellationToken = default)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
            return false;

        var currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
            return false;

        return string.Equals(currentUserId, userId, StringComparison.OrdinalIgnoreCase);
    }

    public async Task<UserValidationResult> ValidateUserRegistrationAsync
        (UserForRegistrationDto dto, CancellationToken cancellationToken)
    {
        var errors = new List<IdentityError>();

        var usernameResult = await CheckDuplicateUsername(dto);
        if (!usernameResult.Succeeded)
            errors.AddRange(usernameResult.Errors);

        var emailResult = await CheckDuplicateEmail(dto);
        if (!emailResult.Succeeded)
            errors.AddRange(emailResult.Errors);

        return new UserValidationResult { Errors = errors };
    }


    public async Task<UserDto> CreateUserAsync(UserForRegistrationDto dto, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(dto);
        var result = await _userManager.CreateAsync(user, dto.Password!);

        if (!result.Succeeded)
        {
            var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
            _logger.LogError($"{nameof(CreateUserAsync)} failed: {errorMessage}");
            throw new Exception("User creation failed.");
        }

        if (dto.Image is not null)
        {
            await _profileImageService.SaveUserProfileImageAsync(user.Id, dto.Image, "avatar", cancellationToken);
        }

        if (dto.Roles?.Any() == true)
            await _userManager.AddToRolesAsync(user, dto.Roles!);

        var userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }
}
