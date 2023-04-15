using APITele.Models;
using APITele.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APITele.Dto;

namespace APITele.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitizenController : ControllerBase
{
    private readonly ICitizenRepository _citizenRepo;
    public CitizenController(ICitizenRepository citizenRepo)
    {
        _citizenRepo = citizenRepo;
    }

    [HttpGet("{id}")]
    public ActionResult<Citizen> GetById(string  id)
    {
        var result = _citizenRepo.GetById(id);
        if (result == null)
            return NotFound("No resident with this id !");
        return Ok(result);
    }

    [HttpGet]
    public ActionResult<ResponseDto> GetAll([FromQuery]ModelFilter filter, int page, int pageLimit)
    {
        var result = _citizenRepo.GetAll(filter, page, pageLimit);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult<Citizen> Add(Citizen citizen)
    {
        var newCitizen = new Citizen(citizen.Id,citizen.Name, citizen.Age, citizen.Sex);
        return Ok(_citizenRepo.Add(newCitizen));
    }

    [HttpDelete("{id}")]
    public ActionResult<Citizen> Delete(string id)
    {
        var deleted = _citizenRepo.Delete(id);
        return Ok(deleted);
    }
}
