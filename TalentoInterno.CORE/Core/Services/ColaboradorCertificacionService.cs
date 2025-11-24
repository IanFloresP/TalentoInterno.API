using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Entities;
using TalentoInterno.CORE.Core.Interfaces;
using System.Linq;

namespace TalentoInterno.CORE.Core.Services;


public class ColaboradorCertificacionService : IColaboradorCertificacionService
{
    private readonly IColaboradorCertificacionRepository _repository;

    public ColaboradorCertificacionService(IColaboradorCertificacionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ColaboradorCertificacionDto>> GetCertificacionesAsync(int colaboradorId)
    {
        var certs = await _repository.GetByColaboradorIdAsync(colaboradorId);

        return certs.Select(c => new ColaboradorCertificacionDto
        {
            ColaboradorId = c.ColaboradorId,
            CertificacionId = c.CertificacionId,
            NombreCertificacion = c.Certificacion?.Nombre ?? "Desconocida",
            FechaObtencion = c.FechaObtencion
        });
    }

    public async Task AddCertificacionAsync(int colaboradorId, ColaboradorCertificacionCreateDto dto)
    {
        // Aquí podrías validar si ya la tiene asignada para no duplicar

        var nuevaCert = new ColaboradorCertificacion
        {
            ColaboradorId = colaboradorId,
            CertificacionId = dto.CertificacionId,
            FechaObtencion = dto.FechaObtencion
        };

        await _repository.AddAsync(nuevaCert);
        await _repository.SaveChangesAsync();
    }

    public async Task RemoveCertificacionAsync(int colaboradorId, int certificacionId)
    {
        await _repository.DeleteAsync(colaboradorId, certificacionId);
        await _repository.SaveChangesAsync();
    }
}
