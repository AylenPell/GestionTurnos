using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders
{
    public static class SpecialtySeeder
    {
        private static readonly string[] DefaultSpecialties = new[]
        {
            "Mastologia",
            "Genetica",
            "Oncologia",
            "Ginecologia",
            "Fertilidad",
            "Obstetricia",
            "Urologia",
            "Clinica",
            "Gastroenterologia",
            "Kinesiologia",
            "Nutricion",
            "Cardiologia",
            "Reumatologia",
            "Neumonologia",
            "Alergologia",
            "CirugiaPlastica",
            "Uroginecologia",
            "Urodinamia"
        };

        /// <summary>
        /// Inserta solo las especialidades faltantes.
        /// No modifica ni reactiva las que ya existen.
        /// </summary>
        public static async Task SeedAsync(IServiceProvider services, bool migrateDb = true)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GestorTurnosContext>();

            if (migrateDb)
                await context.Database.MigrateAsync();

            // Traemos los nombres existentes (activos e inactivos)
            var existingNames = await context.Specialties
                .Select(s => s.SpecialtyName.Trim().ToLower())
                .ToListAsync();

            foreach (var data in DefaultSpecialties)
            {
                var normalizedName = data.Trim();
                var key = normalizedName.ToLower();

                // Si ya existe (activo o inactivo), no hacemos nada
                if (existingNames.Contains(key))
                    continue;

                // No existe: creamos una nueva especialidad activa
                context.Specialties.Add(new Specialty
                {
                    SpecialtyName = normalizedName,
                    IsActive = true
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
