using APITele.Dto;
using APITele.Models;
using APITele.Context;
using Microsoft.EntityFrameworkCore;

namespace APITele.Repositories;

public class CitizenRepository : ICitizenRepository
{
    private readonly CitizenContext _db;
    public CitizenRepository(CitizenContext db)
    {
        _db = db;
    }

    public async Task<Citizen?> GetByIdAsync(string id)
    {
        var result = await _db.Citizens.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (result == null)
            return null;
        return result;
    }

    public async Task<ResponseDto> GetAllAsync(ModelFilter filter, int offset, int pageLimit)
    {
        var citizens = _db.Citizens.AsQueryable();

        //filter
        if (filter.MinYear != null)
            citizens = citizens.Where(x=>x.Age >= filter.MinYear.Value);
        if (filter.MaxYear != null)
            citizens = citizens.Where(x=> x.Age <= filter.MaxYear.Value);
        if (filter.Genders != null)
            citizens = citizens.Where(x=> x.Sex == filter.Genders);
        if (filter == null)
            citizens = _db.Citizens.AsQueryable();
        //pagination
        var total = await citizens.CountAsync();
        var result = await citizens.Skip(offset).Take(pageLimit).AsNoTracking().ToListAsync();
        return new()
        {
            Citizens = result,
            Total = total,
            Limit = pageLimit,
            Offset = offset
        };
    }

    public async Task<Citizen> AddAsync(Citizen citizen)
    {
        await _db.AddAsync(citizen);
        await _db.SaveChangesAsync();
        return citizen;
    }
}
