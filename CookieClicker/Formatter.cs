using System;
using System.Globalization;

namespace CookieClicker
{
    internal static class Formatter
    {
        private static readonly string[] abbreviations = new string[] { "M", "B", "T", "Q", "Qt", "Sx", "Sp", "Oc", "No", "De" };

        public static string FormatCookies(double cookies, string append)
        {
            if (append == null)
                append = cookies <= 1 && cookies != 0 ? "cookie" : "cookies";
            append = " " + append;

            if (cookies < 1_000_000)
            {
                //make cookies over 1000 have a space between every 3 digits
                return cookies.ToString(cookies >= 1000 ? "0 000" : "", CultureInfo.InvariantCulture) + append;
            }
            else
            {
                int order = ((int)Math.Log10(cookies) / 3) - 2;
                double value = cookies / Math.Pow(10, (order + 1) * 3);

                if (order >= abbreviations.Length)
                    return "Too many" + append;

                return value.ToString("0,000", CultureInfo.InvariantCulture) + abbreviations[order] + append;
            }
        }
    }
}
