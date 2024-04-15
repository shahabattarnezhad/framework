using AutoMapper;
using Entities.Models.Sample;
using Shared.DTOs.Sample.Company;

namespace Api.Utilities.MapperProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>().ReverseMap();
    }
}
