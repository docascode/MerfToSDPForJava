namespace MerfToSDPForJava
{
    using Microsoft.Content.Build.MerfToSDPForJava.Common;
    using Microsoft.Content.Build.MerfToSDPForJava.Constants;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.ManagedReference;
    using Microsoft.Content.Build.MerfToSDPForJava.DataContracts.SDP;
    using Microsoft.Content.Build.MerfToSDPForJava.Interpreter;
    using Microsoft.Content.Build.MerfToSDPForJava.Steps;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    class Program
    {
        static int Main(string[] args)
        {
            var opt = new CommandLineOptions();
            var context = new BuildContext();

            ///Store up the newly generated SDP file, then according to the trace, TOC is generated.
            var hierarchyProvider = new HierarchyProvider();

            var procedure = new StepCollection(
                new MerfToSDPGenerator(),
                new TocYamlGenerator()
                );

            var status = 1;
            var watch = Stopwatch.StartNew();
            try
            {
                if (opt.Parse(args))
                {
                    var _config = new ConfigModel()
                    {
                        InputPaths = new List<string>() { opt.SourceFolder },
                        OutputPath = opt.OutputFolder
                    };

                    context.SetSharedObject(Constants.Config, _config);
                    context.SetSharedObject(Constants.HierarchyProvider, hierarchyProvider);

                    procedure.RunAsync(context).Wait();
                    status = 0;
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Intenal Several Error: {ex}");
            }
            finally
            {
                watch.Stop();
            }
           
            var statusString = status == 0 ? "Succeeded" : "Failed";
            Console.WriteLine($" the files have been generated to { opt.OutputFolder}/SDPYml");
            Console.WriteLine($"{statusString} in {watch.ElapsedMilliseconds} milliseconds.");
            return status;
        }
    }
}
