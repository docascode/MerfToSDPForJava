namespace Microsoft.Content.Build.MerfToSDPForJava.Steps
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using System.Threading.Tasks;
    public interface IStep
    {
        string StepName { get; }
        Task RunAsync(BuildContext context);
    }
}
