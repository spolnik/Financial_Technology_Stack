using System;
using System.Collections.Generic;
using System.Linq;
using Finance.Common.Validation;

namespace Finance.Common.Extension
{
    public static class EnumExtensions
    {
        public static T ParseTo<T>(this string value)
        {
            return ParseTo<T>(value, false);
        }

        public static T ParseTo<T>(this string value, bool ignoreCase)
        {
            Guard.IsNotNull(value, "value");

            value = value.Trim();

            Guard.IsValid(() => value.Length > 0, "value",
                              "Must specify valid information for parsing in the string.");

            var type = typeof(T);

            Guard.IsValid(() => type.IsEnum, "T",
                              "Type provided must be an Enum.");

            return (T)Enum.Parse(type, value, ignoreCase);
        }

        /// <summary>
        /// Gets the values from the generic enumeration.
        /// </summary>
        /// <typeparam name="TEnum">Input enumeration.</typeparam>
        /// <returns>Collection of enum's values.</returns>
        public static IEnumerable<string> GetEnumStringValues<TEnum>() where TEnum : struct
        {
            return GetEnumValues<TEnum>().Select(s => s.ToString());
        }

        /// <summary>
        /// Gets the values from the generic enumeration, and returns the IEnumerable with elements of TEnum type
        /// </summary>
        /// <typeparam name="TEnum">Input enumeration.</typeparam>
        /// <returns>Collection of enum's string values.</returns>
        public static IEnumerable<TEnum> GetEnumValues<TEnum>() where TEnum : struct
        {
            Guard.IsValid(() => typeof (TEnum).IsEnum, "T",
                              "Type provided must be an Enum.");

            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}