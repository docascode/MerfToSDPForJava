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
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using YamlDotNet.Serialization;

    public class MerfToSDPGenerator : IStep
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
                        var deserializer = new Deserializer();
                        var serializer = new Serializer();

                        var filename = Path.GetFileNameWithoutExtension(p);
                        var content = File.ReadAllText(p, Encoding.UTF8);

                        if (filename == "toc")
                        {
                            
                        }
                        else
                        {
                            PageModel pageModel;
                            using (StringReader reader = new StringReader(content))
                            {
                                pageModel = YamlUtility.Deserialize<PageModel>(reader);
                            }
                            List<JaveAbstractExpression> expressions = new List<JaveAbstractExpression>();
                            expressions.Add(new JavePackageExpression(config.OutputPath, filename));
                            expressions.Add(new JaveTypeExpress(config.OutputPath, filename));
                            expressions.Add(new JaveEnumExpression(config.OutputPath, filename));

                            if (pageModel == null)
                                return;

                            foreach (var expression in expressions)
                            {
                                expression.Interpreter(pageModel,context);
                            }
                        }
                    });
            }

            return Task.FromResult(1);
        }
    }
}
