using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExtensionHttpSamples
{
    public class GitHubService
    {
        public GitHubService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        public Task<JObject> GetJson()
        {
            return HttpClient.GetAsync<JObject>("/");
        }
    }
}
