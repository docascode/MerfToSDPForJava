namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts;
    using Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts;
    using System.Collections.Generic;
    using System.Linq;

    public class JavePackageExpression : JaveAbstractExpression
    {
        public JavePackageExpression(string outputFolder, string fileName)
           : base(outputFolder, fileName)
        {

        }
        public override void Interpreter(PageModel pageModel, BuildContext context)
        {
            if (!base.Valid(pageModel))
            {
                return;
            }

            var fieldItem = pageModel.Items.Where(item => item.Type == MemberType.Namespace).ToList().FirstOrDefault();
            if (fieldItem != null)
            {
                var namespaceSDPModel = new NamespaceSDPModel();
                namespaceSDPModel.Uid = fieldItem.Uid;
                namespaceSDPModel.FullName = fieldItem.FullName;
                //namespaceSDPModel.Metadata = pageModel.Metadata;
                namespaceSDPModel.Name = fieldItem.Name;
                namespaceSDPModel.Package = fieldItem.PackageName;
                namespaceSDPModel.Summary = fieldItem.Summary;
                namespaceSDPModel.Classes = TransforClasses(pageModel.Items);
                namespaceSDPModel.Enums = TransforEnums(pageModel.Items);
                namespaceSDPModel.Interfaces = TransforInterfaces(pageModel.Items);

                base.Save(namespaceSDPModel, namespaceSDPModel.YamlMime, namespaceSDPModel.Uid);

                List<JaveAbstractExpression> expressions = new List<JaveAbstractExpression>();
                expressions.Add(new JaveConstructorExpression(_outputFolder, namespaceSDPModel.Uid));
                expressions.Add(new JavaFieldExpression(_outputFolder, namespaceSDPModel.Uid));
                expressions.Add(new JavaMethodExpression(_outputFolder, namespaceSDPModel.Uid));
                foreach (var expression in expressions)
                {
                    expression.Interpreter(pageModel, context);
                }

                return;
            }
        }
        private IEnumerable<string> TransforClasses(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var classes = articleItemYamls.Where(item => item.Type == MemberType.Class).Select(p => p.Uid).ToArray();
            return classes;
        }
        private IEnumerable<string> TransforEnums(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var enums = articleItemYamls.Where(item => item.Type == MemberType.Enum).Select(p => p.Uid).ToArray();
            return enums;
        }
        private IEnumerable<string> TransforInterfaces(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var interfaces = articleItemYamls.Where(item => item.Type == MemberType.Interface).Select(p => p.Uid).ToArray();
            return interfaces;
        }
    }
}
