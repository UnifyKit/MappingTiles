using System;

namespace MappingTiles
{
    public static class Utilities
    {
        internal static string CreateUniqueId()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 22).Replace("/", "").Replace("+", "").Replace("-", "");
        }
    }
}
