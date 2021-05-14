using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ExtensionHttpSamples
{
    public class GitHubService
    {
        public GitHubService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        public Task<Object> GetJson()
        {
            return HttpClient.GetAsync<Object>("/");
        }
    }
}
