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
        public DbSet<ProfessionalSpecialty> ProfessionalSpecialties { get; set; }

        public GestorTurnosContext(DbContextOptions<GestorTurnosContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>()
                .Property(r => r.RoleName)
                .HasConversion<string>();

            modelBuilder.Entity<ProfessionalSpecialty>(profSpEntity =>
            {
                profSpEntity.ToTable("ProfessionalSpecialties");

                // Clave compuesta
                profSpEntity.HasKey(profSpRelation => new { profSpRelation.ProfessionalId, profSpRelation.SpecialtyId });

                // Relaciones (sin cascada física)
                profSpEntity.HasOne(profSpRelation => profSpRelation.Professional)
                    .WithMany(professional => professional.ProfessionalSpecialties)
                    .HasForeignKey(profSpRelation => profSpRelation.ProfessionalId)
                    .OnDelete(DeleteBehavior.Restrict);

                profSpEntity.HasOne(profSpRelation => profSpRelation.Specialty)
                    .WithMany(specialty => specialty.ProfessionalSpecialties)
                    .HasForeignKey(profSpRelation => profSpRelation.SpecialtyId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Soft delete -> solo filas activas
                profSpEntity.HasQueryFilter(profSpRelation => profSpRelation.IsActive);

                // Índices útiles
                profSpEntity.HasIndex(profSpRelation => profSpRelation.ProfessionalId);
                profSpEntity.HasIndex(profSpRelation => profSpRelation.SpecialtyId);

                // Conversión DateOnly <-> DateTime (para compatibilidad con SQLite o SQL)
                profSpEntity.Property(profSpRelation => profSpRelation.AssignedDate)
                    .HasConversion(
                        date => date.ToDateTime(TimeOnly.MinValue),
                        dateTime => DateOnly.FromDateTime(dateTime));

                profSpEntity.Property(profSpRelation => profSpRelation.LastUpdate)
                    .HasConversion(
                        date => date.HasValue ? date.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                        dateTime => dateTime.HasValue ? DateOnly.FromDateTime(dateTime.Value) : (DateOnly?)null);
            });

        }
    }
}