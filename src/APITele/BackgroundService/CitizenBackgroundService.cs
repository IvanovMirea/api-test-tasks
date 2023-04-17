using APITele.Repositories;
using APITele.Models;
using APITele.HttpClients;
using Newtonsoft.Json;
using Microsoft.OpenApi.Writers;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace APITele.BackgroundService;

public class CitizenBackgroundService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IExternalCitizensClient _externalCitizensClient;
    public CitizenBackgroundService(IServiceScopeFactory serviceScopeFactory, IExternalCitizensClient externalCitizensClient)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _externalCitizensClient = externalCitizensClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ICitizenRepository>();
        var citizens = await _externalCitizensClient.GetCitizensAsync();
        if (citizens == null)
        {
            throw new NullReferenceException("No citizens to process");
        }
        foreach(var citizen in citizens)
        {
            var externalCitizen = await _externalCitizensClient.GetCitizenByIdAsync(citizen.Id);
            if (await repository.GetByIdAsync(citizen.Id) != null)
                continue;
            if (externalCitizen == null)
            {
                throw new ArgumentException("Person with this id not found");
            }
            citizen.Age = externalCitizen.Age;
            await repository.AddAsync(citizen);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
