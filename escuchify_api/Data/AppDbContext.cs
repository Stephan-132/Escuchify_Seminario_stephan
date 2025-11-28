namespace escuchify_api.Data;


using Microsoft.EntityFrameworkCore;
using escuchify_api.Core.Entities;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Canciones> Canciones { get; set; }
    public DbSet<Discos> Discos { get; set; }
    public DbSet<Artista> Artistas { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Discos>()
        .HasMany(d => d.Canciones)
        .WithOne(c => c.discos)
        .HasForeignKey(c => c.DiscosID)
        .OnDelete(DeleteBehavior.Cascade);
    }
}