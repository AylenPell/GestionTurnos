using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders
{
    public static class ProfessionalSpecialtySeeder
    {
        public static async Task SeedAsync(IServiceProvider services, bool migrateDb = false)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GestorTurnosContext>();

            if (migrateDb)
                await context.Database.MigrateAsync();

            // Si no hay profesionales o especialidades, no hacemos nada
            if (!context.Professionals.Any() || !context.Specialties.Any())
                return;

            // Obtenemos IDs actuales
            var professionals = await context.Professionals
                .OrderBy(p => p.Id)
                .ToListAsync();

            var specialties = await context.Specialties
                .OrderBy(s => s.Id)
                .ToListAsync();

            // Relaciones existentes para evitar duplicados (ignora el query filter de IsActive)
            var existingRelations = await context.ProfessionalSpecialties
                .IgnoreQueryFilters()
                .Select(ps => new { ps.ProfessionalId, ps.SpecialtyId })
                .ToListAsync();

            var relationsToAdd = new List<ProfessionalSpecialty>();

            // Ejemplo de asignación automática:
            // los primeros N profesionales se reparten cíclicamente entre las especialidades
            for (int i = 0; i < professionals.Count; i++)
            {
                var prof = professionals[i];
                var specialty = specialties[i % specialties.Count]; // distribuye en ciclo

                bool alreadyExists = existingRelations.Any(r =>
                    r.ProfessionalId == prof.Id && r.SpecialtyId == specialty.Id);

                if (!alreadyExists)
                {
                    relationsToAdd.Add(new ProfessionalSpecialty
                    {
                        ProfessionalId = prof.Id,
                        SpecialtyId = specialty.Id,
                        AssignedDate = DateOnly.FromDateTime(DateTime.Now),
                        IsActive = true
                    });
                }
            }

            if (relationsToAdd.Any())
            {
                context.ProfessionalSpecialties.AddRange(relationsToAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}
