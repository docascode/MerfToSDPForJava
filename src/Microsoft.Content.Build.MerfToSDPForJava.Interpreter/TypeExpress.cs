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
        public override bool Interpreter(PageModel pageModel, BuildContext context)
        {
            if (!base.Valid(pageModel))
            {
                return false;
            }

            var typeItem = pageModel.Items.Where(item => item.Type == MemberType.Interface || item.Type == MemberType.Class).ToList().FirstOrDefault();
            if (typeItem != null)
            {
                var objType = new TypeSDPModel();

                objType.Constructors = TransforConstructors(pageModel.Items);
                objType.Fields = TransforFields(pageModel.Items);
                objType.Methods = TransforMethods(pageModel.Items);

                objType.FullName = typeItem.FullName;
                objType.Inheritances = ConvertStringToInlineMD(typeItem.Inheritance);
                objType.InheritedMembers = typeItem.InheritedMembers;
                objType.Name = typeItem.Name;
                objType.NameWithType = typeItem.NameWithType;
                objType.Package = typeItem.PackageName;
                objType.Summary = typeItem.Summary;
                objType.Syntax = TransforSyntax(typeItem.Syntax);
                objType.Type = typeItem.Type?.ToString().ToLower();
                objType.TypeParameters = TransforTypeParameters(typeItem.Syntax);
                objType.Uid = typeItem.Uid;

                base.Save(objType, objType.YamlMime, objType.Uid);


                return false;
            }

            return true;
        }
        private IEnumerable<string> TransforConstructors(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var constructors = articleItemYamls.Where(item => item.Type == MemberType.Constructor).Select(p => p.Uid).ToArray();

            if (constructors.Length == 0)
            {
                return null;
            }

            return constructors;
        }
        private IEnumerable<string> TransforFields(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var fields = articleItemYamls.Where(item => item.Type == MemberType.Field).Select(p => p.Uid).ToArray();

            if (fields.Length == 0)
            {
                return null;
            }

            return fields;
        }
        private IEnumerable<string> TransforMethods(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var methods = articleItemYamls.Where(item => item.Type == MemberType.Method).Select(p => p.Uid).ToArray();

            if (methods.Length == 0)
            {
                return null;
            }

            return methods;
        }

    }
}
