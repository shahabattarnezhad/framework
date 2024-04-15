using Contracts.Base;
using Contracts.Logging;
using Service.Contracts.Interfaces;

namespace Service.Services.Sample;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public CompanyService(IRepositoryManager repository,
                          ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
