using APITele.Context;
using Microsoft.EntityFrameworkCore;
using APITele.Repositories;
using APITele.BackgroundService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<CitizenBackgroundService>();

//var services = new ServiceCollection();
//var serviceProvider = services.BuildServiceProvider();
//var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
//var citizenScopedFactory = serviceProvider.GetService<CitizenRepository>();

//services.AddHttpClient("apiClient", client =>
//{
//    client.BaseAddress = new Uri("https://testlodtask20172.azurewebsites.net/task");
//});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICitizenRepository, CitizenRepository>();
builder.Services.AddDbContext<CitizenContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
