namespace Service.Helpers;

public static class CacheTagFormatter
{
    public static string ListTag(string entityName) => $"{entityName}ListCache";

    public static string DetailTag<TEntityId>(string entityName, TEntityId entityId) =>
        $"{entityName}DetailCache-{entityId}";
}
