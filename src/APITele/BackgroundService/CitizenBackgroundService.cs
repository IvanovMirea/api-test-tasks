using APITele.Repositories;
using APITele.Models;
using Newtonsoft.Json;


namespace APITele.BackgroundService;

public class CitizenBackgroundService : IHostedService
{
    private readonly CitizenRepository _citizenRepository;
    private readonly HttpClient _httpClient;
    public CitizenBackgroundService(CitizenRepository citizenRepository, HttpClient httpClient, IServiceScopeFactory serviceScopeFactory)
    {
        _citizenRepository = citizenRepository;
        _httpClient = httpClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var citizens = await _httpClient.GetFromJsonAsync<Citizen[]>("https://testlodtask20172.azurewebsites.net/task");
        foreach(Citizen citizen in citizens)
        {
            var citizenJson = await _httpClient.GetFromJsonAsync<Citizen>($"https://testlodtask20172.azurewebsites.net/task/{citizen.Id}");
            if (_citizenRepository.GetById(citizenJson.Id) != null)
                continue;
            _citizenRepository.Add(citizenJson);
        }

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
