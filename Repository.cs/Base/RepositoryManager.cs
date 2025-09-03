using Contracts.Base;
using Contracts.Interfaces.Authentication;
using Contracts.Interfaces.Sample;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Data;
using Repository.Repositories.Authentication;
using Repository.Repositories.Sample;

namespace Repository.Base;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;

    // Sample:
    private readonly Lazy<ICompanyRepository> _companyRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;

    // Authentication:
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IRoleRepository> _roleRepository;
    private readonly Lazy<IRolePermissionRepository> _rolePermissionRepository;
    private readonly Lazy<IUserRoleRepository> _userRoleRepository;
    private readonly Lazy<IUserProfileImageRepository> _profileImageRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;

        // Sample:
        _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(repositoryContext));
        _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(repositoryContext));

        // Authentication:
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
        _roleRepository = new Lazy<IRoleRepository>(() => new RoleRepository(repositoryContext));
        _rolePermissionRepository = new Lazy<IRolePermissionRepository>
            (() => new RolePermissionRepository(repositoryContext));
        _userRoleRepository = new Lazy<IUserRoleRepository>(() => new UserRoleRepository(repositoryContext));
        _profileImageRepository = new Lazy<IUserProfileImageRepository>
            (() => new UserProfileImageRepository(repositoryContext));
    }

    // Sample:
    public ICompanyRepository Company => _companyRepository.Value;
    public IEmployeeRepository Employee => _employeeRepository.Value;

    // Authentication:
    public IUserRepository User => _userRepository.Value;
    public IRoleRepository Role => _roleRepository.Value;
    public IRolePermissionRepository RolePermission => _rolePermissionRepository.Value;
    public IUserRoleRepository UserRole => _userRoleRepository.Value;
    public IUserProfileImageRepository ProfileImage => _profileImageRepository.Value;


    // Saving process:
    public async Task SaveAsync(CancellationToken cancellationToken = default) =>
        await _repositoryContext.SaveChangesAsync(cancellationToken);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        await _repositoryContext.Database.BeginTransactionAsync(cancellationToken);
}
