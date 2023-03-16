using Microsoft.EntityFrameworkCore;
using Cotizaciones.Models;


namespace Cotizaciones.Data{


public class DataContext : DbContext
{
    private readonly IConfiguration _config;

        public DataContext(IConfiguration config)
        {
            _config = config;
        }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<CotizacionOferta> CotizacionesOferta { get; set; }
    public virtual DbSet<CotizacionTotal> CotizacionesTotal { get; set; }
    public virtual DbSet<Auth> Auth { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                        optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
            modelBuilder.HasDefaultSchema("Cotizaciones");

            modelBuilder.Entity<User>()
                .ToTable("Users", "Cotizaciones")
                .HasKey(u => u.UserId);

            modelBuilder.Entity<CotizacionOferta>()
                .HasKey(u => u.TransactionId);

            modelBuilder.Entity<CotizacionTotal>()
                .HasKey(u => u.TransactionId);

            modelBuilder.Entity<Auth>()
                .HasKey(u => u.Email);
    }
}
}