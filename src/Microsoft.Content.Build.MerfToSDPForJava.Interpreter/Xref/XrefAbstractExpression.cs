namespace Microsoft.Content.Build.MerfToSDPForJava.Interpreter
{
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;

    public abstract class XrefAbstractExpression
    {
        public virtual bool Interpreter(PageModel pageModel, string uid, out string result)
        {
            result = null;
            return true;
        }
    }
}
