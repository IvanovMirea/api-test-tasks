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
    public async Task<ActionResult<Citizen>> GetById(string  id)
    {
        var result = await _citizenRepo.GetByIdAsync(id);
        if (result == null)
            return NotFound("No resident with this id !");
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto>> GetAll([FromQuery]ModelFilter filter, int offset, int limit = 10)
    {
        var result = await _citizenRepo.GetAllAsync(filter, offset, limit);
        return Ok(result);
    }
}
