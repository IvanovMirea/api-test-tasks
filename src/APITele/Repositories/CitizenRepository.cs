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

    public async Task<Citizen?> GetById(string id)
    {
        var result = await _db.Citizens.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (result == null)
            return null;
        return result;
    }

    public async Task<ResponseDto> GetAll(ModelFilter? filter, int offset, int pageLimit)
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
        double total = await citizens.CountAsync();
        var result = await citizens.Skip(offset).Take(pageLimit).AsNoTracking().ToListAsync();
        return new() { Citizens = result, Total = (int)total, Limit = pageLimit };
    }

    public async Task<Citizen> Add(Citizen citizen)
    {
        await _db.AddAsync(citizen);
        await _db.SaveChangesAsync();
        return citizen;
    }

    public async Task<bool> Delete(string id)
    {
        var citizen = await _db.Citizens.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (citizen == null) 
            return false;
        _db.Remove(citizen);
        await _db.SaveChangesAsync();
        return true;
    }
}
