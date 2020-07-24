namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts;
    using Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts;
    using System.Collections.Generic;
    using System.Linq;
    public class JaveTypeExpress : JaveAbstractExpression
    {
        public JaveTypeExpress(string outputFolder, string fileName) 
            : base(outputFolder, fileName)
        { 
        
        }
        public override void Interpreter(PageModel pageModel, BuildContext context)
        {
            if (!base.Valid(pageModel))
            {
                return;
            }

            var typeItem = pageModel.Items.Where(item => item.Type == MemberType.Interface || item.Type == MemberType.Class).ToList().FirstOrDefault();
            if (typeItem!=null )
            {
                var objType = new TypeSDPModel();
                //obj.Artifact = item;

                objType.Constructors = TransforConstructors(pageModel.Items);
                objType.Fields = TransforFields(pageModel.Items);
                objType.Methods = TransforMethods(pageModel.Items);
                //objType.Implements = item.;

                objType.FullName = typeItem.FullName;
                objType.Inheritances = typeItem.Inheritance;
                objType.InheritedMembers = typeItem.InheritedMembers;
                objType.Metadata = pageModel.Metadata;
                
                objType.Name = typeItem.Name;
                objType.NameWithType = typeItem.NameWithType;
                objType.Package = typeItem.PackageName;
                objType.Summary = typeItem.Summary;
                objType.Syntax = TransforSyntax(typeItem);
                objType.Type = typeItem.Type;
                objType.TypeParameters = TransforTypeParameters(typeItem.Syntax);
                objType.Uid = typeItem.Uid;

                base.Save(objType, objType.YamlMime, objType.Uid);

                List<JaveAbstractExpression> expressions = new List<JaveAbstractExpression>();
                expressions.Add(new JaveConstructorExpression(_outputFolder, objType.Uid));
                expressions.Add(new JavaFieldExpression(_outputFolder, objType.Uid));
                expressions.Add(new JavaMethodExpression(_outputFolder, objType.Uid));

                foreach (var expression in expressions)
                {
                    expression.Interpreter(pageModel, context);
                }

                return;
            }
        }

        private IEnumerable<string> TransforConstructors(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var constructors = articleItemYamls.Where(item => item.Type == MemberType.Constructor).Select(p=> p.Uid).ToArray();
            return constructors;
        }
        private IEnumerable<string> TransforFields(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var fields = articleItemYamls.Where(item => item.Type == MemberType.Field).Select(p => p.Uid).ToArray();
            return fields;
        }
        private IEnumerable<string> TransforMethods(List<ArticleItemYaml> articleItemYamls)
        {
            if (articleItemYamls == null)
                return null;
            var methods = articleItemYamls.Where(item => item.Type == MemberType.Method).Select(p => p.Uid).ToArray();
            return methods;
        }
        
    }
}
