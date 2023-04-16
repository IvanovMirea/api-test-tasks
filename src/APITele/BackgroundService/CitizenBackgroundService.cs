using APITele.Repositories;
using APITele.Models;
using Newtonsoft.Json;
using Microsoft.OpenApi.Writers;

namespace APITele.BackgroundService;

public class CitizenBackgroundService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    public CitizenBackgroundService(IServiceScopeFactory serviceScopeFactory, IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var repository = _serviceScopeFactory.CreateScope().ServiceProvider.GetService<CitizenRepository>();
        var httpClient = _httpClientFactory.CreateClient();
        var citizens = await httpClient.GetFromJsonAsync<Citizen[]>("https://testlodtask20172.azurewebsites.net/task");
        foreach(Citizen citizen in citizens)
        {
            var citizenJson = await httpClient.GetFromJsonAsync<Citizen>($"https://testlodtask20172.azurewebsites.net/task/{citizen.Id}");
            if (repository.GetById(citizenJson.Id) != null)
                continue;
            repository.Add(citizenJson);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
