using AutoMapper;
using Entities.Models.Authentication;
using Entities.Models.Sample;
using Shared.DTOs.Authentication;
using Shared.DTOs.Sample.Company;
using Shared.DTOs.Sample.Employee;

namespace Api.Utilities.MapperProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Sample:
        CreateMap<Company, CompanyDto>().ReverseMap();
        CreateMap<Company, CompanyForUpdateDto>().ReverseMap();
        CreateMap<Company, CompanyForCreationDto>().ReverseMap();

        CreateMap<Employee, EmployeeDto>().ReverseMap();
        CreateMap<Employee, EmployeeForUpdateDto>().ReverseMap();
        CreateMap<Employee, EmployeeForCreationDto>().ReverseMap();

        // Authentication:
        CreateMap<User, UserForRegistrationDto>().ReverseMap();
        CreateMap<User, UserForUpdationDto>().ReverseMap();
        CreateMap<User, UserForPatchDto>().ReverseMap();
        CreateMap<User, UserForDisplayDto>().ForMember
            (dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles != null ? src.UserRoles
            .Where(ur => ur.Role != null)
            .Select(ur => ur.Role!.Name!)
            .ToList() : new List<string>()))
            .ForMember(dest => dest.ProfileImageSmallUrl, opt => opt.MapFrom
            (src => src.ProfileImage != null ? src.ProfileImage.ProfileImageSmallUrl : null))
            .ReverseMap();

        CreateMap<User, UserDto>()
    .ForMember(dest => dest.Roles,
        opt => opt.MapFrom(src => src.UserRoles != null
            ? src.UserRoles
                .Where(ur => ur.Role != null)
                .Select(ur => ur.Role!.Name!)
                .ToList()
            : new List<string>()))
    .ForMember(dest => dest.ProfileImageSmallUrl,
        opt => opt.MapFrom(src => src.ProfileImage != null
            ? src.ProfileImage.ProfileImageSmallUrl
            : null))
    .ForMember(dest => dest.ProfileImageMediumUrl,
        opt => opt.MapFrom(src => src.ProfileImage != null
            ? src.ProfileImage.ProfileImageMediumUrl
            : null))
    .ForMember(dest => dest.ProfileImageLargeUrl,
        opt => opt.MapFrom(src => src.ProfileImage != null
            ? src.ProfileImage.ProfileImageLargeUrl
            : null))
    .ForMember(dest => dest.ProfileImageOriginalUrl,
        opt => opt.MapFrom(src => src.ProfileImage != null
            ? src.ProfileImage.ProfileImageOriginalUrl
            : null))
    .ReverseMap();

        CreateMap<UserRole, UserRoleDto>().ReverseMap();

        CreateMap<Role, RoleDto>().ReverseMap();
    }
}
