using System;
using System.Collections.Generic;

namespace Finance.Common.Extension
{
    public static class DisposableExtensions
    {
        /// <summary>
        ///   Wraps an object that implements IDisposable in an enumeration to make it safe for use in LINQ expressions
        /// </summary>
        /// <typeparam name = "T">The type of the object, which must implement IDisposable</typeparam>
        /// <param name = "target">The target to wrap</param>
        /// <returns>An enumeration with a single entry equal to the target</returns>
        public static IEnumerable<T> AutoDispose<T>(this T target)
            where T : IDisposable
        {
            try
            {
                yield return target;
            }
            finally
            {
                target.Dispose();
            }
        }
    }
}