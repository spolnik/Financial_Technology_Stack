using System;
using Finance.Common.Extension;

namespace Finance.Common.Validation
{
    public static class Guard
    {
        public static void IsNotNull<T>(T value) where T : class
        {
            if (value.IsNull())
                throw new ArgumentNullException();
        }

        public static void IsNotNull<T>(T value, string paramName) where T : class
        {
            if (value.IsNull())
                throw new ArgumentNullException(paramName);
        }

        public static void IsNotNull<T>(T value, string paramName, string message)
            where T : class
        {
            if (value.IsNull())
                throw new ArgumentNullException(paramName, message);
        }

        public static void IsNotEmpty(string value)
        {
            if (value.IsEmpty())
                throw new ArgumentException("string value must not be empty");
        }

        public static void IsNotEmpty(string value, string paramName)
        {
            if (value.IsEmpty())
                throw new ArgumentException("string value must not be empty", paramName);
        }

        public static void IsNotEmpty(string value, string paramName, string message)
        {
            if (value.IsEmpty())
                throw new ArgumentException(message, paramName);
        }

        public static void IsValid(Func<bool> isValid, string paramName, string message)
        {
            if (!isValid())
                throw new ArgumentException(message, paramName);
        }

        public static void IsTrue(Func<bool> isValid, string message)
        {
            if (!isValid())
                throw new Exception(message);
        }

        public static void IsTrue(Func<bool> isValid, string message, string paramName)
        {
            if (!isValid())
                throw new ArgumentException(message, paramName);
        }

        public static void IsTrue<T>(T target, Func<T, bool> condition)
        {
            if (!condition(target))
                throw new ArgumentException("condition was not true");
        }

        public static void IsTrue<T>(T target, Func<T, bool> condition, string paramName)
        {
            if (!condition(target))
                throw new ArgumentException("condition was not true", paramName);
        }

        public static void IsTrue<T>(T target, Func<T, bool> condition, string paramName, string message)
        {
            if (!condition(target))
                throw new ArgumentException(message, paramName);
        }
    }
}