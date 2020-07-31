namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using System.Linq;
    using System.Text;

    public class ComplexXrefExpression : XrefAbstractExpression
    {
        public override bool Interpreter(PageModel pageModel, string uid, out string result)
        {
            result = null;
            if (pageModel == null || pageModel.References == null || string.IsNullOrEmpty(uid))
            {
                return false;
            }

            var references = pageModel.References.Where(p => p.Uid == uid)?.ToList();
            var reference = references?.FirstOrDefault();
            if (reference == null)
            {
                return false;
            }

            if (reference.SpecForJava == null)
            {
                return true;
            }

            if (reference.SpecForJava.Count > 1)
            {
                var sb = new StringBuilder();
                foreach (var item in reference.SpecForJava)
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

            result = null;
            return true;
        }

    }
}
