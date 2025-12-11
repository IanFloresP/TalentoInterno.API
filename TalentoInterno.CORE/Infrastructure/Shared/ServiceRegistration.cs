using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Core.Services;
using TalentoInterno.CORE.Core.Settings;


namespace UESAN.Ecommerce.CORE.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<JWTSettings>(_config.GetSection("JWTSettings"));

            services.AddTransient<IJwtService, JwtService>();

            var issuer = _config["JWTSettings:Issuer"];
            var audience = _config["JWTSettings:Audience"];
            var secretKey = _config["JWTSettings:SecretKey"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

               .AddJwtBearer(o =>
               {
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,

                       ValidIssuer = issuer,
                       ValidAudience = audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                   };
               });

            // Register new auditing, reporting, auth, dashboard and alerta services using fully-qualified names to avoid resolution issues
            services.AddScoped<TalentoInterno.CORE.Core.Interfaces.IAuditoriaService, TalentoInterno.CORE.Core.Services.AuditoriaService>();
            services.AddScoped<TalentoInterno.CORE.Core.Interfaces.IReporteService, TalentoInterno.CORE.Core.Services.ReporteService>();
            services.AddScoped<TalentoInterno.CORE.Core.Interfaces.IAuthService, TalentoInterno.CORE.Core.Services.AuthService>();
            services.AddScoped<TalentoInterno.CORE.Core.Interfaces.IDashboardService, TalentoInterno.CORE.Core.Services.DashboardService>();
            services.AddScoped<TalentoInterno.CORE.Core.Interfaces.IAlertaService, TalentoInterno.CORE.Core.Services.AlertaService>();
        }
    }
}
