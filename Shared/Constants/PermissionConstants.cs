using Shared.DTOs.Authentication;
using System.Security;

namespace Shared.Constants;

public static class PermissionConstants
{
    // PermissionId:
    public static readonly Guid UsersReadId = Guid.Parse("7ff0c4fe-c379-46ea-8a6c-1e8a3edf8745");
    public static readonly Guid UsersCreateId = Guid.Parse("3f93cbcc-bb58-4775-87ce-bfe7f97d65f5");
    public static readonly Guid UsersUpdateId = Guid.Parse("a9b7ca4a-8233-4b88-b63e-f04ddfd9facf");
    public static readonly Guid UsersDeleteId = Guid.Parse("bd9f69bf-630e-477c-b2ae-c9f67e5e9c0c");

    // PermissionName:
    public const string UsersReadName = "Users.Read";
    public const string UsersCreateName = "Users.Create";
    public const string UsersUpdateName = "Users.Update";
    public const string UsersDeleteName = "Users.Delete";

    // PermissionDisplayName:
    public const string UsersReadDisplayName = "Read Users";
    public const string UsersCreateDisplayName = "Create Users";
    public const string UsersUpdateDisplayName = "Update Users";
    public const string UsersDeleteDisplayName = "Delete Users";

    // Sample:
    // Companies Permissions
    public static readonly Guid CompaniesReadId = Guid.Parse("cac6e0c1-296e-4627-a662-6273bd2beaaf");
    public static readonly Guid CompaniesCreateId = Guid.Parse("7b84f9f8-b255-4385-835d-5a5576366454");
    public static readonly Guid CompaniesUpdateId = Guid.Parse("6c76f210-0a9a-486c-99e5-1ac1cf0fa90c");
    public static readonly Guid CompaniesDeleteId = Guid.Parse("b6fdb509-59d8-4fc7-a7bd-8b7bdaa473a7");

    public const string CompaniesReadName = "Companies.Read";
    public const string CompaniesCreateName = "Companies.Create";
    public const string CompaniesUpdateName = "Companies.Update";
    public const string CompaniesDeleteName = "Companies.Delete";

    public const string CompaniesReadDisplayName = "Read Companies";
    public const string CompaniesCreateDisplayName = "Create Companies";
    public const string CompaniesUpdateDisplayName = "Update Companies";
    public const string CompaniesDeleteDisplayName = "Delete Companies";


    // Centralized Permission List
    public static IReadOnlyList<PermissionDto> AllPermissions => new List<PermissionDto>
    {
        new (UsersReadId,UsersReadName,UsersReadDisplayName),
        new (UsersCreateId,UsersCreateName,UsersCreateDisplayName),
        new (UsersUpdateId,UsersUpdateName,UsersUpdateDisplayName),
        new (UsersDeleteId,UsersDeleteName,UsersDeleteDisplayName),

        // Sample:
        new (CompaniesReadId,CompaniesReadName,CompaniesReadDisplayName),
        new (CompaniesCreateId,CompaniesCreateName,CompaniesCreateDisplayName),
        new (CompaniesUpdateId,CompaniesUpdateName,CompaniesUpdateDisplayName),
        new (CompaniesDeleteId,CompaniesDeleteName,CompaniesDeleteDisplayName),
    };
}
