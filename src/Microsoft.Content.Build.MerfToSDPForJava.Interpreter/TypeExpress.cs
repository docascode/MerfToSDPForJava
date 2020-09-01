namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using System.Collections.Generic;
    using System.Linq;

    public class TypeExpress : AbstractExpression
    {
        public TypeExpress(string outputFolder, string fileName)
            : base(outputFolder, fileName)
        {

        }
        public override bool Interpret(PageModel pageModel, BuildContext context)
        {
            if (!base.Valid(pageModel))
            {
                return false;
            }

            var typeItem = pageModel.Items?.Where(item => item.Type == MemberType.Interface || item.Type == MemberType.Class)?.ToList()?.FirstOrDefault();
            if (typeItem != null)
            {
                var objType = new TypeSDPModel();
                objType.Constructors = TransferConstructors(pageModel.Items);
                objType.Fields = TransferFields(pageModel.Items);
                objType.Methods = TransferMethods(pageModel.Items);
                objType.FullName = typeItem.FullName;
                objType.Inheritances = ConvertStringToInlineMD(typeItem.Inheritance);
                objType.InheritedMembers = typeItem.InheritedMembers;
                objType.Name = typeItem.Name;
                objType.NameWithType = typeItem.NameWithType;
                objType.Package = typeItem.PackageName;
                objType.Summary = typeItem.Summary;
                objType.Syntax = TransferSyntax(typeItem.Syntax);
                objType.Type = typeItem.Type?.ToString().ToLower();
                objType.Implements = typeItem.Implements;
                objType.TypeParameters = TransferTypeParameters(typeItem.Syntax);
                objType.Uid = typeItem.Uid;
                
                objType.PropertyToXrefString(pageModel);
                base.Save(objType, objType.YamlMime, objType.Uid);

                List<AbstractExpression> expressions = new List<AbstractExpression>();
                expressions.Add(new ConstructorExpression(_outputFolder, objType.Uid));
                expressions.Add(new FieldExpression(_outputFolder, objType.Uid));
                expressions.Add(new MethodExpression(_outputFolder, objType.Uid));
                foreach (var expression in expressions)
                {
                    if (!expression.Interpret(pageModel, context))
                    {
                        break;
                    }
                }

                return false;
            }

            return true;
        }
        private IEnumerable<string> TransferConstructors(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;

            var constructors = articleItemYamls.Where(item => item.Type == MemberType.Constructor)?.Select(p => p.Uid)?.ToArray();
            if (constructors == null || constructors.Length == 0)
            {
                return null;
            }

            return constructors;
        }
        private IEnumerable<string> TransferFields(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;

            var fields = articleItemYamls.Where(item => item.Type == MemberType.Field)?.Select(p => p.Uid)?.ToArray();
            if (fields == null || fields.Length == 0)
            {
                return null;
            }

            return fields;
        }
        private IEnumerable<string> TransferMethods(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;

            var methods = articleItemYamls.Where(item => item.Type == MemberType.Method)?.Select(p => p.Uid)?.ToArray();
            if (methods == null || methods.Length == 0)
            {
                return null;
            }

            return methods;
        }

    }
}
