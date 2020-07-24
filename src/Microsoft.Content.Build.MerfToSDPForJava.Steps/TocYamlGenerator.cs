namespace Microsoft.Content.Build.MerfToSDPForJava.Steps
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.Common.Utility;
    using Microsoft.Content.Build.MerfToSDPForJava.Constants;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts;
    using Microsoft.Content.Build.MerfToSDPForJava.Interpreter;
    using Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class TocYamlGenerator : IStep
    {
        public string StepName
        {
            get { return "MerfTransferSDP"; }
        }
        public Task RunAsync(BuildContext context)
        {
            var config = context.GetSharedObject(Constants.Config) as ConfigModel;
            if (config == null)
            {
                throw new ApplicationException(string.Format("Key: {0} doesn't exist in build context", Constants.Config));
            }

            string sdpYmlFolder = StepUtility.GetSDPYmlOutputPath(config.OutputPath);
            if (!Directory.Exists(sdpYmlFolder))
            {
                throw new ApplicationException(string.Format("SDPYmlOutputPath: {0} doesn't exist.", sdpYmlFolder));
            }

            var inputPaths = config.InputPaths;
            foreach (var inputPath in inputPaths)
            {
                var tocFilePath = Path.Combine(inputPath, "toc.yml");
                if (!File.Exists(tocFilePath))
                {
                    throw new ApplicationException(string.Format("toc.yml: toc.yml doesn't exist in the {0} directory.", inputPath));
                }

                var filename = Path.GetFileNameWithoutExtension(tocFilePath);
                var content = File.ReadAllText(tocFilePath, Encoding.UTF8);
                TocYaml toc;
                using (StringReader reader = new StringReader(content))
                {
                    toc = YamlUtility.Deserialize<TocYaml>(reader);
                }
                JavaTocExpression javaTocExpression = new JavaTocExpression(config.OutputPath);
                javaTocExpression.Interpreter(toc, context);
            }

            return Task.FromResult(1);
        }
    }
}
