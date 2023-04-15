using APITele.Models;
using Microsoft.EntityFrameworkCore;

namespace APITele.Context;

public class CitizenContext : DbContext
{
    public DbSet<Citizen> Citizens => Set<Citizen>();
    public CitizenContext(DbContextOptions options) : base(options) { }
}
