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
        public ConstructorExpression(string outputFolder, string fileName)
            : base(outputFolder, fileName)
        {

        }
        public override bool Interpret(PageModel pageModel, BuildContext context)
        {
            if (pageModel == null || pageModel.Items == null)
            {
                return false;
            }

            var constructors = pageModel.Items.Where(item => item.Type == MemberType.Constructor)?.ToList();
            if (constructors == null || constructors.Count == 0)
                return true;

            var constructorsOverLoading = constructors.Where(item => item != null && item.Overload != null && item.Overload.EndsWith('*'))?.ToList();
            if (constructorsOverLoading == null)
            {
                return true;
            }

            var tocTracker = context.GetSharedObject(Constants.Constants.ExtendedIdMappings) as ConcurrentDictionary<string, List<TocItemYaml>>;
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

                var member = TransferMember(constructor);
                if (member != null)
                {
                    combinationSDPModel.Members.Add(member);
                }

                if (i == constructors.Count - 1)
                {
                    combinationSDPModel.PropertyToXrefString(pageModel);
                    var filename = constructor.Overload.Remove(constructor.Overload.Length - 1);
                    base.Save(combinationSDPModel, combinationSDPModel.YamlMime, filename, combinationSDPModel.Type);
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

            var constructorsNoOverLoading = constructors.Where(item => item != null && item.Overload != null && !item.Overload.EndsWith('*'))?.ToList();
            if (constructorsNoOverLoading != null)
            {
                return true;
            }

            foreach (var constructor in constructorsNoOverLoading)
            {
                var memberSDPModel = new MemberSDPModel();
                memberSDPModel.FullName = constructor.FullName.RemoveFromValue("(");
                memberSDPModel.Name = constructor.Name.RemoveFromValue("(");
                memberSDPModel.NameWithType = constructor.NameWithType.RemoveFromValue("(");
                memberSDPModel.Uid = constructor.Uid;
                memberSDPModel.Type = constructor.Type.ToString().ToLower();
                memberSDPModel.Package = constructor.PackageName;
                memberSDPModel.Members = new List<Member>();

                var member = TransferMember(constructor);
                if (member != null)
                {
                    memberSDPModel.Members.Add(member);
                }

                memberSDPModel.PropertyToXrefString(pageModel);
                base.Save(memberSDPModel, memberSDPModel.YamlMime, memberSDPModel.Uid, memberSDPModel.Type); var tocItem = new List<TocItemYaml>() { new TocItemYaml()
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
