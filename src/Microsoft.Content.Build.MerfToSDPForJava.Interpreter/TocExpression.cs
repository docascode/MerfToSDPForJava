namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.Common.Utility;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;

    public class TocExpression
    {
        protected string _outputFolder;
        public TocExpression(string outputFolder)

        {
            _outputFolder = outputFolder;
        }
        public void Interpreter(TocYaml tocItemYamls, BuildContext context)
        {
            var tocTracker = context.GetSharedObject(Constants.Constants.ExtendedIdMappings) as ConcurrentDictionary<string, List<TocItemYaml>>;

            UpdateTocYaml(tocItemYamls, tocTracker);

            Save(tocItemYamls, null, "toc");
        }

        public void UpdateTocYaml(TocYaml tocItemYamls, ConcurrentDictionary<string, List<TocItemYaml>> tocTracker)
        {
            if (tocItemYamls == null)
                return;

            foreach (var tocItem in tocItemYamls)
            {
                if (tocTracker.ContainsKey(tocItem.Uid))
                {
                    if (tocItem.Items == null)
                    {
                        tocItem.Items = new TocYaml();
                    }

                    tocItem.Items.AddRange(tocTracker[tocItem.Uid]);
                }

                UpdateTocYaml(tocItem.Items, tocTracker);
            }
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
