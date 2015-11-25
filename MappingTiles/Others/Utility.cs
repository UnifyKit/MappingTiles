using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace MappingTiles
{
    public static class Utility
    {
        public static string Version
        {
            get
            {
                string name = typeof(Utility).Assembly.FullName;
                AssemblyName assemblyName = new AssemblyName(name);

                return string.Format(CultureInfo.InvariantCulture,"{0}.{1}" ,assemblyName.Version.Major,assemblyName.Version.Minor);
            }
        }

        internal static string CreateUniqueId()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 22).Replace("/", "").Replace("+", "").Replace("-", "");
        }

        public static double ConvertDegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
