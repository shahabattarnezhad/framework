﻿using AutoMapper;
using Entities.Models.Sample;
using Shared.DTOs.Sample.Company;
using Shared.DTOs.Sample.Employee;

namespace Api.Utilities.MapperProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>().ReverseMap();

        CreateMap<Employee, EmployeeDto>().ReverseMap();
    }
}
