namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using System.Linq;
    public class SingletonXrefExpression : XrefAbstractExpression
    {
        public override bool Interpreter(PageModel pageModel, string uid, out string result)
        {
            if (pageModel == null || pageModel.References == null || string.IsNullOrEmpty(uid))
            {
                result = null;
                return false;
            }

            var references = pageModel.References.Where(p => p.Uid == uid)?.ToList();
            var singletonReference = references?.FirstOrDefault();
            if (singletonReference == null)
            {
                result = null;
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

            result = null;
            return true;
        }
    }
}
