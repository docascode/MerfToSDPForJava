namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using System.Collections.Generic;
    using System.Linq;

    public static class Extensions
    {
        private static readonly List<XrefAbstractExpression> xrefExpressionList = new List<XrefAbstractExpression>()
            {
                new SingletonXrefExpression(),
                new ComplexXrefExpression()
            };
        public static string RemoveFromValue(this string str, string startValue)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            if (string.IsNullOrEmpty(startValue))
            {
                return str;
            }

            var indexOfValue = str.IndexOf(startValue);
            if (indexOfValue != -1)
            {
                str = str.Remove(indexOfValue);
            }

            return str;
        }
        public static void PropertyToXrefString(this TypeSDPModel typeSDPModel, PageModel pageModel)
        {
            if (typeSDPModel == null)
                return;
           
            typeSDPModel.Implements?.Select(i => { return i = InterpreterXref(pageModel, i) ?? i; });
            typeSDPModel.Inheritances?.Select(i => { return i = InterpreterXref(pageModel, i) ?? i; });
            typeSDPModel.InheritedMembers?.Select(i => { return i = InterpreterXref(pageModel, i) ?? i; });
         
        }

        public static void PropertyToXrefString(this MemberSDPModel memberSDPModel, PageModel pageModel)
        {
            if (memberSDPModel == null || memberSDPModel.Members == null)
                return;

            foreach (var member in memberSDPModel.Members)
            {
                member.Overridden =InterpreterXref(pageModel, member.Overridden) ?? (member.Overridden??null);
                member.Parameters = member.Parameters?.Select(p => { return new Parameter() { Name = p.Name, Type = InterpreterXref(pageModel, p.Type) ?? p.Type??null, Description = p.Description }; });

                if(member.Returns != null)
                {
                    member.Returns.Type = InterpreterXref(pageModel, member.Returns.Type) ?? member.Returns.Type;
                }
            }
        }
        public static void PropertyToXrefString(this EnumSDPModel enumSDPModel, PageModel pageModel)
        {
            if (enumSDPModel == null)
                return;
            enumSDPModel.Implements?.Select(i => { return i = InterpreterXref(pageModel, i) ?? i; });
            enumSDPModel.Inheritances?.Select(i => { return i = InterpreterXref(pageModel, i) ?? i; });
            enumSDPModel.InheritedMembers?.Select(i => { return i = InterpreterXref(pageModel, i) ?? i; });

            if (enumSDPModel.Methods != null)
            {
                foreach (var method in enumSDPModel.Methods)
                {
                    method.Overridden= InterpreterXref(pageModel, method.Overridden) ?? method.Overridden?? null;
                    method.Parameters = method.Parameters?.Select(p => { return new Parameter() { Name = p.Name, Type = InterpreterXref(pageModel, p.Type?? null) ?? p.Type, Description = p.Description }; });

                    if (method.Returns != null)
                    {
                        method.Returns.Type = InterpreterXref(pageModel, method.Returns.Type) ?? method.Returns.Type;
                    }
                }
            }
        }

        private static string InterpreterXref(PageModel pageModel, string uid)
        {
            var result = uid;
            foreach (var child in xrefExpressionList)
            {
                if (!child.Interpreter(pageModel, uid, out result))
                {
                    break;
                }
            }

            return result;
        }
    }
}
