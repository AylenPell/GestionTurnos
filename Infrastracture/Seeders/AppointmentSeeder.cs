using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders
{
    public static class AppointmentSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, bool migrateDb = true)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GestorTurnosContext>();

            if (migrateDb)
                await context.Database.MigrateAsync();

            // Debe haber pacientes, profesionales y/o estudios (según el tipo)
            var users = await context.Users.Where(u => u.RoleId == 3 && u.IsActive).ToListAsync();
            if (users.Count == 0) return;

            var professionals = await context.Professionals.Where(p => p.IsActive).ToListAsync();
            var studies = await context.Studies.Where(s => s.IsActive).ToListAsync();

            // Si ya hay turnos, no duplicamos (ajustá a gusto)
            if (await context.Appointments.AnyAsync())
                return;

            var random = new Random();

            // Armar tipos posibles según disponibilidad real
            var possibleTypes = new List<string>();
            if (professionals.Count > 0) possibleTypes.Add("SOLO CONSULTA");
            if (studies.Count > 0) possibleTypes.Add("SOLO ESTUDIO");
            if (professionals.Count > 0 && studies.Count > 0) possibleTypes.Add("CIRCUITO");

            if (possibleTypes.Count == 0) return; // No hay datos para crear citas

            var isPatientOptions = new[] { "SI", "NO", "NR" };

            var appointments = new List<Appointment>();

            foreach (var user in users)
            {
                // 2 turnos por paciente
                for (int i = 0; i < 2; i++)
                {
                    // Elegir tipo válido según lo disponible
                    var apptType = possibleTypes[random.Next(possibleTypes.Count)];

                    // Fecha 1..30 días desde hoy
                    var date = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(1, 30)));
                    // Horario cada 30'
                    var time = new TimeOnly(random.Next(8, 18), random.Next(0, 2) == 0 ? 0 : 30);

                    int? professionalId = null;
                    int? studyId = null;

                    if (apptType == "SOLO CONSULTA" || apptType == "CIRCUITO")
                    {
                        var prof = professionals[random.Next(professionals.Count)];
                        professionalId = prof.Id;
                    }

                    if (apptType == "SOLO ESTUDIO" || apptType == "CIRCUITO")
                    {
                        var study = studies[random.Next(studies.Count)];
                        studyId = study.Id;
                    }

                    appointments.Add(new Appointment
                    {
                        IsPatient = isPatientOptions[random.Next(isPatientOptions.Length)],
                        AppointmentType = apptType,
                        AppointmentDate = date,
                        AppointmentTime = time,
                        // AppointmentStatus: no se setea → queda Pendiente por default
                        ProfessionalId = professionalId,
                        StudyId = studyId,
                        UserId = user.Id,
                        IsActive = true
                    });
                }
            }

            if (appointments.Count > 0)
            {
                await context.Appointments.AddRangeAsync(appointments);
                await context.SaveChangesAsync();
            }
        }
    }
}
