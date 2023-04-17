using APITele.Dto;
using APITele.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APITeleTests;

public class CitizenControllerTest
{
    [Test]
    public async Task CitizenController_GetById_Returns_Ok()
    {
        Citizen expectedResult = new Citizen("string1", "John", 22, Genders.Male);
        var repositoryMock = new Mock<ICitizenRepository>();

        repositoryMock.Setup(x => x.GetByIdAsync("string1")).ReturnsAsync(expectedResult);
        var controller = new CitizenController(repositoryMock.Object);
        var result = await controller.GetById("string1");

        Assert.That(((Citizen)((OkObjectResult)result.Result).Value).Id, Is.EqualTo(expectedResult.Id));
    }
    [Test]
    public async Task CitizenController_GetById_ThrowsNotFound()
    {
        var repositoryMock = new Mock<ICitizenRepository>();

        repositoryMock.Setup(x => x.GetByIdAsync("string1")).ReturnsAsync(()=>null);
        var controller = new CitizenController(repositoryMock.Object);
        var result = await controller.GetById("string1");

        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }


    [Test]
    public async Task CitizenController_GetAll_Returns_Ok()
    {
        Citizen citizen = new Citizen("string1", "John", 22, Genders.Male);
        List<Citizen> citizens = new List<Citizen>();
        citizens.Add(citizen);
        ResponseDto responseDto = new ResponseDto() { Citizens = citizens, Limit = 1, Offset = 0, Total = 1};
        ModelFilter filter = new ModelFilter() { MaxYear = 100, MinYear = 0, Genders = Genders.Male};
        var repositoryMock = new Mock<ICitizenRepository>();

        repositoryMock.Setup(x => x.GetAllAsync(filter,0,1)).ReturnsAsync(responseDto);
        var controller = new CitizenController(repositoryMock.Object);
        var result = await controller.GetAll(filter,0,1);

        Assert.That(((ResponseDto)((OkObjectResult)result.Result).Value).Citizens.Count(), Is.EqualTo(responseDto.Citizens.Count));
        Assert.That(((ResponseDto)((OkObjectResult)result.Result).Value).Total, Is.EqualTo(responseDto.Total));
        Assert.That(((ResponseDto)((OkObjectResult)result.Result).Value).Limit, Is.EqualTo(responseDto.Limit));
        Assert.That(((ResponseDto)((OkObjectResult)result.Result).Value).Offset, Is.EqualTo(responseDto.Offset));
    }
    [Test]
    public async Task CitizenController_GetAll_Returns_Ok_With_Empty_LIst()
    {
        List<Citizen> citizens = new();
        ResponseDto responseDto = new() { Citizens = citizens, Limit = 1, Offset = 0, Total = 1 };
        ModelFilter filter = new ModelFilter() { MaxYear = 100, MinYear = 99, Genders = Genders.Male };

        var repositoryMock = new Mock<ICitizenRepository>();

        repositoryMock.Setup(x => x.GetAllAsync(filter, 0, 1)).ReturnsAsync(responseDto);
        var controller = new CitizenController(repositoryMock.Object);
        var result = await controller.GetAll(filter, 0, 1);

        Assert.That(((ResponseDto)((OkObjectResult)result.Result).Value).Citizens.Count(),Is.EqualTo(responseDto.Citizens.Count));
        Assert.That(((ResponseDto)((OkObjectResult)result.Result).Value).Total, Is.EqualTo(responseDto.Total));
        Assert.That(((ResponseDto)((OkObjectResult)result.Result).Value).Limit, Is.EqualTo(responseDto.Limit));
        Assert.That(((ResponseDto)((OkObjectResult)result.Result).Value).Offset, Is.EqualTo(responseDto.Offset));
    }
}