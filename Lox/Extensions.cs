namespace Lox
{
    public static class Extensions
    {
        public static string JavaStyleSubstring(this string s, int beginIndex, int endIndex)
        {
            // simulates Java substring function
            int len = endIndex - beginIndex;
            return s.Substring(beginIndex, len);
        }
    }
}
