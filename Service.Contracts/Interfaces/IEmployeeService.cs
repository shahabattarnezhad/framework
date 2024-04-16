﻿using Shared.DTOs.Sample.Employee;

namespace Service.Contracts.Interfaces;

public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetAll(Guid companyId, bool trackChanges);

    EmployeeDto Get(Guid companyId, Guid id, bool trackChanges);
}
