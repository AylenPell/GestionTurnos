using Application.Abstraction;
using Application.Abstraction.Notifications;
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
using Polly;
using System.Net.Http.Headers;
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

builder.Services.AddDbContext<GestorTurnosContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    Options.AddPolicy("ProfessionalPolicy", policy => policy.RequireRole("Professional", "SuperAdmin"));
    Options.AddPolicy("UserPolicy", policy => policy.RequireRole("User", "SuperAdmin"));
    Options.AddPolicy("UserAndAdminPolicy", policy => policy.RequireRole("User", "Admin", "SuperAdmin"));
    Options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin", "SuperAdmin"));
    Options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("SuperAdmin"));
});

#region Injections
// user
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
//admin
builder.Services.AddScoped<IAdminService, AdminService>();
// superadmin
builder.Services.AddScoped<ISuperAdminService, SuperAdminService>();
// auth
builder.Services.AddScoped<IAuthService, AuthService>();
// professional schedule
builder.Services.AddScoped<IProfessionalScheduleService, ProfessionalScheduleService>();
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
// twilio
builder.Services.AddScoped<ITwilioWhatsAppService, TwilioWhatsAppService>();
builder.Services.AddScoped<IAppointmentNotifier, TwilioAppointmentNotifier>();

#endregion

#region Pollys
PollySettings twillioPollys = new()
{
    RetryCount = 2,
    RetryAttemptInSec = 3,
    BreakInSec = 120,
    HandleEventsAllowed = 5
};
#endregion

#region TwilioClientFactory
// 🔹 Cargar configuración de Twilio
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));

// 🔹 Registrar HttpClient para Twilio + Polly
builder.Services
    .AddHttpClient("TwilioClient", (sp, client) =>
    {
        var settings = sp.GetRequiredService<IOptions<TwilioSettings>>().Value;

        // URL base del API Twilio
        client.BaseAddress = new Uri($"https://api.twilio.com/2010-04-01/Accounts/{settings.AccountSid}/");

        // Header de autenticación básica (AccountSid + AuthToken)
        var byteArray = System.Text.Encoding.ASCII.GetBytes($"{settings.AccountSid}:{settings.AuthToken}");
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
    })
    // 🔹 Agregar políticas Polly (Retry + CircuitBreaker)
    .AddPolicyHandler((sp, _) => ResiliencePolicies.GetWaitAndRetryPolicy(twillioPollys))
    .AddPolicyHandler((sp, _) => ResiliencePolicies.GetCircuitBreakerPolicy(twillioPollys));
#endregion


var app = builder.Build();

#region Seeders
await RoleSeeder.SeedAsync(
    app.Services,
    migrateDb: app.Environment.IsDevelopment()
);

await PatientSeeder.SeedAsync(
    app.Services,
    migrateDb: app.Environment.IsDevelopment()
);

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

await ProfessionalSpecialtySeeder.SeedAsync(
    app.Services,
    migrateDb: app.Environment.IsDevelopment()
);

await AppointmentSeeder.SeedAsync(
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

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
