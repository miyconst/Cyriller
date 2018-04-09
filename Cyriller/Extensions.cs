using System;
using System.Text.RegularExpressions;

namespace Cyriller
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string Value)
        {
            return string.IsNullOrEmpty(Value);
        }

        public static bool IsNotNullOrEmpty(this string Value)
        {
            return !string.IsNullOrEmpty(Value);
        }

        public static string ReplaceRegex(this string Value, string RegexWhat, string ReplaceTo)
        {
            if (Value.IsNullOrEmpty() || RegexWhat.IsNullOrEmpty())
            {
                return Value;
            }

            return Regex.Replace(Value, RegexWhat, ReplaceTo);
        }

        public static bool RegexHasMatches(this String Value, String RegexPattern, Boolean CaseSensetive = false, Boolean MultiLine = true)
        {
            RegexOptions options = !CaseSensetive ? RegexOptions.IgnoreCase : RegexOptions.None;
            options |= MultiLine ? RegexOptions.Multiline : options;
            return Regex.IsMatch(Value, RegexPattern, options);
        }

        public static string UppercaseFirst(this string Value)
        {
            switch (Value.Length)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return Value.ToUpper();
                default:
                    return char.ToUpper(Value[0]) + Value.Substring(1);
            }
        }
    }
}
