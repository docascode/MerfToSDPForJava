namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class MethodExpression : AbstractExpression
    {
        private Dictionary<string, MemberSDPModel> keyValuePairs_methodOverloading = new Dictionary<string, MemberSDPModel>();
        public MethodExpression(string outputFolder, string parentUid)
               : base(outputFolder, parentUid)
        {

        }
        public override bool Interpret(PageModel pageModel, BuildContext context)
        {
            if (pageModel == null || pageModel.Items == null)
            {
                return false;
            }

            var methods = pageModel.Items.Where(item => item.Type == MemberType.Method)?.ToList();
            if (methods == null || methods.Count == 0)
                return true;

            var methodsOverLoading = methods.Where(item => item != null && item.Overload != null && item.Overload.EndsWith('*'))?.ToList();
            if (methodsOverLoading == null)
            {
                return true;
            }

            foreach (var item in methodsOverLoading)
            {
                var filename = item.Overload.Remove(item.Overload.Length - 1);

                MemberSDPModel exist;
                if (!keyValuePairs_methodOverloading.TryGetValue(filename, out exist))
                {
                    var memberSDPModel = AutoMapArticleItemYamlToMemberSDPModel(item);
                    memberSDPModel.Uid = item.Overload;
                    keyValuePairs_methodOverloading.Add(filename, memberSDPModel);
                }
                else
                {
                    var member = TransferMember(item);
                    exist.Members.Add(member);
                }
            }

            foreach (var keyValuePair in keyValuePairs_methodOverloading)
            {
                keyValuePair.Value.PropertyToXrefString(pageModel);
                base.Save(keyValuePair.Value, keyValuePair.Value.YamlMime, keyValuePair.Key, keyValuePair.Value.Type);
                TrackTocItem(new ArticleItemYaml()
                {
                    Uid = keyValuePair.Value.Uid,
                    Name = keyValuePair.Value.Name.RemoveFromValue("("),
                    Type = MemberType.Method
                }, context);
            }

            return true;
        }
        private MemberSDPModel AutoMapArticleItemYamlToMemberSDPModel(ArticleItemYaml articleItemYaml)
        {
            if (articleItemYaml == null)
                return null;

            var memberSDPModel = new MemberSDPModel();
            memberSDPModel.FullName = articleItemYaml.FullName;
            memberSDPModel.Name = articleItemYaml.Name;
            memberSDPModel.NameWithType = articleItemYaml.NameWithType.RemoveFromValue("(");
            memberSDPModel.Uid = articleItemYaml.Uid;
            memberSDPModel.Type = articleItemYaml.Type.ToString().ToLower();
            memberSDPModel.Package = articleItemYaml.PackageName;
            memberSDPModel.Members = new List<Member>();

            var member = TransferMember(articleItemYaml);
            if (member != null)
            {
                memberSDPModel.Members.Add(member);
            }

            return memberSDPModel;
        }
    }
}
