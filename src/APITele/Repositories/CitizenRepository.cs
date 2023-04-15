﻿using APITele.Dto;
using APITele.Models;
using APITele.Context;
using Microsoft.EntityFrameworkCore;

namespace APITele.Repositories
{
    public class CitizenRepository : ICitizenRepository
    {
        private readonly CitizenContext _db;
        public CitizenRepository(CitizenContext db)
        {
            _db = db;
        }

        public Citizen? GetById(string id)
        {
            var result = _db.Citizens.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (result == null)
                return null;
            return result;
        }

        public ResponseDto GetAll(ModelFilter? filter, int page, int pageLimit)
        {
            var citizens = _db.Citizens.AsQueryable();

            //filter
            if (filter.MinYear != null)
                citizens = citizens.Where(x=>x.Age >= filter.MinYear.Value);
            if (filter.MaxYear != null)
                citizens = citizens.Where(x=> x.Age <= filter.MaxYear.Value);

            if (filter.Genders != null)
                citizens = citizens.Where(x=> x.Sex == filter.Genders);


            //pagination
            double total = citizens.Count();

            var lastPage = Math.Ceiling(total / pageLimit);
            var offset = (page - 1) * pageLimit;
            var result = citizens.Skip(offset).Take(pageLimit).AsNoTracking().ToList();
            return new() { Citizens = result, CurrentPage = page, Pages = (int)lastPage };
        }

        public Citizen Add(Citizen citizen)
        {
            _db.Add(citizen);
            _db.SaveChanges();
            return citizen;
        }

        public bool Delete(string id)
        {
            var citizen = _db.Citizens.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (citizen == null) 
                return false;
            _db.Remove(citizen);
            _db.SaveChanges();
            return true;
        }
    }
}
