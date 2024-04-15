﻿using Contracts.Interfaces.Sample;
using Entities.Models.Sample;
using Repository.Base;
using Repository.Data;

namespace Repository.Repositories.Sample;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext) 
        : base(repositoryContext)
    {
    }
}