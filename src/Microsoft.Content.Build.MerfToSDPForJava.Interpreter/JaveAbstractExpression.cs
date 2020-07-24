namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.Common.Utility;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts;
    using Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public abstract class JaveAbstractExpression
    {
        protected string _outputFolder;
        protected string _parentUid;
        public JaveAbstractExpression(string outputFolder,string parentUid)
        {
            _outputFolder = outputFolder;
            _parentUid = parentUid;
        }
        public virtual void Interpreter(PageModel pageModel, BuildContext context)
        {
           
        }

        //protected virtual void Save(string str,string fileName)
        //{
        //    if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(fileName))
        //    {
        //        return;
        //    }

        //    var fullFileName = fileName + Constants.Constants.YamlExtension;
        //    string sdpymlfilePath =Path.Combine(StepUtility.GetSDPYmlOutputPath(_outputFolder), fileName+Constants.Constants.YamlExtension);
        //    StreamWriter sw = File.CreateText(sdpymlfilePath);
        //    sw.Write(str);
        //    sw.Flush();
        //    sw.Close();
        //}
        protected virtual void Save(object obj, string comment, string fileName)
        {
            if (obj==null || string.IsNullOrEmpty(fileName))
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
            member.Summary = articleItemYaml.Summary;
            member.TypeParameters = TransforTypeParameters(articleItemYaml.Syntax);
            member.Uid = articleItemYaml.Uid;

            return member;
        }

        protected IEnumerable<Return> TransforReturns(SyntaxDetailViewModel syntax)
        {
            if (syntax == null || syntax.Return == null)
            {
                return null;
            }

            var item = new Return()
            {
                Description = syntax.Return.Description,
                Type = syntax.Return.Type
            };

            var returns = new List<Return>() { item };
            return returns;
        }

        protected IEnumerable<Parameter> TransforParameters(SyntaxDetailViewModel syntax)
        {
            if (syntax == null || syntax.Parameters == null || syntax.Parameters.Count == 0)
            {
                return null;
            }

            var parameters = syntax.Parameters.Select(p => new Parameter() { Description = p.Description, Name = p.Name, Type = p.Type }).ToArray();
            return parameters;
        }

        protected IEnumerable<TypeParameter> TransforTypeParameters(SyntaxDetailViewModel syntax)
        {
            if (syntax == null || syntax.TypeParameters == null || syntax.TypeParameters.Count == 0)
            {
                return null;
            }

            var typeParameters = syntax.TypeParameters.Select(p => new TypeParameter() { Description = p.Description, Name = p.Name }).ToArray();
            return typeParameters;
        }

        protected string TransforSyntax(ArticleItemYaml articleItemYaml)
        {
            if (articleItemYaml == null)
                return null;
           return articleItemYaml.Syntax.Content;
        }
        protected string RemoveOverloadingFromPropertyName(string str)
        {
            if (str == null)
            {
                return str;
            }

            var index = str.IndexOf('(');

            if (index != -1)
            {
                str = str.Remove(index);
            }

            return str;
        }
    }
}
