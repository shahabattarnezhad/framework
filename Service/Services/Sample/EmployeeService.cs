using AutoMapper;
using Contracts.Base;
using Contracts.Logging;
using Service.Contracts.Interfaces;

namespace Service.Services.Sample;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repository,
                           ILoggerManager logger,
                           IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
}
