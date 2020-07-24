namespace Microsoft.Content.Build.MerfToSDPForJava.Steps
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class StepCollection : IStep
    {
        public string StepName
        {
            get { return "StepCollection"; }
        }

        private IEnumerable<IStep> _steps;

        public StepCollection(IEnumerable<IStep> steps)
        {
            if (steps == null)
            {
                throw new ArgumentNullException("steps");
            }
            _steps = steps;
        }

        public StepCollection(params IStep[] steps)
        {
            _steps = steps;
        }

        public async Task RunAsync(BuildContext context)
        {
            foreach (IStep step in _steps)
            {
                try
                {
                    ConsoleLogger.WriteLine(new LogEntry { Phase = step.StepName, Level = LogLevel.Info, Message = "Start ..." });
                    await step.RunAsync(context);
                }
                catch (Exception ex)
                {
                    ConsoleLogger.WriteLine(
                        new LogEntry
                        {
                            Phase = step.StepName,
                            Level = LogLevel.Error,
                            Message = ex.Message,
                            Data = ex,
                        });
                    // rethrow exception
                    throw;
                }
            }
        }
    }
}
