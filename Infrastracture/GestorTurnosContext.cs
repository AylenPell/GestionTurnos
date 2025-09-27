using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class GestorTurnosContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; } 
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Study> Studies { get; set; }

        public GestorTurnosContext(DbContextOptions<GestorTurnosContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Convierte el enum Roles a string en la BD para ponerles el nombre
            modelBuilder.Entity<Role>()
                .Property(r => r.RoleName)
                .HasConversion<string>();
        }
    }
}
