using AutoMapper;
using Contracts.Base;
using Contracts.Logging;
using Entities.Exceptions.Authentication;
using Entities.Exceptions.General;
using Entities.Models.Authentication;
using Microsoft.AspNetCore.Http;
using Service.Contracts.Base;
using Service.Contracts.Interfaces.Authentication;

namespace Service.Services.Authentication;

public class UserProfileImageService : IUserProfileImageService
{
    private readonly IRepositoryManager _repository;
    private readonly IFileService _fileService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public UserProfileImageService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IFileService fileService)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<UserProfileImage> SaveUserProfileImageAsync
        (string userId, IFormFile file, string imageType, CancellationToken cancellationToken)
    {
        // ذخیره تصویر اصلی و دریافت URL آن
        var originalUrl = await _fileService.SaveFileAsync(file, userId);

        // استخراج نام فایل از URL
        var fileName = Path.GetFileName(originalUrl);

        // ساخت مسیرهای سایزهای مختلف (در صورت نیاز واقعی می‌تونی پردازش انجام بدی)
        var image = new UserProfileImage
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = imageType,
            AltText = "User profile image",
            ProfileImageOriginalUrl = originalUrl,
            ProfileImageSmallUrl = $"/files/images/{userId}/small/{fileName}",
            ProfileImageMediumUrl = $"/files/images/{userId}/medium/{fileName}",
            ProfileImageLargeUrl = $"/files/images/{userId}/large/{fileName}",
        };

        _repository.ProfileImage.CreateEntity(image);
        await _repository.SaveAsync(cancellationToken);

        return image;
    }

    public async Task DeleteUserProfileImageAsync(string userId, Guid imageId, CancellationToken cancellationToken)
    {
        await GetUserById(userId, false, cancellationToken);

        var image = 
            await _repository.ProfileImage.GetByUserIdAsync(userId, imageId,  true, cancellationToken);

        if (image == null)
            throw new EntityNotFoundException(imageId);

        // حذف فایل اصلی
        if (!string.IsNullOrWhiteSpace(image.ProfileImageOriginalUrl))
            await _fileService.DeleteFileAsync(image.ProfileImageOriginalUrl);

        // (در صورت نیاز، سایزهای کوچک‌تر هم حذف بشن، به ساختار فایل‌هات بستگی داره)

        _repository.ProfileImage.DeleteEntity(image);
        await _repository.SaveAsync(cancellationToken);
    }

    private async Task GetUserById(string userId, bool trackChanges, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new IdParametersBadRequestException();

        var user =
            await _repository.User.CheckIfUserExistsAsync(userId, trackChanges, cancellationToken);

        if (!user)
            throw new UserNotFoundByIdException(userId);
    }
}
