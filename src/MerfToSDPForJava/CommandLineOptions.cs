namespace MerfToSDPForJava
{
    using Mono.Options;
    using System.Collections.Generic;

    public class CommandLineOptions
    {
        public string SourceFolder = null;
        public string OutputFolder = null;
        public string Language = "java";

        List<string> Extras = null;
        OptionSet _options = null;
        public CommandLineOptions()
        {
            _options = new OptionSet {
                { "s|source=", "[Required] the output folder containing the Merf Yaml files.", s => SourceFolder = s.NormalizePath() },
                { "o|output=", "[Required] the output folder to put SDP yml files.", o => OutputFolder = o.NormalizePath() },
                ///{ "l|lan=", "the folder path containing the overwrite MD files for metadata.", s => Language = s.NormalizePath() }
            };
        }

        public bool Parse(string[] args)
        {
            Extras = _options.Parse(args);
            if (string.IsNullOrEmpty(SourceFolder) || string.IsNullOrEmpty(OutputFolder) || string.IsNullOrEmpty(Language))
            {
                //PrintUsage();
                return false;
            }

            return true;
        }

        //private void PrintUsage()
        //{
        //    OPSLogger.LogUserError(LogCode.ECMA2Yaml_Command_Invalid, null);
        //    Console.WriteLine("Usage: ECMA2Yaml.exe <Options>");
        //    _options.WriteOptionDescriptions(Console.Out);
        //}

        private string NormalizeRepoUrl(string repoUrl)
        {
            if (!string.IsNullOrEmpty(repoUrl))
            {
                repoUrl = repoUrl.TrimEnd('/');
                if (repoUrl.EndsWith(".git"))
                {
                    repoUrl = repoUrl.Substring(0, repoUrl.Length - 4);
                }
            }
            return repoUrl;
        }
    }
}
