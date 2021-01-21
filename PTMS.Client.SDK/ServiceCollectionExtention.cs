using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using PTMS.Client.SDK.Models;
using System;
using System.Net.Http;
using System.Text;

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
        public static IServiceCollection AddPtmsClient(this IServiceCollection services, IConfiguration configuration, string sectionName)
        {
            var config = configuration.GetSection(sectionName).Get<PtmsConfiguration>();
            services.AddHttpClient("ptms-client", options => {
                var byteArray = Encoding.ASCII.GetBytes($"{config.UserName}:" +
                                                        $"{config.Password}");
                options.BaseAddress = new Uri(config.BaseUrl);
                options.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }).AddPolicyHandler(GetRetryPolicy());
            services.AddSingleton<IPTMSClient, PTMSClient>();
            return services;
        }

        /// <summary>
        /// Add Ptms client to use in dependency injection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddPtmsClient(this IServiceCollection services, PtmsConfiguration config)
        {
            services.AddHttpClient("ptms-client", options => {
                var byteArray = Encoding.ASCII.GetBytes($"{config.UserName}:" +
                                                        $"{config.Password}");
                options.BaseAddress = new Uri(config.BaseUrl);
                options.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }).AddPolicyHandler(GetRetryPolicy());
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
