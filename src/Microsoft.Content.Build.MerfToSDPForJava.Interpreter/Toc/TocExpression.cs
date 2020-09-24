namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using System.IO;

    public class TocExpression
    {
        protected string _outputFolder;
        public TocExpression(string outputFolder)
        {
            _outputFolder = outputFolder;
        }
        public void Interpret(BuildContext context)
        {
            var hierarchyProvider = context.GetSharedObject(Constants.Constants.HierarchyProvider) as HierarchyProvider;
            if (hierarchyProvider == null)
            {
                return;
            }

            var tocItemYamls = hierarchyProvider.BuildToc();
            Save(tocItemYamls, null, "toc");
        }

        protected virtual void Save(object obj, string comment, string fileName)
        {
            if (obj == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            var fullFileName = fileName + Constants.Constants.YamlExtension;
            string sdpymlfilePath = Path.Combine(StepUtility.GetSDPYmlOutputPath(_outputFolder), fullFileName);
            YamlUtility.Serialize(sdpymlfilePath, obj, comment);
        }
    }
}
