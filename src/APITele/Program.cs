using APITele.Context;
using Microsoft.EntityFrameworkCore;
using APITele.Repositories;
using APITele.BackgroundService;
using APITele.HttpClients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<CitizenBackgroundService>();

builder.Services.AddHttpClient<IExternalCitizensClient, ExternalCitizensClient>();

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
