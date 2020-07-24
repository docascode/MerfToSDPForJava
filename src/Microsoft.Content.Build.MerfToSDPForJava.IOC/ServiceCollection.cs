namespace Microsoft.Content.Build.MerfToSDPForJava.IOC
{
    using Microsoft.Content.Build.MerfToSDPForJava.Steps;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    public static class Register
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException("services");
            services.AddSingleton(factory =>
            {
                Func<string, IStep> accesor = key =>
                {
                    if (key.Equals(nameof(StepCollection)))
                    {
                        return factory.GetService<StepCollection>();
                    }
                    else if (key.Equals(nameof(MerfTransferSDP)))
                    {
                        return factory.GetService<MerfTransferSDP>();
                    }
                    else
                    {
                        throw new ArgumentException($"Not Support key : {key}");
                    }
                };
                return accesor;
            });
            
            return services;
        }
    }
}
