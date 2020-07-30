namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    public class ConstructorExpression : AbstractExpression
    {
        //private static readonly Regex constructorPropertyRegex = new Regex(@"([\w].*)\(.*\)", RegexOptions.Compiled);
        public ConstructorExpression(string outputFolder, string fileName)
            : base(outputFolder, fileName)
        {

        }
        public override bool Interpreter(PageModel pageModel, BuildContext context)
        {
            if (pageModel == null || pageModel.Items == null)
            {
                return false;
            }

            var constructors = pageModel.Items.Where(item => item.Type == MemberType.Constructor).ToList();
            if (constructors.Count == 0)
                return true;

            var tocTracker = context.GetSharedObject(Constants.Constants.ExtendedIdMappings) as ConcurrentDictionary<string, List<TocItemYaml>>;

            var constructorsOverLoading = constructors.Where(item => item.Overload != null && item.Overload.EndsWith('*')).ToList();
            var combinationSDPModel = new MemberSDPModel();
            for (var i = 0; i < constructorsOverLoading.Count; i++)
            {
                var constructor = constructorsOverLoading[i];
                if (i == 0)
                {
                    combinationSDPModel = new MemberSDPModel();
                    combinationSDPModel.FullName = constructor.FullName.RemoveFromValue("(");
                    combinationSDPModel.Name = constructor.Name.RemoveFromValue("(");
                    combinationSDPModel.NameWithType = constructor.NameWithType.RemoveFromValue("(");
                    combinationSDPModel.Uid = constructor.Overload;
                    combinationSDPModel.Type = constructor.Type?.ToString().ToLower();
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
                    var filename = constructor.Overload.Remove(constructor.Overload.Length - 1);

                    combinationSDPModel.PropertyToXrefString(pageModel);

                    base.Save(combinationSDPModel, combinationSDPModel.YamlMime, filename);

                    var tocItem = new List<TocItemYaml>() { new TocItemYaml()
                    {
                        Uid = constructor.Overload,
                        Name =  combinationSDPModel.Name.RemoveFromValue("("),
                        Type =MemberType.Constructor.ToString().ToLower()
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

                //No metadata;
                //memberSDPModel.Metadata
                memberSDPModel.Uid = constructor.Uid;
                memberSDPModel.Type = constructor.Type.ToString().ToLower();
                memberSDPModel.Package = constructor.PackageName;
                memberSDPModel.Members = new List<Member>();

                var member = TransforMember(constructor);
                if (member != null)
                {
                    memberSDPModel.Members.Add(member);
                }

                memberSDPModel.PropertyToXrefString(pageModel);

                base.Save(memberSDPModel, memberSDPModel.YamlMime, memberSDPModel.Uid);

                var tocItem = new List<TocItemYaml>() { new TocItemYaml()
                {
                    Uid = memberSDPModel.Uid,
                    Name = memberSDPModel.Name.RemoveFromValue("("),
                    Type = MemberType.Constructor.ToString().ToLower()
                } };

                tocTracker.AddOrUpdate(_parentUid, tocItem, (key, oldValue) =>
                {
                    oldValue.AddRange(tocItem);
                    return oldValue;
                });
            }

            return true;
        }
    }
}
