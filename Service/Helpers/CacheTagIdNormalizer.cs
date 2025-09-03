using System.Globalization;

namespace Service.Helpers;

public static class CacheTagIdNormalizer
{
    public static string Normalize(object id)
    {
        switch (id)
        {
            case Guid g:
                return g.ToString("D");
            case string s when Guid.TryParse(s, out var g2):
                return g2.ToString("D");
            case IFormattable f:
                return f.ToString(null, CultureInfo.InvariantCulture);
            default:
                return id.ToString() ?? string.Empty;
        }
    }
}
