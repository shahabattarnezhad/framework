using Contracts.Interfaces.Authentication;
using Contracts.Interfaces.Sample;
using Microsoft.EntityFrameworkCore.Storage;

namespace Contracts.Base;

public interface IRepositoryManager
{
    // Sample:
    ICompanyRepository Company { get; }
    IEmployeeRepository Employee { get; }

    // Authentication:
    IUserRepository User { get; }
    IRoleRepository Role { get; }
    IRolePermissionRepository RolePermission { get; }
    IUserRoleRepository UserRole { get; }
    IPermissionRepository Permission { get; }
    IUserProfileImageRepository ProfileImage { get; }


    Task SaveAsync(CancellationToken cancellationToken = default);

    // Atmoic transaction:
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
