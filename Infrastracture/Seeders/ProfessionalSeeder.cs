using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders
{
    public static class ProfessionalSeeder
    {
        private static readonly (string Name, string LastName, string License, string AttentionSchedule)[] DefaultProfessionals =
        {
            ("Juana", "Azurduy", "LIC001", "Lunes a Viernes 8:00-16:00"),
            ("Mariquita", "Sánchez de Thompson", "LIC002", "Martes y Jueves 10:00-18:00"),
            ("Manuela", "Pedraza", "LIC003", "Lunes a Miércoles 9:00-14:00"),
            ("Remedios", "del Valle", "LIC004", "Viernes 12:00-19:00"),
            ("Petrona", "Rosende", "LIC005", "Lunes a Viernes 8:00-15:00"),
            ("Cecilia", "Grierson", "LIC006", "Martes y Jueves 9:00-17:00"),
            ("Elvira", "Rawson", "LIC007", "Lunes a Viernes 7:00-14:00"),
            ("Julieta", "Lanteri", "LIC008", "Martes y Viernes 10:00-18:00"),
            ("Alicia", "Moreau de Justo", "LIC009", "Lunes a Miércoles 8:00-15:00"),
            ("Eva", "Perón", "LIC010", "Miércoles a Viernes 9:00-16:00"),
            ("Alfonsina", "Storni", "LIC011", "Lunes a Viernes 9:00-17:00"),
            ("Victoria", "Ocampo", "LIC012", "Martes a Sábado 10:00-18:00"),
            ("Ameghino", "Bosch", "LIC013", "Lunes a Viernes 8:00-14:00"),
            ("Silvina", "Ocampo", "LIC014", "Martes y Jueves 12:00-19:00"),
            ("Alejandra", "Pizarnik", "LIC015", "Lunes a Miércoles 9:00-16:00"),
            ("Carolina", "Muzzilli", "LIC016", "Lunes a Viernes 8:00-15:00"),
            ("Nadia", "Bustillo", "LIC017", "Martes y Jueves 10:00-17:00"),
            ("Dora", "Barrancos", "LIC018", "Lunes a Viernes 9:00-17:00"),
            ("Martha", "Argerich", "LIC019", "Miércoles y Viernes 8:00-16:00"),
            ("Esther", "Feldman", "LIC020", "Lunes a Sábado 9:00-13:00"),
            ("Nora", "Cortiñas", "LIC021", "Martes a Viernes 8:00-14:00"),
            ("Estela", "de Carlotto", "LIC022", "Lunes a Viernes 10:00-18:00"),
            ("Hebe", "de Bonafini", "LIC023", "Lunes a Jueves 8:00-15:00"),
            ("Sara", "Gallardo", "LIC024", "Martes y Viernes 9:00-17:00"),
            ("Ada", "María Elflein", "LIC025", "Lunes a Miércoles 8:00-14:00"),
            ("Lola", "Mora", "LIC026", "Lunes a Viernes 9:00-17:00"),
            ("María", "Elena Walsh", "LIC027", "Martes y Jueves 10:00-18:00"),
            ("Gabriela", "Mistral", "LIC028", "Miércoles a Viernes 12:00-19:00"), // chilena, pero influyente
            ("Herminia", "Brumana", "LIC029", "Lunes a Viernes 8:00-16:00"),
            ("Olga", "Cossettini", "LIC030", "Martes y Viernes 9:00-15:00"),
            ("Rosa", "Guarú", "LIC031", "Lunes a Jueves 9:00-16:00"),
            ("Azucena", "Villaflor", "LIC032", "Lunes a Viernes 8:00-14:00")
        };

        public static async Task SeedAsync(IServiceProvider services, bool migrateDb = true)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GestorTurnosContext>();

            if (migrateDb)
                await context.Database.MigrateAsync();

            var existingLicenses = await context.Professionals
                .Select(p => p.License.Trim().ToLower())
                .ToListAsync();

            foreach (var (Name, LastName, License, AttentionSchedule) in DefaultProfessionals)
            {
                var normalizedLicense = License.Trim().ToLower();
                if (existingLicenses.Contains(normalizedLicense))
                    continue;

                context.Professionals.Add(new Professional
                {
                    Name = Name,
                    LastName = LastName,
                    License = License,
                    AttentionSchedule = AttentionSchedule,
                    RoleId = 4,
                    IsActive = true
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
