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
        public override bool Interpret(PageModel pageModel, BuildContext context)
        {
            if (!base.Valid(pageModel))
            {
                return false;
            }

            var enumItem = pageModel.Items?.Where(item => item.Type == MemberType.Enum)?.ToList()?.FirstOrDefault();
            if (enumItem != null)
            {
                var enumSDPModel = new EnumSDPModel();
                enumSDPModel.Uid = enumItem.Uid;
                enumSDPModel.FullName = enumSDPModel.FullName;
                enumSDPModel.Name = enumItem.Name;
                enumSDPModel.Package = enumItem.PackageName;
                enumSDPModel.Summary = enumItem.Summary;
                enumSDPModel.NameWithType = enumItem.NameWithType;
                enumSDPModel.Syntax = TransferSyntax(enumItem.Syntax);
                enumSDPModel.Inheritances = ConvertStringToInlineMD(enumItem.Inheritance);
                enumSDPModel.InheritedMembers = enumItem.InheritedMembers;
                enumSDPModel.Methods = TransferMethods(pageModel.Items);
                enumSDPModel.Fields = TransferFields(pageModel.Items);
                
                base.Save(enumSDPModel, enumSDPModel.YamlMime, enumSDPModel.Uid);

                return false;
            }

            return true;
        }

        private IEnumerable<EnumField> TransferFields(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;

            var fields = articleItemYamls.Where(item => item.Type == MemberType.Field)?.ToArray();
            if (fields == null)
                return null;

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
        private IEnumerable<EnumMethod> TransferMethods(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;

            var methods = articleItemYamls.Where(item => item.Type == MemberType.Method)?.ToArray();
            if (methods == null)
                return null;

            List<EnumMethod> enumMethods = new List<EnumMethod>();
            foreach (var method in methods)
            {
                var enumMethod = new EnumMethod();
                enumMethod.FullName = method.FullName;
                enumMethod.Name = method.Name;
                enumMethod.NameWithType = method.NameWithType;
                enumMethod.Overridden = method.Overridden;
                enumMethod.Parameters = TransferParameters(method.Syntax);
                enumMethod.Summary = method.Summary;
                enumMethod.Syntax = TransferSyntax(method.Syntax);
                enumMethod.Returns = TransferReturns(method.Syntax);
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
