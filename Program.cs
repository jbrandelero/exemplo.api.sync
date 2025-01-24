using exemplo.api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    { 
        Console.WriteLine("Configurando serviços...");
        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com"); // /todos
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError() 
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
        Console.WriteLine("HttpClient configurado.");
         
        services.AddLogging(config =>
        {
            config.AddConsole();
        });
        Console.WriteLine("Log configurado.");

        services.AddHostedService<ApiSyncService>();
        Console.WriteLine("Serviço de sincronização configurado.");
    })
    .Build();

await host.RunAsync();