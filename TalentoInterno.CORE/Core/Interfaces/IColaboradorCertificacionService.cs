using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentoInterno.CORE.Core.DTOs;

namespace TalentoInterno.CORE.Core.Interfaces
{

    public interface IColaboradorCertificacionService
    {
        Task AddCertificacionAsync(int colaboradorId, ColaboradorCertificacionCreateDto dto);
        Task<IEnumerable<ColaboradorCertificacionDto>> GetCertificacionesAsync(global::System.Int32 colaboradorId);
        Task RemoveCertificacionAsync(global::System.Int32 colaboradorId, global::System.Int32 certificacionId);
    }
}
