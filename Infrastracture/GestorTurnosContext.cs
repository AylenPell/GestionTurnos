using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class GestorTurnosContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Study> Studies { get; set; }

        public GestorTurnosContext(DbContextOptions<GestorTurnosContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>()
                .Property(r => r.RoleName)
                .HasConversion<string>();

            modelBuilder.Entity<Specialty>()
                .Property(s => s.Name)
                .HasConversion<string>();

            modelBuilder.Entity<Professional>()
                .HasOne(p => p.Specialty)
                .WithMany(s => s.Professionals)
                .HasForeignKey(p => p.SpecialtyId)
                .IsRequired();
        }
    }
}