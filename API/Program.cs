using Application.Abstraction;
using Application.ExternalServices;
using Application.Services;
using Infrastructure;
using Infrastructure.ExternalServices;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction=>
    {
        setupAction.AddSecurityDefinition("ApiBearerAuth", new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            Description = "Input your Bearer token to access this API"
        });
        setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiBearerAuth"
                    }
                },
                new List<string>()
            }
        });
    });

builder.Services.AddDbContext<GestorTurnosContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });
builder.Services.AddAuthorization(Options =>
{
    Options.AddPolicy("ProfessionalPolicy", policy => policy.RequireRole("Professional"));
    Options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
    Options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    Options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("SuperAdmin"));
    Options.AddPolicy("SuperAdminOrAdminPolicy", policy => policy.RequireRole("Admin", "SuperAdmin"));
});

#region Injections
// user
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
//admin
builder.Services.AddScoped<IAdminService, AdminService>();
// superadmin
builder.Services.AddScoped<ISuperAdminService, SuperAdminService>();
// specialty
builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<ISpecialtyService, SpecialtyService>();
// professional
builder.Services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
builder.Services.AddScoped<IProfessionalService, ProfessionalService>();
// study
builder.Services.AddScoped<IStudyRepository, StudyRepository>();
builder.Services.AddScoped<IStudyService, StudyService>();
// appointment
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
// auth
builder.Services.AddScoped<IAuthService, AuthService>();
#endregion

var app = builder.Build();

#region Seeders
await SpecialtySeeder.SeedAsync(
    app.Services,
    migrateDb: app.Environment.IsDevelopment()
);

await StudySeeder.SeedAsync(
    app.Services,
    migrateDb: app.Environment.IsDevelopment()
);

await ProfessionalSeeder.SeedAsync(
    app.Services,
    migrateDb: app.Environment.IsDevelopment()
);
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
