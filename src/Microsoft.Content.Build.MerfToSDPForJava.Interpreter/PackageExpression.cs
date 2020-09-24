namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using System.Collections.Generic;
    using System.Linq;

    public class PackageExpression : AbstractExpression
    {
        public PackageExpression(string outputFolder)
           : base(outputFolder)
        {

        }
        public override bool Interpret(PageModel pageModel, BuildContext context)
        {
            if (!base.Valid(pageModel))
            {
                return false;
            }

            var fieldItem = pageModel.Items?.Where(item => item.Type == MemberType.Namespace)?.ToList()?.FirstOrDefault();
            if (fieldItem != null)
            {
                var namespaceSDPModel = new NamespaceSDPModel();
                namespaceSDPModel.Uid = fieldItem.Uid;
                namespaceSDPModel.FullName = fieldItem.FullName;
                namespaceSDPModel.Name = fieldItem.Name;
                namespaceSDPModel.Package = fieldItem.PackageName;
                namespaceSDPModel.Summary = fieldItem.Summary;
                namespaceSDPModel.Classes = TransferClasses(pageModel.References);
                namespaceSDPModel.Enums = TransferEnums(pageModel.References);
                namespaceSDPModel.Interfaces = TransferInterfaces(pageModel.References);
                base.Save(namespaceSDPModel, namespaceSDPModel.YamlMime, namespaceSDPModel.Uid, MemberType.Namespace.ToString());
                TrackTocItem(fieldItem, context);
                List<AbstractExpression> expressions = new List<AbstractExpression>();
                expressions.Add(new ConstructorExpression(_outputFolder, namespaceSDPModel.Uid));
                expressions.Add(new FieldExpression(_outputFolder, namespaceSDPModel.Uid));
                expressions.Add(new MethodExpression(_outputFolder, namespaceSDPModel.Uid));
                foreach (var expression in expressions)
                {
                    if (!expression.Interpret(pageModel, context))
                    {
                        break;
                    }
                }

                return false;
            }

            return true;
        }
        private IEnumerable<string> TransferClasses(List<ReferenceViewModel> referenceViewModels)
        {
            if (referenceViewModels == null)
                return null;

            var classes = referenceViewModels.Where(item => item.Type == MemberType.Class)?.Select(p => p.Uid)?.ToArray();
            if (classes == null || classes.Length == 0)
            {
                return null;
            }

            return classes;
        }
        private IEnumerable<string> TransferEnums(List<ReferenceViewModel> referenceViewModels)
        {
            if (referenceViewModels == null)
                return null;

            var enums = referenceViewModels.Where(item => item.Type == MemberType.Enum)?.Select(p => p.Uid).ToArray();
            if (enums==null ||enums.Length == 0)
            {
                return null;
            }

            return enums;
        }
        private IEnumerable<string> TransferInterfaces(List<ReferenceViewModel> referenceViewModels)
        {
            if (referenceViewModels == null)
                return null;

            var interfaces = referenceViewModels.Where(item => item.Type == MemberType.Interface)?.Select(p => p.Uid).ToArray();
            if (interfaces == null || interfaces.Length == 0)
            {
                return null;
            }

            return interfaces;
        }
    }
}
