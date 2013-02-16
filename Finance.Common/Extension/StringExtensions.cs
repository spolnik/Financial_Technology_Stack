namespace Finance.Common.Extension
{
    public static class StringExtensions
    {
        /// <summary>
        /// Check if a string is null or empty
        /// </summary>
        /// <param name="value">A string instance</param>
        /// <returns>True if the string is null or empty, otherwise false</returns>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Checks if a string is not null or empty
        /// </summary>
        /// <param name="value">A string instance</param>
        /// <returns>True if the string has a value</returns>
        public static bool IsNotEmpty(this string value)
        {
            return !value.IsEmpty();
        }

        /// <summary>
        /// Uses the string as a template and applies the specified arguments
        /// </summary>
        /// <param name="format">The format string</param>
        /// <param name="args">The arguments to pass to the format provider</param>
        /// <returns>The formatted string</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return format.IsEmpty() ? format : string.Format(format, args);
        }

        /// <summary>
        /// Converts "yes/no/true/false/0/1"
        /// </summary>
        /// <param name="txt">String to convert to boolean.</param>
        /// <returns>Boolean converted from string.</returns>
        public static bool ToBool(this string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return false;

            var trimed = txt.Trim().ToLower();
            return trimed == "yes" || trimed == "true" || trimed == "1";
        }
    }
}