using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options) { }

  public DbSet<User> Users => Set<User>();
  public DbSet<Category> Categories => Set<Category>();
  public DbSet<Transaction> Transactions => Set<Transaction>();
}