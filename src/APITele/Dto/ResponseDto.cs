using APITele.Models;

namespace APITele.Dto;

public class ResponseDto
{
    public List<Citizen> Citizens { get; set; } = new();
    public int Total { get; set; }
    public int Limit { get; set; } = 10;
    public int Offset { get; set; }
}
