using APITele.Context;
using APITele.HttpClients;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APITele.Models;
using APITele.BackgroundService;
using System.Net.Http.Json;

namespace APITeleTests;
[TestFixture]
internal class CitizenBackgroundServiceTests
{
    [Test]
    public async Task CitizenBackgroundService_StartAsync_Returns_AllId_Except_One_That_Allready_Exist()
    {
        var citizen1 = new Citizen ( "1", "John",10,Genders.Male );
        var citizen2 = new Citizen ("2", "Jane", 40, Genders.Female);
        var httpClientFactoryMock = new Mock<IExternalCitizensClient>();

        httpClientFactoryMock.Setup(m => m.GetCitizensAsync())
            .ReturnsAsync(new[] { citizen1, citizen2 });

        httpClientFactoryMock.Setup(m => m.GetCitizenByIdAsync(citizen1.Id))
            .ReturnsAsync(citizen1);

        httpClientFactoryMock.Setup(m => m.GetCitizenByIdAsync(citizen2.Id))
            .ReturnsAsync(citizen2);

        var citizenRepositoryMock = new Mock<ICitizenRepository>();

        citizenRepositoryMock.Setup(m => m.GetByIdAsync(citizen1.Id))
            .ReturnsAsync((Citizen)null);
        citizenRepositoryMock.Setup(m => m.GetByIdAsync(citizen2.Id))
            .ReturnsAsync(citizen2);

        var serviceScopeMock = new Mock<IServiceScope>();
        var serviceProvider = new ServiceCollection()
            .AddScoped(_ => citizenRepositoryMock.Object)
            .BuildServiceProvider();
        serviceScopeMock.Setup(m => m.ServiceProvider).Returns(serviceProvider);
        var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        serviceScopeFactoryMock.Setup(m => m.CreateScope()).Returns(serviceScopeMock.Object);
        var sut = new CitizenBackgroundService(serviceScopeFactoryMock.Object, httpClientFactoryMock.Object);

        // Act
        await sut.StartAsync(CancellationToken.None);

        // Assert
        citizenRepositoryMock.Verify(m => m.GetByIdAsync(citizen1.Id), Times.Once);
        citizenRepositoryMock.Verify(m => m.GetByIdAsync(citizen2.Id), Times.Once);
        citizenRepositoryMock.Verify(m => m.AddAsync(citizen1), Times.Once);
        citizenRepositoryMock.Verify(m => m.AddAsync(citizen2), Times.Never);

    }
}
