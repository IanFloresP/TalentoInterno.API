// --- Importaciones necesarias ---
using Couchbase.Protostellar.KV.V1;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;
using TalentoInterno.CORE.Infrastructure.Data;
using TalentoInterno.CORE.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

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

// Servicios de ColaboradorSkill
builder.Services.AddScoped<IColaboradorSkillRepository, ColaboradorSkillRepository>();
builder.Services.AddScoped<IColaboradorSkillService, ColaboradorSkillService>();

// Servicios de Skill
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<ISkillService, SkillService>();

// Servicios de Rol
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IRolService, RolService>();

// Servicios de Vacante
builder.Services.AddScoped<IVacanteRepository, VacanteRepository>();
builder.Services.AddScoped<IVacanteService, VacanteService>();

// Servicios de VacanteSkillReq
builder.Services.AddScoped<IVacanteSkillReqRepository, VacanteSkillReqRepository>();
builder.Services.AddScoped<IVacanteSkillReqService, VacanteSkillReqService>();

builder.Services.AddScoped<INivelDominioRepository, NivelDominioRepository>();
builder.Services.AddScoped<INivelDominioService, NivelDominioService>();

// Servicios de Lógica de Negocio (Matching, KPI, Exportación)
builder.Services.AddScoped<IMatchingService, MatchingService>();

builder.Services.AddScoped<IExportacionService, ExportacionService>();

// --- 4. Configuración de CORS (¡Importante para tu Frontend!) ---
// Permite que tu app Quasar/Vue (ej: en localhost:9000) hable con tu API (en localhost:5066)
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

// --- Construcción de la App ---
var app = builder.Build();



app.UseHttpsRedirection();

// Usa la política de CORS que definimos
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();