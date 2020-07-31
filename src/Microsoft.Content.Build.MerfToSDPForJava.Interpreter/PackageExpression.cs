namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using System.Collections.Generic;
    using System.Linq;

    public class PackageExpression : AbstractExpression
    {
        public PackageExpression(string outputFolder, string fileName)
           : base(outputFolder, fileName)
        {

        }
        public override bool Interpreter(PageModel pageModel, BuildContext context)
        {
            if (!base.Valid(pageModel))
            {
                return false;
            }

            var fieldItem = pageModel.Items.Where(item => item.Type == MemberType.Namespace)?.ToList()?.FirstOrDefault();
            if (fieldItem != null)
            {
                var namespaceSDPModel = new NamespaceSDPModel();
                namespaceSDPModel.Uid = fieldItem.Uid;
                namespaceSDPModel.FullName = fieldItem.FullName;
                namespaceSDPModel.Name = fieldItem.Name;
                namespaceSDPModel.Package = fieldItem.PackageName;
                namespaceSDPModel.Summary = fieldItem.Summary;
                namespaceSDPModel.Classes = TransforClasses(pageModel.References);
                namespaceSDPModel.Enums = TransforEnums(pageModel.References);
                namespaceSDPModel.Interfaces = TransforInterfaces(pageModel.References);

                base.Save(namespaceSDPModel, namespaceSDPModel.YamlMime, namespaceSDPModel.Uid);

                return false;
            }

            return true;
        }
        private IEnumerable<string> TransforClasses(List<ReferenceViewModel> referenceViewModels)
        {
            if (referenceViewModels == null)
                return null;
            var classes = referenceViewModels.Where(item => item.Type == MemberType.Class).Select(p => p.Uid).ToArray();
            if (classes.Length == 0)
            {
                return null;
            }
            return classes;
        }
        private IEnumerable<string> TransforEnums(List<ReferenceViewModel> referenceViewModels)
        {
            if (referenceViewModels == null)
                return null;
            var enums = referenceViewModels.Where(item => item.Type == MemberType.Enum).Select(p => p.Uid).ToArray();
            if (enums.Length == 0)
            {
                return null;
            }
            return enums;
        }
        private IEnumerable<string> TransforInterfaces(List<ReferenceViewModel> referenceViewModels)
        {
            if (referenceViewModels == null)
                return null;
            var interfaces = referenceViewModels.Where(item => item.Type == MemberType.Interface).Select(p => p.Uid).ToArray();
            if (interfaces.Length == 0)
            {
                return null;
            }
            return interfaces;
        }
    }
}
