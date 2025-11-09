using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders
{
    public static class PatientSeeder
    {
        private static readonly (string Name, string LastName, string Dni, string Email, string Password, string City, string Address, string Phone, string HealthInsurance, string HealthInsurancePlan)[] DefaultUsers =
        {
            ("Camila", "Moretti", "40000001", "camila.moretti@paciente.com", "Camila123!", "Buenos Aires", "Av. Santa Fe 1234", "1130000001", "OSDE", "210"),
            ("Rocío", "Fernandez", "40000002", "rocio.fernandez@paciente.com", "Rocio456@", "La Plata", "Calle 12 345", "2214000002", "Swiss Medical", "SM100"),
            ("Luciana", "Iglesias", "40000003", "luciana.iglesias@paciente.com", "Luciana789#", "Rosario", "San Martín 800", "3415000003", "Galeno", "320"),
            ("Paula", "Benítez", "40000004", "paula.benitez@paciente.com", "Paula321$", "Córdoba", "Belgrano 250", "3516000004", "Medicus", "230"),
            ("Juliana", "Rivas", "40000005", "juliana.rivas@paciente.com", "Juliana654@", "Mendoza", "9 de Julio 456", "2617000005", "Ospe", "Plan Oro")
        };

        public static async Task SeedAsync(IServiceProvider services, bool migrateDb = true)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GestorTurnosContext>();

            if (migrateDb)
                await context.Database.MigrateAsync();

            // Traer DNIs existentes para no duplicar
            var existingDnis = await context.Users
                .Select(u => u.DNI.Trim().ToLower())
                .ToListAsync();

            foreach (var (Name, LastName, Dni, Email, Password, City, Address, Phone, HealthInsurance, HealthInsurancePlan) in DefaultUsers)
            {
                var normalizedDni = Dni.Trim().ToLower();
                if (existingDnis.Contains(normalizedDni))
                    continue;

                context.Users.Add(new User
                {
                    Name = Name,
                    LastName = LastName,
                    DNI = Dni,
                    Email = Email,
                    Password = Password,
                    City = City,
                    Address = Address,
                    Phone = Phone,
                    HealthInsurance = HealthInsurance,
                    HealthInsurancePlan = HealthInsurancePlan,
                    RoleId = 3, // Paciente
                    IsActive = true
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
