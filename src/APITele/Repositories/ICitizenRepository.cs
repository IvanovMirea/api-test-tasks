using APITele.Models;
using APITele.Dto;

namespace APITele.Repositories;

public interface ICitizenRepository
{
    Task<ResponseDto> GetAll(ModelFilter? filter, int page, int pageLimit);
    Task<Citizen?> GetById(string id);
    Task<Citizen> Add(Citizen citizen);
    Task<bool> Delete(string id);
}
