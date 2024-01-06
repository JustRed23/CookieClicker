using System;
using System.Globalization;

namespace CookieClicker
{
    /// <summary>
    /// Formats numbers to be more readable
    /// </summary>
    internal static class Formatter
    {
        private static readonly string[] abbreviations = new string[] { "M", "B", "T", "Q", "Qt", "Sx", "Sp", "Oc", "No", "De" };

        /// <summary>
        /// Formats a number to be more readable, adds a suffix if the number is too large and spaces out every 3 digits
        /// </summary>
        /// <param name="number">The number you want to format</param>
        /// <param name="append">The text you want to append after the formatted number, defaults to 'cookie' or 'cookies' depending on the number</param>
        /// <returns>A properly formatted number</returns>
        public static string FormatCookies(double number, string append)
        {
            if (append == null)
                append = number <= 1 && number != 0 ? "cookie" : "cookies";
            append = " " + append;

            if (number < 1_000_000)
            {
                //make cookies over 1000 have a space between every 3 digits
                return number.ToString(number >= 1000 ? "0 000" : "", CultureInfo.InvariantCulture) + append;
            }
            else
            {
                int order = ((int)Math.Log10(number) / 3) - 2;
                double value = number / Math.Pow(10, (order + 1) * 3);

                if (order >= abbreviations.Length)
                    return "Too many" + append;

                return value.ToString("0,000", CultureInfo.InvariantCulture) + abbreviations[order] + append;
            }
        }
    }
}
