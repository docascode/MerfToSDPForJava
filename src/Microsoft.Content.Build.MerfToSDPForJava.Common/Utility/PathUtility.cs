namespace Microsoft.Content.Build.MerfToSDPForJava.Common
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    public static class PathUtility
    {
        private static readonly Regex UriWithProtocol = new Regex(@"^\w{2,}\:", RegexOptions.Compiled);
        private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();
        public static bool IsRelativePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            // IsWellFormedUriString does not try to escape characters such as '\' ' ', '(', ')' and etc. first. Use TryCreate instead
            Uri absoluteUri;
            if (Uri.TryCreate(path, UriKind.Absolute, out absoluteUri))
            {
                return false;
            }

            if (UriWithProtocol.IsMatch(path))
            {
                return false;
            }

            foreach (var ch in InvalidPathChars)
            {
                if (path.Contains(ch))
                {
                    return false;
                }
            }

            return !Path.IsPathRooted(path);
        }
        public static string GetAbsolutePath(string basePath, string relativePath)
        {
            if (basePath == null)
            {
                throw new ArgumentNullException(nameof(basePath));
            }

            if (relativePath == null)
            {
                throw new ArgumentNullException(nameof(relativePath));
            }

            Uri resultUri = new Uri(new Uri(basePath), new Uri(relativePath, UriKind.Relative));
            return resultUri.LocalPath;
        }
    }
}
