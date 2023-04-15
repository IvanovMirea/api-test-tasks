using APITele.Models;
using APITele.Dto;

namespace APITele.Repositories;

public interface ICitizenRepository
{
    ResponseDto GetAll(ModelFilter? filter, int page, int pageLimit);
    Citizen? GetById(string id);
    Citizen Add(Citizen citizen);
    bool Delete(string id);
}
