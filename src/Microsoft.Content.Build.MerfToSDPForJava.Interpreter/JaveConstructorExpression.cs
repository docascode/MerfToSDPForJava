namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts;
    using Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    public class JaveConstructorExpression : JaveAbstractExpression
    {
        //private static readonly Regex constructorPropertyRegex = new Regex(@"([\w].*)\(.*\)", RegexOptions.Compiled);
        public JaveConstructorExpression(string outputFolder, string fileName)
            : base(outputFolder, fileName)
        {

        }
        public override void Interpreter(PageModel pageModel, BuildContext context)
        {
            if (pageModel == null || pageModel.Items == null)
            {
                return;
            }

            var constructors = pageModel.Items.Where(item => item.Type == MemberType.Constructor).ToList();
            if (constructors.Count == 0)
                return;

            var tocTracker = context.GetSharedObject(Constants.Constants.ExtendedIdMappings) as ConcurrentDictionary<string, List<TocItemYaml>>;

            var constructorsOverLoading = constructors.Where(item => item.Overload != null && item.Overload.EndsWith('*')).ToList();
            var combinationSDPModel = new MemberSDPModel();
            for (var i = 0; i < constructorsOverLoading.Count; i++)
            {
                var constructor = constructorsOverLoading[i];
                if (i == 0)
                {
                    combinationSDPModel.FullName = RemoveOverloadingFromPropertyName(constructor.FullName);
                    combinationSDPModel.Name = RemoveOverloadingFromPropertyName(constructor.Name);
                    combinationSDPModel.NameWithType = RemoveOverloadingFromPropertyName(constructor.NameWithType);
                    //memberSDPModel.Summary = RemoveMethodOverloadingFromPropertyName(constructor.Summary);

                    //No metadata;
                    //memberSDPModel.Metadata
                    combinationSDPModel.Uid = constructor.Overload;
                    combinationSDPModel.Type = constructor.Type;
                    combinationSDPModel.Package = constructor.PackageName;
                    combinationSDPModel.Members = new List<Member>();
                }

                var member = TransforMember(constructor);
                if (member != null)
                {
                    combinationSDPModel.Members.Add(member);
                }

                if (i == constructors.Count - 1)
                {
                    var filename = RemoveOverloadingFromPropertyName(constructor.Uid);
                    base.Save(combinationSDPModel, combinationSDPModel.YamlMime, filename);


                    var tocItem = new List<TocItemYaml>() { new TocItemYaml()
                    {
                        Uid = combinationSDPModel.Uid,
                        Name = combinationSDPModel.Name,
                        Href = combinationSDPModel.Uid + Constants.Constants.YamlExtension
                    } };

                    tocTracker.AddOrUpdate(_parentUid, tocItem, (key, oldValue) =>
                    {
                        oldValue.AddRange(tocItem);
                        return oldValue;
                    });
                }
            }

            var constructorsNoOverLoading = constructors.Where(item => (item == null) || (item.Overload != null && !item.Overload.EndsWith('*'))).ToList();
            foreach (var constructor in constructorsNoOverLoading)
            {
                var memberSDPModel = new MemberSDPModel();
                memberSDPModel.FullName = constructor.FullName;
                memberSDPModel.Name = constructor.Name;
                memberSDPModel.NameWithType = constructor.NameWithType;
                memberSDPModel.Summary = constructor.Summary;

                //No metadata;
                //memberSDPModel.Metadata
                memberSDPModel.Uid = constructor.Uid;
                memberSDPModel.Type = constructor.Type;
                memberSDPModel.Package = constructor.PackageName;
                memberSDPModel.Members = new List<Member>();

                var member = TransforMember(constructor);
                if (member != null)
                {
                    memberSDPModel.Members.Add(member);
                }

                base.Save(memberSDPModel, memberSDPModel.YamlMime, memberSDPModel.Uid);

                var tocItem = new List<TocItemYaml>() { new TocItemYaml()
                {
                    Uid = memberSDPModel.Uid,
                    Name = memberSDPModel.Name,
                    Href = memberSDPModel.Uid + Constants.Constants.YamlExtension
                } };

                tocTracker.AddOrUpdate(_parentUid, tocItem, (key, oldValue) =>
                {
                    oldValue.AddRange(tocItem);
                    return oldValue;
                });
            }
        }
    }
}
