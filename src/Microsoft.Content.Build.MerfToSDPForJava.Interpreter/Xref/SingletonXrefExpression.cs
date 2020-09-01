namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using System.Linq;
    using System.Text;
    public class SingletonXrefExpression : XrefAbstractExpression
    {
        public override bool Interpret(PageModel pageModel, string uid, out string result)
        {
            result = null;
            if (pageModel == null || pageModel.References == null || string.IsNullOrEmpty(uid))
            {
                return false;
            }

            var references = pageModel.References.Where(p => p.Uid == uid)?.ToList();
            var singletonReference = references?.FirstOrDefault();
            if (singletonReference == null)
            {
                return false;
            }

            if (singletonReference.SpecForJava == null)
            {
                result = Common.YamlUtility.EncodeXrefLink(singletonReference.Name, singletonReference.Uid, singletonReference.Uid);
                return false;
            }

            if (singletonReference.SpecForJava.Count == 1)
            {
                var specForjave = singletonReference.SpecForJava.FirstOrDefault();
                result = Common.YamlUtility.EncodeXrefLink(specForjave.Name, specForjave.Uid, specForjave.FullName);
                return false;
            }

            return true;
        }

        public override bool Interpreting(PageModel pageModel, string uid, out string result)
        {
            result = null;

            if (pageModel == null || string.IsNullOrEmpty(uid))
            {
                return false;
            }

            var singletonReference = pageModel.References?.Where(p => p.Uid == uid)?.ToList().FirstOrDefault();

            if (singletonReference == null)
            {
                result = Common.YamlUtility.EncodeXrefLink(uid);
                return false;
            }

            if (singletonReference.SpecForJava == null || singletonReference.SpecForJava.Count == 0)
            {
                result = Common.YamlUtility.EncodeXrefLink(singletonReference?.Name, singletonReference.Uid);
                return false;
            }

            if (singletonReference.SpecForJava.Count == 1)
            {
                var specForjave = singletonReference.SpecForJava.FirstOrDefault();
                result = Common.YamlUtility.EncodeXrefLink(specForjave.Name, specForjave.Uid, specForjave.FullName);
                return false;
            }

            if (singletonReference.SpecForJava.Count > 1)
            {
                var sb = new StringBuilder();
                foreach (var item in singletonReference.SpecForJava)
                {
                    if (item.Uid == null)
                    {
                        var replaceStr = item.Name.Replace("<", "&lt;").Replace(">", "&gt;");
                        sb.Append(replaceStr);
                    }
                    else
                    {
                        sb.Append(Common.YamlUtility.EncodeXrefLink(item.Name, item.Uid, item.FullName));
                    }
                }

                result = sb.ToString();
                return false;
            }

            return true;
        }
    }
}
