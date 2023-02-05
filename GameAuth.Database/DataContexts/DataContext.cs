using Microsoft.EntityFrameworkCore;
using Entities = GameAuth.Database.Models.Entities;

namespace GameAuth.Database.DataContexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        //if (Database.CanConnect()) Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public required DbSet<Entities.Account> Account { get; set; }
    public required DbSet<Entities.Address> Address { get; set; }
    public required DbSet<Entities.Email> Email { get; set; }
    public required DbSet<Entities.VerificationEmail> VerificationEmail { get; set; }
}