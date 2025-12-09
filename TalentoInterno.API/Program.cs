// --- Importaciones necesarias ---

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System.Text;
using System.Text.Json.Serialization;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;
using TalentoInterno.CORE.Infrastructure.Data;
using TalentoInterno.CORE.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Settings;


var builder = WebApplication.CreateBuilder(args);


// Si es uso no comercial:
// --- 1. Registro de Servicios de .NET ---

// Añade controladores y configura JSON para ignorar ciclos (útil con Entity Framework)
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();


// --- 2. Registro del DbContext (Conexión a BD) ---
builder.Services.AddDbContext<TalentoInternooContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection")));
// Asegúrate de que "DefaultConnection" exista en tu appsettings.json

// --- 3. Registro de tus Repositorios y Servicios (Inyección de Dependencias) ---

// Servicios de Colaborador
builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
builder.Services.AddScoped<IColaboradorService, ColaboradorService>();

builder.Services.AddScoped<IColaboradorCertificacionRepository, ColaboradorCertificacionRepository>();
builder.Services.AddScoped<IColaboradorCertificacionService, ColaboradorCertificacionService>();

builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();

builder.Services.AddScoped<IVacanteRepository, VacanteRepository>();
builder.Services.AddScoped<IVacanteService, VacanteService>();

builder.Services.AddScoped<IVacanteSkillReqRepository, VacanteSkillReqRepository>();
builder.Services.AddScoped<IVacanteSkillReqService, VacanteSkillReqService>();

builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IAreaService, AreaService>();

builder.Services.AddScoped<IPostulacionRepository, PostulacionRepository>();
builder.Services.AddScoped<IPostulacionService, PostulacionService>();

builder.Services.AddScoped<INivelDominioRepository, NivelDominioRepository>();
builder.Services.AddScoped<INivelDominioService, NivelDominioService>();

builder.Services.AddScoped<IColaboradorSkillRepository, ColaboradorSkillRepository>();
builder.Services.AddScoped<IColaboradorSkillService, ColaboradorSkillService>();

builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<ISkillService, SkillService>();

builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IMatchingService, MatchingService>();

builder.Services.AddScoped<IExportacionService, ExportacionService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();

// REGISTRA EL FILTRO AQUÍ
builder.Services.AddScoped<TalentoInterno.API.Filters.AuditoriaFilter>();

// Register colaborador/colaboradorSkill services
builder.Services.AddScoped<IColaboradorSkillService, ColaboradorSkillService>();
builder.Services.AddScoped<IColaboradorSkillRepository, ColaboradorSkillRepository>();

builder.Services.AddScoped<ICertificacionRepository, CertificacionRepository>();
builder.Services.AddScoped<ICertificacionService, CertificacionService>();

builder.Services.AddScoped<IAlertaService, AlertaService>();
builder.Services.AddScoped<IKpiService, KpiService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddControllers();
//Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:9000") // <-- Cambia esto por la URL de tu frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// --- 1. Configuración de Autenticación JWT (¡AGREGA ESTO!) ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
        };
    });
// -------------------------------------------------------------
// --- Construcción de la App ---
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // ESTA ES LA LÍNEA CLAVE:
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();



//app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

// --- ¡IMPORTANTE! El orden importa aquí ---
app.UseAuthentication(); // 1. ¿Quién eres? (¡AGREGA ESTO!)
app.UseAuthorization();  // 2. ¿Qué puedes hacer?

app.MapControllers();

app.Run();