using APITele.Models;


namespace APITele.HttpClients;

public interface IExternalCitizensClient
{
    public Task<Citizen[]?> GetCitizensAsync();
    public Task<Citizen?> GetCitizenByIdAsync(string id);
}
