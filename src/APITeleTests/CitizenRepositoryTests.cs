using APITele.Context;
using APITele.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITeleTests;
[TestFixture]
internal class CitizenRepositoryTests
{
    private CitizenContext _db;
    private ICitizenRepository _repository;
    [SetUp]
    public void Setup()
    {
        var option = new DbContextOptionsBuilder<CitizenContext>()
            .UseInMemoryDatabase("City").Options;
        _db = new CitizenContext(option);
        _repository = new CitizenRepository(_db);
    }
    [TearDown]
    public void Teardown()
    {
        _db.Database.EnsureCreated();
        _db.Dispose();
    }

    [Test]
    public async Task CitizenRepository_GetById_ReturnsCitizen()
    {
        var citizen = new Citizen("someid", "Name", 21, Genders.Male);
        await _db.Citizens.AddAsync(citizen);
        await _db.SaveChangesAsync();

        var result = await _repository.GetByIdAsync("someid");

        Assert.IsNotNull(result);
        Assert.That(result.Name, Is.EqualTo(citizen.Name));
        Assert.That(result.Sex, Is.EqualTo(citizen.Sex));
        Assert.That(result.Age, Is.EqualTo(citizen.Age));
    }
    [Test]
    public async Task CitizenRepository_GetById_ReturnsNull()
    {
        var citizen = new Citizen("someid", "Name", 21, Genders.Male);
        await _db.Citizens.AddAsync(citizen);
        await _db.SaveChangesAsync();

        var result = await _repository.GetByIdAsync("someid1");

        Assert.Null(result);
    }

    [Test]
    public async Task CitizensRepository_GetAll_WithFilter_ReturnsFiltered_Citizens()
    {
        var citizenAgeTwenty = new Citizen("someid1", "Name1", 20, Genders.Male);
        var citizenAgeFifty = new Citizen("someid2", "Name2", 50, Genders.Male);
        await _db.Citizens.AddAsync(citizenAgeFifty);
        await _db.Citizens.AddAsync(citizenAgeTwenty);
        await _db.SaveChangesAsync();
        var filter = new ModelFilter() { MinYear = 45 };

        var result = _repository.GetAllAsync(filter, 0, 3);

        Assert.That(result.Result.Citizens.Count, Is.EqualTo(1));
        Assert.That(result.Result.Citizens.First().Name, Is.EqualTo(citizenAgeFifty.Name));
        Assert.That(result.Result.Total, Is.EqualTo(1));
        Assert.That(result.Result.Limit, Is.EqualTo(3));
    }
    [Test]
    public async Task CitizensRepository_GetAll_WithoutFilter_Returns_AllCitizens()
    {
        var citizenAgeTwenty = new Citizen("someid1", "Name1", 20, Genders.Male);
        var citizenAgeFifty = new Citizen("someid2", "Name2", 50, Genders.Male);
        await _db.Citizens.AddAsync(citizenAgeFifty);
        await _db.Citizens.AddAsync(citizenAgeTwenty);
        await _db.SaveChangesAsync();
        var filter = new ModelFilter() { };

        var result = _repository.GetAllAsync(filter, 0, 3);

        Assert.That(result.Result.Citizens.Count, Is.EqualTo(2));
        Assert.That(result.Result.Citizens.Last().Name, Is.EqualTo(citizenAgeTwenty.Name));
        Assert.That(result.Result.Citizens.First().Name, Is.EqualTo(citizenAgeFifty.Name));
        Assert.That(result.Result.Total, Is.EqualTo(2));
        Assert.That(result.Result.Limit, Is.EqualTo(3));
    }
    [Test]
    public async Task CitizensRepository_GetAll_WithFilter_Returns_NotNullEmpty()
    {
        var citizenAgeTwenty = new Citizen("someid1", "Name1", 20, Genders.Male);
        var citizenAgeFifty = new Citizen("someid2", "Name2", 50, Genders.Male);
        await _db.Citizens.AddAsync(citizenAgeFifty);
        await _db.Citizens.AddAsync(citizenAgeTwenty);
        await _db.SaveChangesAsync();
        var filter = new ModelFilter() {MinYear = 100 };

        var result = _repository.GetAllAsync(filter, 0, 3);

        Assert.That(result.Result.Citizens.Count, Is.EqualTo(0));
        Assert.That(result.Result.Total, Is.EqualTo(0));
        Assert.That(result.Result.Limit, Is.EqualTo(3));
    }

    [Test]
    public async Task CitizensRepository_Add_Returns_Citizen()
    {
        var citizenAgeTwenty = new Citizen("someid1", "Name1", 20, Genders.Male);

        var result = _repository.AddAsync(citizenAgeTwenty);

        Assert.That(result.Result.Id, Is.EqualTo(_db.Citizens.First().Id));
        Assert.That(result.Result.Name, Is.EqualTo(_db.Citizens.First().Name));
        Assert.That(result.Result.Age, Is.EqualTo(_db.Citizens.First().Age));
        Assert.That(result.Result.Sex, Is.EqualTo(_db.Citizens.First().Sex));
    }

    [Test]
    public async Task CitizensRepository_Add_Returns_Exception()
    {
        var citizenAgeTwenty = new Citizen("someid1", "Name1", 20, Genders.Male);
        var sameIdCitizen = new Citizen("someid1", "Name2", 22, Genders.Male);

        var first = await _repository.AddAsync(citizenAgeTwenty);
        var result = _repository.AddAsync(sameIdCitizen);


        Assert.ThrowsAsync<InvalidOperationException>( () => result);
    }
}
