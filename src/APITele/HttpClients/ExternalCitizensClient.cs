using APITele.Models;
using System.Net.Http;

namespace APITele.HttpClients;

public class ExternalCitizensClient : IExternalCitizensClient
{
    private readonly HttpClient _httpClient;
    public ExternalCitizensClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<Citizen?> GetCitizenByIdAsync(string id) => 
        await _httpClient.GetFromJsonAsync<Citizen>($"https://testlodtask20172.azurewebsites.net/task/{id}");
    

    public async Task<Citizen[]?> GetCitizensAsync() => 
        await _httpClient.GetFromJsonAsync<Citizen[]>("https://testlodtask20172.azurewebsites.net/task");

}
