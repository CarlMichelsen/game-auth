using Microsoft.EntityFrameworkCore;
using GameAuth.Database.Models.Entities;

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

    public required DbSet<Account> Account { get; set; }
    public required DbSet<Address> Address { get; set; }
    public required DbSet<Email> Email { get; set; }
    public required DbSet<Ban> Ban { get; set; }
}