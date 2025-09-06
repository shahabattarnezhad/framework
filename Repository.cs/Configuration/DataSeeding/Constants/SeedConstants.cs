namespace Repository.Configuration.DataSeeding.Constants;

public static class SeedConstants
{
    // Roles:
    public static readonly string AdminRoleId = "f560db91-8df4-4ee3-b221-f6dd95de65ee";
    public static readonly string ClientRoleId = "42b5e49e-027a-4119-b4c9-b28ae95787d5";
    public static readonly string AccountantRoleId = "31d95c22-b96f-43f6-a2e9-608b249f8ad5";
    public static readonly string ManagerRoleId = "7230de22-9f56-443b-b769-1bd9ec9a7d55";
    public static readonly string DriverRoleId = "f5298825-9df0-497a-98c9-8f5ca37e609b";

    // Users:
    public static readonly string AdminUserId = "6e8351db-d080-4693-9080-9b89a8f8a74e";

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
}
