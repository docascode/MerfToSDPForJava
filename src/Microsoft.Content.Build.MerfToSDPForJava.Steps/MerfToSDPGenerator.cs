namespace Microsoft.Content.Build.MerfToSDPForJava.Steps
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.Common.Utility;
    using Microsoft.Content.Build.MerfToSDPForJava.Constants;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using Microsoft.Content.Build.MerfToSDPForJava.Interpreter;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YamlDotNet.Serialization;

    public class MerfToSDPGenerator : IStep
    {
        public string StepName
        {
            get { return "MerfToSDP"; }
        }
        public Task RunAsync(BuildContext context)
        {
            var config = context.GetSharedObject(Constants.Config) as ConfigModel;
            if (config == null)
            {
                throw new ApplicationException(string.Format("Key: {0} doesn't exist in build context", Constants.Config));
            }

            string sdpYmlFolder = StepUtility.GetSDPYmlOutputPath(config.OutputPath);
            if (Directory.Exists(sdpYmlFolder))
            {
                Directory.Delete(sdpYmlFolder, recursive: true);
            }

            Directory.CreateDirectory(sdpYmlFolder);

            var inputPaths = config.InputPaths;
            foreach (var inputPath in inputPaths)
            {
                Directory.EnumerateFiles(inputPath, "*.yml").AsParallel().ForAll(
                p =>
                {
                    var filename = Path.GetFileNameWithoutExtension(p);
                    try
                    {
                        var deserializer = new Deserializer();
                        var serializer = new Serializer();
                        var content = File.ReadAllText(p, Encoding.UTF8);

                        if (filename != "toc")
                        {
                            PageModel pageModel;
                            using (StringReader reader = new StringReader(content))
                            {
                                pageModel = YamlUtility.Deserialize<PageModel>(reader);
                            }
                            List<AbstractExpression> expressions = new List<AbstractExpression>();
                            expressions.Add(new PackageExpression(config.OutputPath, filename));
                            expressions.Add(new TypeExpress(config.OutputPath, filename));
                            expressions.Add(new EnumExpression(config.OutputPath, filename));

                            if (pageModel == null)
                                return;

                            foreach (var expression in expressions)
                            {
                                if (!expression.Interpreter(pageModel, context))
                                {
                                    break;
                                }
                            }

                            ConsoleLogger.WriteLine(
                              new LogEntry
                              {
                                  Phase = StepName,
                                  Level = LogLevel.Info,
                                  Message = $"Succeed to interpret Merf file: {filename}.yml.",
                              });
                        }
                    }
                    catch (Exception ex)
                    {
                        var sb = new StringBuilder();
                        sb.Append($"Fail to interpret Merf file: {filename}.yml.");
                        sb.AppendLine("     ");
                        sb.Append($"Exception: { ex}");
                        ConsoleLogger.WriteLine(
                               new LogEntry
                               {
                                   Phase = StepName,
                                   Level = LogLevel.Error,
                                   Message = sb.ToString()
                               });
                    }

                });
            }

            return Task.FromResult(1);
        }
    }
}
