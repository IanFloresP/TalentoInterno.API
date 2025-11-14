using Microsoft.EntityFrameworkCore;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;
using TalentoInterno.CORE.Infrastructure.Data;
using TalentoInterno.CORE.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var _configuration = builder.Configuration;
var connectionString = _configuration.GetConnectionString("DevConnection");
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

builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
builder.Services.AddScoped<IColaboradorService, ColaboradorService>();

builder.Services.AddDbContext<TalentoInternooContext>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
//Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder//.WithOrigins("http://localhost:9000/#/login")
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();
