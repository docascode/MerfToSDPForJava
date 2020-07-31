namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using System.Linq;

    public static class Extensions
    {
        public static string RemoveFromValue(this string str, string startValue)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(startValue))
                return str;

            var indexOfValue = str.IndexOf(startValue);
            if (indexOfValue != -1)
            {
                str = str.Remove(indexOfValue);
            }

            return str;
        }

    }
}
