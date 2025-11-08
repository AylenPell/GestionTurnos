using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders
{
    public static class ProfessionalSeeder
    {
        private static readonly (string Name, string LastName, string License, string AttentionSchedule, string Email, string Password)[] DefaultProfessionals =
        {
            ("Juana", "Azurduy", "LIC001", "Lunes a Viernes 8:00-16:00", "juana.azurduy@clinicapp.com", "JuanaAzu123!"),
            ("Mariquita", "Sánchez de Thompson", "LIC002", "Martes y Jueves 10:00-18:00", "mariquita.sanchez@clinicapp.com", "MariSanchez456@"),
            ("Manuela", "Pedraza", "LIC003", "Lunes a Miércoles 9:00-14:00", "manuela.pedraza@clinicapp.com", "ManuPed789#"),
            ("Remedios", "del Valle", "LIC004", "Viernes 12:00-19:00", "remedios.valle@clinicapp.com", "RemeValle321!"),
            ("Petrona", "Rosende", "LIC005", "Lunes a Viernes 8:00-15:00", "petrona.rosende@clinicapp.com", "PetRo567@"),
            ("Cecilia", "Grierson", "LIC006", "Martes y Jueves 9:00-17:00", "cecilia.grierson@clinicapp.com", "CeciGr123$"),
            ("Elvira", "Rawson", "LIC007", "Lunes a Viernes 7:00-14:00", "elvira.rawson@clinicapp.com", "ElviRaw654!"),
            ("Julieta", "Lanteri", "LIC008", "Martes y Viernes 10:00-18:00", "julieta.lanteri@clinicapp.com", "JuliLan987@"),
            ("Alicia", "Moreau de Justo", "LIC009", "Lunes a Miércoles 8:00-15:00", "alicia.moreau@clinicapp.com", "AlicMore456#"),
            ("Eva", "Perón", "LIC010", "Miércoles a Viernes 9:00-16:00", "eva.peron@clinicapp.com", "EvaPeron321!"),
            ("Alfonsina", "Storni", "LIC011", "Lunes a Viernes 9:00-17:00", "alfonsina.storni@clinicapp.com", "AlfonStorn654@"),
            ("Victoria", "Ocampo", "LIC012", "Martes a Sábado 10:00-18:00", "victoria.ocampo@clinicapp.com", "VickyOca123#"),
            ("Ameghino", "Bosch", "LIC013", "Lunes a Viernes 8:00-14:00", "ameghino.bosch@clinicapp.com", "AmegBosch987!"),
            ("Silvina", "Ocampo", "LIC014", "Martes y Jueves 12:00-19:00", "silvina.ocampo@clinicapp.com", "SilviOca456$"),
            ("Alejandra", "Pizarnik", "LIC015", "Lunes a Miércoles 9:00-16:00", "alejandra.pizarnik@clinicapp.com", "AlePiz321@"),
            ("Carolina", "Muzzilli", "LIC016", "Lunes a Viernes 8:00-15:00", "carolina.muzzilli@clinicapp.com", "CaroMuzz654!"),
            ("Nadia", "Bustillo", "LIC017", "Martes y Jueves 10:00-17:00", "nadia.bustillo@clinicapp.com", "NadiaBus123#"),
            ("Dora", "Barrancos", "LIC018", "Lunes a Viernes 9:00-17:00", "dora.barrancos@clinicapp.com", "DoraBar789!"),
            ("Martha", "Argerich", "LIC019", "Miércoles y Viernes 8:00-16:00", "martha.argerich@clinicapp.com", "MarArge456@"),
            ("Esther", "Feldman", "LIC020", "Lunes a Sábado 9:00-13:00", "esther.feldman@clinicapp.com", "EstFeld987#"),
            ("Nora", "Cortiñas", "LIC021", "Martes a Viernes 8:00-14:00", "nora.cortinas@clinicapp.com", "NoraCor654!"),
            ("Estela", "de Carlotto", "LIC022", "Lunes a Viernes 10:00-18:00", "estela.carlotto@clinicapp.com", "EstCarl321@"),
            ("Hebe", "de Bonafini", "LIC023", "Lunes a Jueves 8:00-15:00", "hebe.bonafini@clinicapp.com", "HebeBona789$"),
            ("Sara", "Gallardo", "LIC024", "Martes y Viernes 9:00-17:00", "sara.gallardo@clinicapp.com", "SaraGal456!"),
            ("Ada", "María Elflein", "LIC025", "Lunes a Miércoles 8:00-14:00", "ada.elflein@clinicapp.com", "AdaElf123#"),
            ("Lola", "Mora", "LIC026", "Lunes a Viernes 9:00-17:00", "lola.mora@clinicapp.com", "LolaMora987@"),
            ("María", "Elena Walsh", "LIC027", "Martes y Jueves 10:00-18:00", "maria.walsh@clinicapp.com", "MariaWal654!"),
            ("Gabriela", "Mistral", "LIC028", "Miércoles a Viernes 12:00-19:00", "gabriela.mistral@clinicapp.com", "GabyMistr123@"),
            ("Herminia", "Brumana", "LIC029", "Lunes a Viernes 8:00-16:00", "herminia.brumana@clinicapp.com", "HermBru987!"),
            ("Olga", "Cossettini", "LIC030", "Martes y Viernes 9:00-15:00", "olga.cossettini@clinicapp.com", "OlgaCos456$"),
            ("Rosa", "Guarú", "LIC031", "Lunes a Jueves 9:00-16:00", "rosa.guarru@clinicapp.com", "RosaGua321!"),
            ("Azucena", "Villaflor", "LIC032", "Lunes a Viernes 8:00-14:00", "azucena.villaflor@clinicapp.com", "AzuVilla654@")
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

            foreach (var (Name, LastName, License, AttentionSchedule, Email, Password) in DefaultProfessionals)
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
                    Email = Email,
                    Password = Password,
                    RoleId = 4,
                    IsActive = true
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
