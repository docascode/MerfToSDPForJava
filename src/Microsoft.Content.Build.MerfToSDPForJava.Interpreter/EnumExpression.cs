namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using System.Collections.Generic;
    using System.Linq;

    public class EnumExpression : AbstractExpression
    {
        public EnumExpression(string outputFolder, string fileName)
              : base(outputFolder, fileName)
        {

        }
        public override bool Interpreter(PageModel pageModel, BuildContext context)
        {
            if (!base.Valid(pageModel))
            {
                return false;
            }

            var enumItem = pageModel.Items.Where(item => item.Type == MemberType.Enum)?.ToList().FirstOrDefault();
            if (enumItem != null)
            {
                var enumSDPModel = new EnumSDPModel();
                enumSDPModel.Uid = enumItem.Uid;
                enumSDPModel.FullName = enumSDPModel.FullName;
                enumSDPModel.Name = enumItem.Name;
                enumSDPModel.Package = enumItem.PackageName;
                enumSDPModel.Summary = enumItem.Summary;
                enumSDPModel.NameWithType = enumItem.NameWithType;
                enumSDPModel.Syntax = TransforSyntax(enumItem.Syntax);
                enumSDPModel.Inheritances = ConvertStringToInlineMD(enumItem.Inheritance);
                enumSDPModel.InheritedMembers = enumItem.InheritedMembers;
                enumSDPModel.Methods = TransforMethods(pageModel.Items);
                enumSDPModel.Fields = TransforFields(pageModel.Items);

                base.Save(enumSDPModel, enumSDPModel.YamlMime, enumSDPModel.Uid);

                return false;
            }

            return true;
        }

        private IEnumerable<EnumField> TransforFields(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var fields = articleItemYamls.Where(item => item.Type == MemberType.Field).ToArray();
            List<EnumField> enumFields = new List<EnumField>();
            foreach (var field in fields)
            {
                var enumField = new EnumField();
                enumField.FullName = field.FullName;
                enumField.Name = field.Name;
                enumField.NameWithType = field.NameWithType;
                enumField.Summary = field.Summary;
                enumField.Uid = field.Uid;
                enumFields.Add(enumField);
            }

            if (enumFields.Count == 0)
            {
                return null;
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
                enumMethod.Syntax = TransforSyntax(method.Syntax);
                enumMethod.Returns = TransforReturns(method.Syntax);
                enumMethod.Uid = method.Uid;
                enumMethods.Add(enumMethod);
            }

            if (enumMethods.Count == 0)
            {
                return null;
            }

            return enumMethods;
        }

    }
}
