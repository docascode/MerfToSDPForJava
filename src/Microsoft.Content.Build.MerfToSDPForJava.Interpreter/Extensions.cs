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
                new SingletonXrefExpression()
            };

        public static string RemoveFromValue(this string str, string startValue)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(startValue))
                return str;

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

            typeSDPModel.Implements = typeSDPModel.Implements?.Select(i => { return i = InterpretXref(pageModel, i) ?? i; });
            typeSDPModel.Inheritances = typeSDPModel.Inheritances?.Select(i => { return i = InterpretXref(pageModel, i) ?? i; });

        }
        public static void PropertyToXrefString(this MemberSDPModel memberSDPModel, PageModel pageModel)
        {
            if (memberSDPModel == null || memberSDPModel.Members == null)
                return;

            foreach (var member in memberSDPModel.Members)
            {             
                member.Parameters = member.Parameters?.Select(p => { return new Parameter() { Name = p.Name, Type = InterpretXref(pageModel, p.Type) ?? p.Type ?? null, Description = p.Description }; });

                if (member.Returns != null)
                {
                    member.Returns.Type = InterpretXref(pageModel, member.Returns.Type) ?? member.Returns.Type;
                }

                if (member.Exceptions != null)
                {
                    foreach (var subMember in member.Exceptions)
                    {
                        subMember.Type = InterpretXref(pageModel, subMember.Type) ?? subMember.Type;
                    }
                }
            }
        }
        public static void PropertyToXrefString(this EnumSDPModel enumSDPModel, PageModel pageModel)
        {
            if (enumSDPModel == null)
                return;

            enumSDPModel.Implements = enumSDPModel.Implements?.Select(i => { return i = InterpretXref(pageModel, i) ?? i; });
            enumSDPModel.Inheritances = enumSDPModel.Inheritances?.Select(i => { return i = InterpretXref(pageModel, i) ?? i; });

            if (enumSDPModel.Methods != null)
            {
                foreach (var method in enumSDPModel.Methods)
                {
                    method.Parameters = method.Parameters?.Select(p => { return new Parameter() { Name = p.Name, Type = InterpretXref(pageModel, p.Type ?? null) ?? p.Type, Description = p.Description }; });

                    if (method.Returns != null)
                    {
                        method.Returns.Type = InterpretXref(pageModel, method.Returns.Type) ?? method.Returns.Type;
                    }

                    if (method.Exceptions != null)
                    {
                        foreach (var subMember in method.Exceptions)
                        {
                            subMember.Type = InterpretXref(pageModel, subMember.Type) ?? subMember.Type;
                        }
                    }
                }
            }
        }
        private static string InterpretXref(PageModel pageModel, string uid)
        {
            var result = uid;
            foreach (var child in xrefExpressionList)
            {
                if (!child.Interpreting(pageModel, uid, out result))
                {
                    break;
                }
            }

            return result;
        }

    }
}
