﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Serialize.EfCore.Internals
{
    internal static class KnownTypes
    {
        public static readonly Type[] All =
        {
            typeof(bool),
            typeof(decimal), typeof(double),
            typeof(float),
            typeof(int), typeof(uint),
            typeof(short), typeof(ushort),
            typeof(long), typeof(ulong),
            typeof(string),
            typeof(DateTime), typeof(DateTimeOffset),
            typeof(TimeSpan), typeof(Guid),
            typeof(DayOfWeek), typeof(DateTimeKind),
            typeof(Enum),
            #if NET6_0_OR_GREATER
            typeof(DateOnly), typeof(TimeOnly),
            #endif
        };

        private static readonly HashSet<Type> _allExploded = new HashSet<Type>(Explode(All, true, true));

        public static bool Match(Type type) => 
            type != null && (_allExploded.Contains(type) || _allExploded.Any(t => t.IsAssignableFrom(type)));

        public static IEnumerable<Type> Explode(IEnumerable<Type> types, bool includeArrayTypes, bool includeListTypes)
        {
            foreach (var type in types)
            {
                yield return type;
                if (includeArrayTypes)
                    yield return type.MakeArrayType();
                if (includeListTypes)
                    yield return typeof(List<>).MakeGenericType(type);

                if (type.IsClass || type.IsInterface)
                    continue;

                var nullableType = typeof(Nullable<>).MakeGenericType(type);
                yield return nullableType;
                if (includeArrayTypes)
                    yield return nullableType.MakeArrayType();
                if (includeListTypes)
                    yield return typeof(List<>).MakeGenericType(nullableType);
            }
        }
    }
}
