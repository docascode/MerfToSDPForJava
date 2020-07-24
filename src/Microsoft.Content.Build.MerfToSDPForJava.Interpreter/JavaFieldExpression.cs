namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts;
    using Microsoft.Content.Build.MerfToSDPForJava.SDP.DataContracts;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class JavaFieldExpression : JaveAbstractExpression
    {
        public JavaFieldExpression(string outputFolder, string parentUid)
               : base(outputFolder, parentUid)
        {

        }
        public override void Interpreter(PageModel pageModel, BuildContext context)
        {
            if (pageModel == null || pageModel.Items == null)
            {
                return;
            }

            var fields = pageModel.Items.Where(item => item.Type == MemberType.Field).ToList();
            if (fields.Count == 0)
                return;

            var tocTracker = context.GetSharedObject(Constants.Constants.ExtendedIdMappings) as ConcurrentDictionary<string, List<TocItemYaml>>;
            foreach (var field in fields)
            {
                var memberSDPModel = new MemberSDPModel();
                memberSDPModel.FullName = field.FullName;
                memberSDPModel.Name = field.Name;
                memberSDPModel.NameWithType = field.NameWithType;
                memberSDPModel.Summary = field.Summary;

                //No metadata;
                //memberSDPModel.Metadata
                memberSDPModel.Uid = field.Uid;
                memberSDPModel.Type = field.Type;
                memberSDPModel.Package = field.PackageName;
                memberSDPModel.Members = new List<Member>();

                var member = TransforMember(field);
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
    }
}
