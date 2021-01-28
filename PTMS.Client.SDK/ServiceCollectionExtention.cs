using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace PTMS.Client.SDK
{
    public static class ServiceCollectionExtention
    {

        /// <summary>
        /// Add Ptms client to use in dependency injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static IServiceCollection AddPtmsClient(this IServiceCollection services, Action<HttpClient> options)
        {
            services.AddHttpClient("ptms-client", options).AddPolicyHandler(GetRetryPolicy());
            services.AddSingleton<IPTMSClient, PTMSClient>();
            return services;
        }


        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                });
        }
    }
}
