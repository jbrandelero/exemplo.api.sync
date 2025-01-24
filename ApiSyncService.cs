using exemplo.api.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; 

namespace exemplo.api
{
    public class ApiSyncService : BackgroundService
    {
        private readonly IApiClient _apiClient;
        private readonly ILogger<ApiSyncService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public ApiSyncService(IApiClient apiClient, ILogger<ApiSyncService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Iniciando sincronização...");
                    var result = await _apiClient.GetAsync<ExemploModel[]>("todos");

                    _logger.LogInformation($"Foram encontrados {result.Length+1} registros.");
                    //não implementei nada aqui, mas o ideal seria disparar uma fila ou algo do tipo para processar os dados
                    Thread.Sleep(500);

                    _logger.LogInformation("Sincronização concluída com sucesso!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao sincronizar com a API.");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }

}
