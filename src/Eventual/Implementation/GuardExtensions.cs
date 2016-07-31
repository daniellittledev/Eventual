using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventual.Implementation
{
    public static class GuardExtensions
    {
        public static void RequireNotDefault<T>(this T target, string argumentName)
            where T : struct
        {
            if (target.Equals(default(T)))
            {
                throw new ArgumentException("Argument was null", argumentName);
            }
        }

        public static void RequireNotNull<T>(this T target, string argumentName)
        {
            if (target == null)
            {
                throw new ArgumentException("Argument was null", argumentName);
            }
        }

        public static void RequireNotNullOrEmpty<T>(this IEnumerable<T> enumerable, string argumentName)
        {
            if (enumerable == null || !enumerable.Any())
            {
                throw new ArgumentException("Collection was null or empty", argumentName);
            }
        }
    }
}
