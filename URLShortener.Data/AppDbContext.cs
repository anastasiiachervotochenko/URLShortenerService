using Microsoft.EntityFrameworkCore;
using URLShortener.Data.Entity;

namespace URLShortener.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UrlLog> UrlLog { get; set; }
}