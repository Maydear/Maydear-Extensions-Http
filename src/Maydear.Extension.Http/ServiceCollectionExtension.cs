using Maydear.Extension.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 服务注入
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 增加重试代码
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="configureClient"></param>
        /// <param name="timeoutSeconds">超时秒数</param>
        /// <returns></returns>
        public static IHttpClientBuilder AddRetryHttpClient(this IServiceCollection services, string name, Action<HttpClient> configureClient, double timeoutSeconds=60)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (configureClient == null)
            {
                throw new ArgumentNullException(nameof(configureClient));
            }
            IPolicyRegistry<string> registry = services.AddPolicyRegistry();

            var timeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds((timeoutSeconds / 3)));
            var longTimeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(timeoutSeconds));

            registry.Add("regular", timeout);
            registry.Add("long", longTimeout);

            return services.AddHttpClient(name, configureClient)
              .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(timeoutSeconds)))
              .AddPolicyHandlerFromRegistry("regular")
              .AddPolicyHandler((request) =>
              {
                  return request.Method == HttpMethod.Get ? timeout : longTimeout;
              })
              .AddPolicyHandlerFromRegistry((reg, request) =>
              {
                  return request.Method == HttpMethod.Get ?
                      reg.Get<IAsyncPolicy<HttpResponseMessage>>("regular") :
                      reg.Get<IAsyncPolicy<HttpResponseMessage>>("long");
              })
              .AddTransientHttpErrorPolicy(p => p.RetryAsync())
              .AddHttpMessageHandler(() => new RetryHandler(services.BuildServiceProvider().GetRequiredService<ILoggerFactory>()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="configureClient"></param>
        /// <param name="timeoutSeconds">超时秒数</param>
        /// <returns></returns>
        public static IServiceCollection AddHttpService<TService>(this IServiceCollection services, string name, Action<HttpClient> configureClient, double timeoutSeconds = 60) where TService : class
        {
            services.AddRetryHttpClient(name, configureClient, timeoutSeconds)
                    .AddTypedClient<TService>();

            return services;
        }
    }
}
