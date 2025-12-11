using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentoInterno.CORE.Core.Interfaces;

namespace TalentoInterno.CORE.Core.Settings
{
    public class JWTSettings
    {
        public string SecretKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public double DurationInMinutes { get; set; }
    }
}