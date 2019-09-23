using Maydear.Extension.Http;
using Microsoft.Extensions.Configuration;
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
        /// <returns></returns>
        public static IHttpClientBuilder AddRetryHttpClient(this IServiceCollection services, string name, Action<HttpClient> configureClient)
        {
            if (services == null)
            {
                throw new ArgumentNullException("services");
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (configureClient == null)
            {
                throw new ArgumentNullException("configureClient");
            }
            IPolicyRegistry<string> registry = services.AddPolicyRegistry();

            Polly.Timeout.TimeoutPolicy<HttpResponseMessage> timeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
            Polly.Timeout.TimeoutPolicy<HttpResponseMessage> longTimeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));

            registry.Add("regular", timeout);
            registry.Add("long", longTimeout);

            return services.AddHttpClient(name, configureClient)
              .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)))
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
              .AddHttpMessageHandler(() => new RetryHandler());
        }
    }
}
