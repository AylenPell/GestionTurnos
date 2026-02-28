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
        /// Inserta solo los estudios faltantes (modo dev).
        /// No modifica ni reactiva los que ya existen.
        /// Solo asegura que haya 16 registros.
        /// </summary>
        public static async Task SeedAsync(IServiceProvider services, bool migrateDb = true)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GestorTurnosContext>();

            if (migrateDb)
                await context.Database.MigrateAsync();

            // Traemos los IDs existentes
            var existingIds = await context.Studies
                .Select(s => s.Id)
                .ToListAsync();

            for (int i = 1; i <= DefaultStudies.Length; i++)
            {
                // Si ya existe ese ID, no hacemos nada
                if (existingIds.Contains(i))
                    continue;

                context.Studies.Add(new Study
                {
                    Name = DefaultStudies[i - 1].Trim(),
                    IsActive = true
                });
            }

            await context.SaveChangesAsync();
        }
    }
}