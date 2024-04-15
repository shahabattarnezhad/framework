using AutoMapper;
using Contracts.Base;
using Contracts.Logging;
using Entities.Exceptions.Sample.Company;
using Entities.Models.Sample;
using Service.Contracts.Interfaces;
using Shared.DTOs.Sample.Company;

namespace Service.Services.Sample;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repository,
                          ILoggerManager logger,
                          IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public IEnumerable<CompanyDto> GetAll(bool trackChanges)
    {
        var entities =
                _repository.Company.GetAll(trackChanges);

        var entitiesDto =
            _mapper.Map<IEnumerable<CompanyDto>>(entities);

        return entitiesDto;
    }


    public CompanyDto Get(Guid entityId, bool trackChanges)
    {
        var entity =
            _repository.Company.Get(entityId, trackChanges);

        if (entity is null) 
            throw new CompanyNotFoundException(entityId);

        var entityDto =
            _mapper.Map<CompanyDto>(entity);

        return entityDto;
    }
}
