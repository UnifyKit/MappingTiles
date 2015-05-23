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
                throw new ArgumentNullException(argument,ApplicationMessages.CollectionNullOrEmpty);
            }
        }
    }
}
