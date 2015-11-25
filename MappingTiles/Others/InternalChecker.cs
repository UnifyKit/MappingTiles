using System;
using System.Collections.Generic;
using System.Linq;

namespace MappingTiles
{
    internal static class InternalChecker
    {
        public static void CheckArrayIsEmptyOrNull(IEnumerable<object> objects, string argument)
        {
            if (objects == null || objects.Count() == 0)
            {
                throw new ArgumentNullException(argument, Messages.CollectionNullOrEmpty);
            }
        }

        public static void CheckParameterIsNull(object value, string parameter)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameter);
            }
        }

    }
}
