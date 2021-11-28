using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ExtensionHttpSamples
{
    class Program
    {
        public static void Main(string[] args) => Run().GetAwaiter().GetResult();

        public static async Task Run()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(b =>
            {
                b.AddFilter((category, level) => true);
                b.AddConsole();
            });

            Configure(serviceCollection);

            var services = serviceCollection.BuildServiceProvider();

            Console.WriteLine("Creating a client...");
            var github = services.GetRequiredService<GitHubService>();

            Console.WriteLine("Sending a request...");
            var data = await github.GetJson();
            Console.WriteLine("Response data:");
            Console.WriteLine(data);

            Console.WriteLine("Press the ANY key to exit...");
            Console.ReadKey();
        }


        public static void Configure(IServiceCollection services)
        {
            services.AddHttpService<GitHubService>("github", c =>
            {
                c.BaseAddress = new Uri("https://api.github.com/");

                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });
        }

    }
}
