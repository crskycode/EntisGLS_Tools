namespace CSXToolPlus.Extensions
{
    public static class StringExtensions
    {
        public static string Escape(this string s)
        {
            return s.Replace("\r", "\\r")
                    .Replace("\n", "\\n")
                    .Replace("\t", "\\t");
        }

        public static string Unescape(this string s)
        {
            return s.Replace("\\r", "\r")
                    .Replace("\\n", "\n")
                    .Replace("\\t", "\t");
        }
    }
}
