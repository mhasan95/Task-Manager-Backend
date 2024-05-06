using Microsoft.EntityFrameworkCore;

namespace Task_Manager_Backend.Models;

public class AppDbContext : DbContext
{
    protected readonly IConfiguration Configuration;
    //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    //{
    //}
    public AppDbContext(IConfiguration configuration) {
        Configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(Configuration.GetConnectionString("DefaultConnectionString"));
    }
    public DbSet<Task> Tasks => Set<Task>();
}
