namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts;
    using Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts;
    using System.Collections.Generic;
    using System.Linq;

    public class JaveEnumExpression : JaveAbstractExpression
    {
        public JaveEnumExpression(string outputFolder, string fileName)
              : base(outputFolder, fileName)
        {

        }
        public override void Interpreter(PageModel pageModel, BuildContext context)
        {
            if (!base.Valid(pageModel))
            {
                return;
            }

            var enumItem = pageModel.Items.Where(item => item.Type == MemberType.Enum).ToList().FirstOrDefault();
            if (enumItem != null)
            {
                var enumSDPModel = new EnumSDPModel();
                enumSDPModel.Uid = enumItem.Uid;
                enumSDPModel.FullName = enumSDPModel.FullName;
                //namespaceSDPModel.Metadata = pageModel.Metadata;
                enumSDPModel.Name = enumItem.Name;
                enumSDPModel.Package = enumItem.PackageName;
                enumSDPModel.Summary = enumItem.Summary;
                enumSDPModel.NameWithType = enumItem.NameWithType;
                var syntax = TransforSyntax(enumItem);
                if (!string.IsNullOrEmpty(syntax))
                {
                    enumSDPModel.Syntax = TransforSyntax(enumItem);
                }

                enumSDPModel.Inheritances = enumItem.Inheritance;
                enumSDPModel.InheritedMembers = enumItem.InheritedMembers;
                //enumSDPModel.Implements
                enumSDPModel.Methods = TransforMethods(pageModel.Items);
                enumSDPModel.Fields= TransforFields(pageModel.Items);

                base.Save(enumSDPModel, enumSDPModel.YamlMime, enumSDPModel.Uid);

                List<JaveAbstractExpression> expressions = new List<JaveAbstractExpression>();
                expressions.Add(new JaveConstructorExpression(_outputFolder, enumSDPModel.Uid));
                expressions.Add(new JavaFieldExpression(_outputFolder, enumSDPModel.Uid));
                expressions.Add(new JavaMethodExpression(_outputFolder, enumSDPModel.Uid));

                foreach (var expression in expressions)
                {
                    expression.Interpreter(pageModel, context);
                }

                return;
            }
        }
        private IEnumerable<EnumField> TransforFields(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var fields = articleItemYamls.Where(item => item.Type == MemberType.Field).ToArray();
            List<EnumField> enumFields = new List<EnumField>();
            foreach (var field in enumFields)
            {
                var enumField = new EnumField();
                enumField.FullName = field.FullName;
                enumField.Name = field.Name;
                enumField.NameWithType = field.NameWithType;
                enumField.Summary = field.Summary;
                enumField.Uid = field.Uid;
                enumFields.Add(enumField);

            }

            return enumFields;
        }
        private IEnumerable<EnumMethod> TransforMethods(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var methods = articleItemYamls.Where(item => item.Type == MemberType.Method).ToArray();
            List<EnumMethod> enumMethods = new List<EnumMethod>();
            foreach (var method in methods)
            {
                var enumMethod = new EnumMethod();
                enumMethod.FullName = method.FullName;
                enumMethod.Name = method.Name;
                enumMethod.NameWithType = method.NameWithType;
                enumMethod.Overridden = method.Overridden;
                enumMethod.Parameters = TransforParameters(method.Syntax);
                enumMethod.Summary = method.Summary;
                enumMethod.Syntax = TransforSyntax(method);
                enumMethod.Uid = method.Uid;
                enumMethods.Add(enumMethod);

            }

            return enumMethods;
        }

    }
}
