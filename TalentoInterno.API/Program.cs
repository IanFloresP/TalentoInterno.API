using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;
using TalentoInterno.CORE.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Dependency Injection Configuration
builder.Services.AddScoped<IVacanteService, VacanteService>();
builder.Services.AddScoped<IVacanteRepository, VacanteRepository>();

builder.Services.AddScoped<IVacanteSkillReqService, VacanteSkillReqService>();
builder.Services.AddScoped<IVacanteSkillReqRepository, VacanteSkillReqRepository>();

builder.Services.AddScoped<ICuentaService, CuentaService>();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();

builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<IProyectoRepository, ProyectoRepository>();

builder.Services.AddScoped<IPerfilService, PerfilService>();
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();

builder.Services.AddScoped<IUrgenciaService, UrgenciaService>();
builder.Services.AddScoped<IUrgenciaRepository, UrgenciaRepository>();

builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();

builder.Services.AddScoped<INivelDominioService, NivelDominioService>();
builder.Services.AddScoped<INivelDominioRepository, NivelDominioRepository>();

builder.Services.AddScoped<ITipoSkillService, TipoSkillService>();
builder.Services.AddScoped<ITipoSkillRepository, TipoSkillRepository>();

// Register new departamento services
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();

// Register colaborador/colaboradorSkill services
builder.Services.AddScoped<IColaboradorSkillService, ColaboradorSkillService>();
builder.Services.AddScoped<IColaboradorSkillRepository, ColaboradorSkillRepository>();

// Register vacante requirement repo used by ColaboradorSkillService
builder.Services.AddScoped<IVacanteSkillReqRepository, VacanteSkillReqRepository>();

// Register rol services
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IRolRepository, RolRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
