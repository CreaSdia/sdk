using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crea_CaSdkComercial.Code.Business
{
    public static class StringExtensions
    {
        public static bool EqualIgnoreCase(this string value, string compareTo)
        {
            if (value == null)
            {
                return false;
            }
            return value.Trim().Equals(compareTo.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }

        public static string FisrtCharLower(this string value)
        {
            return string.IsNullOrEmpty(value) ? value : char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static string FisrtCharUpper(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            return char.ToUpperInvariant(value[0]) + value.Substring(1);
        }

        public static string SplitInWords(this string value)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            var result = r.Replace(value, " ").ToLower();
            result = result.FisrtCharUpper();
            return result;
        }

        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return (source ?? "").IndexOf(toCheck ?? "", StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        public static bool ContainsWords(this string value, string words)
        {
            if (value == null)
                return false;

            var searchInArray = value.ToLower().Split(' ').ToList();

            var searhArray = words.ToLower().Split(' ');

            var count = searhArray.Count(search => searchInArray.Any(e => e.ContainsIgnoreCase(search)));

            return count >= searhArray.Count();
        }

        public static string NumberToWords(this int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static int TryConvertToInt(this string value)
        {
            int result;
            int.TryParse(value, out result);
            return result;
        }

        public static string ChangeDecimalSeparatorToPont(this string source)
        {
            var decimalSeparator = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            return source.Replace(decimalSeparator, ".");
        }
    }
}
