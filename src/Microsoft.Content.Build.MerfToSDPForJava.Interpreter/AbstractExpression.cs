namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public abstract class AbstractExpression
    {
        protected string _outputFolder;
        protected string _parentUid;
        public AbstractExpression(string outputFolder, string parentUid)
        {
            _outputFolder = outputFolder;
            _parentUid = parentUid;
        }
        public virtual bool Interpreter(PageModel pageModel, BuildContext context)
        {
            return true;
        }

        protected virtual void Save(object obj, string comment, string fileName)
        {
            if (obj == null || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            var fullFileName = fileName + Constants.Constants.YamlExtension;
            string sdpymlfilePath = Path.Combine(StepUtility.GetSDPYmlOutputPath(_outputFolder), fullFileName);
            YamlUtility.Serialize(sdpymlfilePath, obj, comment);
        }
        protected virtual bool Valid(PageModel pageModel)
        {
            if (pageModel == null || pageModel.Items == null || pageModel.Items.Count == 0)
            {
                return false;
            }

            return true;
        }

        protected Member TransforMember(ArticleItemYaml articleItemYaml)
        {
            if (articleItemYaml == null)
            {
                return null;
            }

            var member = new Member();
            member.FullName = articleItemYaml.FullName;
            member.Name = articleItemYaml.Name;
            member.NameWithType = articleItemYaml.NameWithType;
            member.Overridden = articleItemYaml.Overridden;
            member.Parameters = TransforParameters(articleItemYaml.Syntax);
            member.Returns = TransforReturns(articleItemYaml.Syntax);
            member.Syntax = TransforSyntax(articleItemYaml.Syntax);
            member.Summary = articleItemYaml.Summary;
            member.TypeParameters = TransforTypeParameters(articleItemYaml.Syntax);
            member.Uid = articleItemYaml.Uid;

            return member;
        }

        protected Return TransforReturns(SyntaxDetailViewModel syntax)
        {
            if (syntax == null || syntax.Return == null)
            {
                return null;
            }

            var returns = new Return()
            {
                Description = syntax.Return.Description,
                Type = ConvertStringToInlineMD(syntax.Return.Type)
            };

            return returns;
        }

        protected IEnumerable<Parameter> TransforParameters(SyntaxDetailViewModel syntax)
        {
            if (syntax == null || syntax.Parameters == null || syntax.Parameters.Count == 0)
            {
                return null;
            }

            var parameters = syntax.Parameters.Select(p => new Parameter() { Description = p.Description, Name = p.Name, Type = p.Type }).ToArray();

            if (parameters.Length == 0)
            {
                return null;
            }

            return parameters;
        }

        protected IEnumerable<TypeParameter> TransforTypeParameters(SyntaxDetailViewModel syntax)
        {
            if (syntax == null || syntax.TypeParameters == null || syntax.TypeParameters.Count == 0)
            {
                return null;
            }

            var typeParameters = syntax.TypeParameters.Select(p => new TypeParameter() { Description = p.Description, Name = p.Name }).ToArray();

            if (typeParameters.Length == 0)
            {
                return null;
            }

            return typeParameters;
        }

        protected List<string> ConvertStringToInlineMD(List<string> lists)
        {
            if (lists == null)
                return null;

            lists = lists.Select(s => s = s.Replace("<", "&lt;").Replace(">", "&gt;")).ToList();

            return lists;
        }

        protected string ConvertStringToInlineMD(string str)
        {
            return str?.Replace("<", "&lt;").Replace(">", "&gt;");
        }

        protected string TransforSyntax(SyntaxDetailViewModel syntax)
        {
            if (syntax == null)
                return null;
            return syntax.Content;
        }
    }
}
