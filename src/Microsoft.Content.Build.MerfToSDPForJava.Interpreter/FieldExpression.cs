namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    public class FieldExpression : AbstractExpression
    {
        public FieldExpression(string outputFolder, string parentUid)
               : base(outputFolder, parentUid)
        {

        }
        public override bool Interpret(PageModel pageModel, BuildContext context)
        {
            if (pageModel == null || pageModel.Items == null)
            {
                return false;
            }

            var fields = pageModel.Items.Where(item => item.Type == MemberType.Field)?.ToList();
            if (fields == null || fields.Count == 0)
                return true;

            foreach (var field in fields)
            {
                var memberSDPModel = new MemberSDPModel();
                memberSDPModel.FullName = field.FullName;
                memberSDPModel.Name = field.Name;
                memberSDPModel.NameWithType = field.NameWithType;
                memberSDPModel.Uid = field.Uid;
                memberSDPModel.Type = field.Type.ToString().ToLower();
                memberSDPModel.Package = field.PackageName;
                memberSDPModel.Members = new List<Member>();

                var member = TransferMember(field);
                if (member != null)
                {
                    memberSDPModel.Members.Add(member);
                }

                memberSDPModel.PropertyToXrefString(pageModel);
                base.Save(memberSDPModel, memberSDPModel.YamlMime, memberSDPModel.Uid);
            }

            return true;
        }
    }
}
