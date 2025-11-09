using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders
{
    public static class RoleSeeder
    {
        private static readonly Roles[] DefaultRoles =
        {
            Roles.SuperAdmin,
            Roles.Admin,
            Roles.User,
            Roles.Professional
        };

        public static async Task SeedAsync(IServiceProvider services, bool migrateDb = true)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GestorTurnosContext>();

            if (migrateDb)
                await context.Database.MigrateAsync();

            var existingRoles = await context.Roles
                .Select(r => r.RoleName)
                .ToListAsync();

            foreach (var roleName in DefaultRoles)
            {
                if (existingRoles.Contains(roleName))
                    continue;

                context.Roles.Add(new Role
                {
                    RoleName = roleName,
                    IsActive = true
                });
            }

            await context.SaveChangesAsync();
        }

    }
}
