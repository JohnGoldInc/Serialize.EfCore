﻿using System.Linq;

namespace Serialize.EfCore.Extensions
{
    internal static class ObjectExtensions
    {
        public static bool IsEqualToAny<T>(this T item, params T[] items)
        {
            return items.Contains(item);
        }
    }
}
