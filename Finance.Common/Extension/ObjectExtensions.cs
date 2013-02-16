using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Finance.Common.Validation;

namespace Finance.Common.Extension
{
    public static class ObjectExtensions
    {
        private const int RecursionLimit = 10;

        public static bool IsNull(this object value)
        {
            return value == null;
        }

        public static bool IsNotNull(this object value)
        {
            return !value.IsNull();
        }

        public static string Stringify(this object value)
        {
            try
            {
                return StringifyInternal(value, RecursionLimit);
            }
            catch
            {
                return value.ToString();
            }
        }

        private static string StringifyInternal(object value, int recursionLevel)
        {
            if (value.IsNull())
                return "null";

            if (recursionLevel < 0)
                throw new InvalidOperationException();

            if (value is string || value is char)
                return String.Format("\"{0}\"", value);

            var collection = value as IEnumerable;
            if (collection != null)
                return StringifyCollection(collection, recursionLevel);

            return value.GetType().IsValueType 
                ? value.ToString() 
                : StringifyObject(value, recursionLevel);
        }


        private static string StringifyCollection(IEnumerable collection, int recursionLevel)
        {
            var elements = collection.Cast<object>()
                .Select(x => StringifyInternal(x, recursionLevel - 1))
                .ToArray();

            return String.Format("[{0}]", String.Join(", ", elements));
        }

        private static string StringifyObject(object value, int recursionLevel)
        {
            var elements = value
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(
                    x => "{0} = {1}".FormatWith(x.Name, StringifyInternal(x.GetValue(value, null), recursionLevel - 1)))
                .ToArray();

            return String.Format("{{{0}}}", String.Join(", ", elements));
        }

        public static T CastAs<T>(this object input)
            where T : class
        {
            Guard.IsNotNull(input, "input");

            var result = input as T;
            if (result.IsNull())
                throw new InvalidOperationException(
                    "Unable to convert from {0} to {1}".FormatWith(input.GetType().FullName, typeof (T).FullName));

            return result;
        }

        /// <summary>
        ///   Returns the value of the instance member, or the default value if the instance is null
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <typeparam name = "TValue"></typeparam>
        /// <param name = "instance"></param>
        /// <param name = "accessor"></param>
        /// <param name = "defaultValue"></param>
        /// <returns></returns>
        public static TValue ValueOrDefault<T, TValue>(this T instance, Func<T, TValue> accessor, TValue defaultValue)
            where T : class
        {
            return null == instance ? defaultValue : accessor(instance);
        }

        public static T IsTypeOf<T>(this object value)
        {
            Guard.IsNotNull(value);

            if (value is T)
                return (T) value;

            throw new ArgumentException("{0} is not an instance of type {1}".FormatWith(value.GetType().Name,
                                                                                        typeof (T).Name));
        }
    }
}