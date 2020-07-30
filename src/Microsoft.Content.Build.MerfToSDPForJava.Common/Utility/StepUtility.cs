namespace Microsoft.Content.Build.MerfToSDPForJava.Common
{
    using System;
    using System.IO;
    using Microsoft.Content.Build.MerfToSDPForJava.Constants;

    public class StepUtility
    {
        public static string GetSDPYmlOutputPath(string outputPath)
        {
            return Path.Combine(outputPath, Constants.SDPYmlFolderName);
        }
    }
}
