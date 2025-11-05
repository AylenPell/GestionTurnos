using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders
{
    public static class StudySeeder
    {
        private static readonly string[] DefaultStudies = new[]
        {
            "Mamografía/Senografía Digital",
            "Mamografía Con Contraste",
            "Ecografía Mamaria",
            "Ecografía Mamaria Con Doppler Color",
            "Ecografía Transvaginal",
            "Ecografía Tocoginecológica",
            "Ecografía Abdomen",
            "Ecografía Tiroides",
            "Ecografía Reno-Vesical",
            "Otras Ecografías",
            "Densitometría Ósea",
            "PAP",
            "Colposcopía",
            "Video Colposcopía",
            "Estudio Urodinámico/Flujometría",
            "Otro"
        };

        /// <summary>
        /// Inserta solo los estudios faltantes.
        /// No modifica ni reactiva los que ya existen.
        /// </summary>
        public static async Task SeedAsync(IServiceProvider services, bool migrateDb = true)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GestorTurnosContext>();

            if (migrateDb)
                await context.Database.MigrateAsync();

            // Traemos los nombres existentes (activos e inactivos)
            var existingNames = await context.Studies
                .Select(s => s.Name.Trim().ToLower())
                .ToListAsync();

            foreach (var name in DefaultStudies)
            {
                var normalizedName = name.Trim();
                var key = normalizedName.ToLower();

                // Si ya existe (activo o inactivo), no hacemos nada
                if (existingNames.Contains(key))
                    continue;

                // No existe: creamos un nuevo estudio activo
                context.Studies.Add(new Study
                {
                    Name = normalizedName,
                    IsActive = true
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
