using APITele.Models;

namespace APITele.Dto;

public class ResponseDto
{
    public List<Citizen> Citizens { get; set; } = new();
    public int Pages { get; set; }
    public int CurrentPage { get; set; }
}
