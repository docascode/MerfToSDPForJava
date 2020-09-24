namespace Microsoft.Content.Build.MerfToSDPForJava.Steps
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.Constants;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using Microsoft.Content.Build.MerfToSDPForJava.Interpreter;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class TocYamlGenerator : IStep
    {
        public string StepName
        {
            get { return "GenerateToc"; }
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

            try
            {
                TocExpression javaTocExpression = new TocExpression(config.OutputPath);
                javaTocExpression.Interpret(context);

                ConsoleLogger.WriteLine(
                        new LogEntry
                        {
                            Phase = StepName,
                            Level = LogLevel.Info,
                            Message = $"Succeed to interpret Merf file: Toc.yml.",
                        });
            }
            catch (Exception ex)
            {
                ConsoleLogger.WriteLine(
                           new LogEntry
                           {
                               Phase = StepName,
                               Level = LogLevel.Error,
                               Message = $"Fail to interpret Merf file: Toc.yml.Exception: {ex}"
                           });
            }

            return Task.FromResult(1);
        }
    }
}
