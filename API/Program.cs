using Application.Abstraction;
using Application.Services;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GestorTurnosContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
#region Injections
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
#endregion

var app = builder.Build();

// Aplicar migraciones y seed de Roles
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GestorTurnosContext>();

    // Aplica migraciones pendientes
    context.Database.Migrate();

    // Seed Roles si no existen
    if (!context.Roles.Any())
    {
        context.Roles.AddRange(
            new Role { Id = 1, RoleName = Roles.SuperAdmin },
            new Role { Id = 2, RoleName = Roles.Admin },
            new Role { Id = 3, RoleName = Roles.User },
            new Role { Id = 4, RoleName = Roles.Professional }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
