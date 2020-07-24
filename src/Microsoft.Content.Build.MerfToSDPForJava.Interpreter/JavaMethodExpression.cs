namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts;
    using Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts;
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections.Concurrent;

    public class JavaMethodExpression : JaveAbstractExpression
    {
        private Dictionary<string, MemberSDPModel> keyValuePairs_methodOverloading = new Dictionary<string, MemberSDPModel>();
        public JavaMethodExpression(string outputFolder, string fileName)
               : base(outputFolder, fileName)
        {

        }
        public override void Interpreter(PageModel pageModel, BuildContext context)
        {
            if (pageModel == null || pageModel.Items == null)
            {
                return;
            }

            var methods = pageModel.Items.Where(item => item.Type == MemberType.Method).ToList();
            if (methods.Count == 0)
                return;

            var methodsOverLoading = methods.Where(item => item != null && item.Overload.EndsWith('*')).ToList();
            var combinationSDPModel = new MemberSDPModel();
            var tocTracker = context.GetSharedObject(Constants.Constants.ExtendedIdMappings) as ConcurrentDictionary<string, List<TocItemYaml>>;
            foreach (var item in methodsOverLoading)
            {
                var filename = RemoveOverloadingFromPropertyName(item.Uid);
               
                MemberSDPModel exist;
                if (!keyValuePairs_methodOverloading.TryGetValue(filename, out exist))
                {
                    var memberSDPModel = AutoMapArticleItemYamlToMemberSDPModel(item);
                    item.Uid = item.Overload;
                    keyValuePairs_methodOverloading.Add(filename, memberSDPModel);
                }
                else
                {
                    var member = TransforMember(item);
                    exist.Members.Add(member);
                }
            }

            
            foreach (var keyValuePair in keyValuePairs_methodOverloading)
            {
                base.Save(keyValuePair.Value, keyValuePair.Value.YamlMime, keyValuePair.Key);

                var tocItem = new List<TocItemYaml>() {new TocItemYaml{
                    Uid = keyValuePair.Value.Uid,
                    Name = keyValuePair.Value.Name,
                    Href = keyValuePair.Key + Constants.Constants.YamlExtension
                }};
                

                tocTracker.AddOrUpdate(_parentUid, tocItem, (key, oldValue) =>
                {
                    oldValue.AddRange(tocItem); 
                    return oldValue;
                });
            }


            var methodsNoOverLoading = methods.Where(item => item != null && !item.Overload.EndsWith('*')).ToList();
            foreach (var method in methodsNoOverLoading)
            {
                var memberSDPModel = new MemberSDPModel();
                memberSDPModel.FullName = method.FullName;
                memberSDPModel.Name = method.Name;
                memberSDPModel.NameWithType = method.NameWithType;
                memberSDPModel.Summary = method.Summary;

                //No metadata;
                //memberSDPModel.Metadata
                memberSDPModel.Uid = method.Uid;
                memberSDPModel.Type = method.Type;
                memberSDPModel.Package = method.PackageName;
                memberSDPModel.Members = new List<Member>();

                var member = TransforMember(method);
                if (member != null)
                {
                    memberSDPModel.Members.Add(member);
                }

                base.Save(memberSDPModel, memberSDPModel.YamlMime, memberSDPModel.Uid);

                var tocItem = new List<TocItemYaml>() { new TocItemYaml
                {
                    Uid = member.Uid,
                    Name = member.Name,
                    Href = member.Uid + Constants.Constants.YamlExtension
                } };

                tocTracker.AddOrUpdate(_parentUid, tocItem, (key, oldValue) =>
                {
                    oldValue.AddRange(tocItem);
                    return oldValue;
                });
            }

        }
        private MemberSDPModel AutoMapArticleItemYamlToMemberSDPModel(ArticleItemYaml articleItemYaml)
        {
            if (articleItemYaml == null)
                return null;

            var memberSDPModel = new MemberSDPModel();
            memberSDPModel.FullName = articleItemYaml.FullName;
            memberSDPModel.Name = articleItemYaml.Name;
            memberSDPModel.NameWithType = articleItemYaml.NameWithType;
            memberSDPModel.Summary = articleItemYaml.Summary;

            //No metadata;
            //memberSDPModel.Metadata
            memberSDPModel.Uid = articleItemYaml.Uid;
            memberSDPModel.Type = articleItemYaml.Type;
            memberSDPModel.Package = articleItemYaml.PackageName;
            memberSDPModel.Members = new List<Member>();

            var member = TransforMember(articleItemYaml);
            if (member != null)
            {
                memberSDPModel.Members.Add(member);
            }

            return memberSDPModel;
        }
    }
}
