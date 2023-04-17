using APITele.Models;
using APITele.Dto;

namespace APITele.Repositories;

public interface ICitizenRepository
{
    Task<ResponseDto> GetAllAsync(ModelFilter filter, int offset, int pageLimit);
    Task<Citizen?> GetByIdAsync(string id);
    Task<Citizen> AddAsync(Citizen citizen);
}
