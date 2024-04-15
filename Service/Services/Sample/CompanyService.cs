using AutoMapper;
using Contracts.Base;
using Contracts.Logging;
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
        try
        {
            var entities =
                _repository.Company.GetAll(trackChanges);

            var entitiesDto =
                _mapper.Map<IEnumerable<CompanyDto>>(entities);

            return entitiesDto;
        }
        catch (Exception ex)
        {
            _logger
                .LogError($"Something went wrong in the {nameof(GetAll)} service method {ex}");
            throw;
        }
    }
}
