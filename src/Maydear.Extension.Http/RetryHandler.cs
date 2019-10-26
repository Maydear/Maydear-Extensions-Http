using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Maydear.Extension.Http
{
    internal class RetryHandler : DelegatingHandler
    {
        public int RetryCount { get; set; } = 5;

        private readonly ILogger logger;

        public RetryHandler(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<RetryHandler>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            for (int i = 0; i < RetryCount; i++)
            {
                try
                {
                    return await base.SendAsync(request, cancellationToken);
                }
                catch (HttpRequestException hre) when (i == RetryCount - 1)
                {
                    logger.LogWarning(hre, $"HttpRequestException Retry “{i}”：{request.RequestUri.ToString()}");
                    throw;
                }
                catch (HttpRequestException hre)
                {
                    // Retry
                    logger.LogWarning(hre, $"HttpRequestException Retry Delay “{50 * (i + 1)}”");
                    await Task.Delay(TimeSpan.FromMilliseconds(50 * (i + 1)));
                }
            }

            throw null;
        }
    }
}
