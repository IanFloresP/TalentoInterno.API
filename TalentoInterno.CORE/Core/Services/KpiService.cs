using TalentoInterno.CORE.Core.DTOs;
using TalentoInterno.CORE.Core.Interfaces;
using TalentoInterno.CORE.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TalentoInterno.CORE.Core.Services;

public class KpiService : IKpiService
{
    private readonly TalentoInternooContext _context;
    private readonly IMatchingService _matchingService;

    public KpiService(TalentoInternooContext context, IMatchingService matchingService)
    {
        _context = context;
        _matchingService = matchingService;
    }

    public async Task<KpiReportDto> GetKpiDataAsync(DateTime? desde = null, DateTime? hasta = null)
    {
        // --- 1. PREPARAR CONSULTAS CON FILTROS ---
        var queryVacantes = _context.Vacante.AsQueryable();
        var queryColab = _context.Colaborador.AsQueryable();

        if (desde.HasValue)
        {
            var fechaDesde = DateOnly.FromDateTime(desde.Value);
            queryVacantes = queryVacantes.Where(v => v.FechaInicio >= fechaDesde);
            queryColab = queryColab.Where(c => c.FechaAlta >= fechaDesde);
        }

        if (hasta.HasValue)
        {
            var fechaHasta = DateOnly.FromDateTime(hasta.Value);
            queryVacantes = queryVacantes.Where(v => v.FechaInicio <= fechaHasta);
            queryColab = queryColab.Where(c => c.FechaAlta <= fechaHasta);
        }

        // --- 2. EJECUTAR CONTEOS BÁSICOS ---
        var vacantesAbiertas = await queryVacantes
            .CountAsync(v => v.Estado == "Abierta");

        var totalColab = await queryColab
            .CountAsync(c => c.Activo == true);


        // --- 3. CÁLCULO AVANZADO: MATCH PROMEDIO ---
        var vacantesIds = await queryVacantes
            .Where(v => v.Estado == "Abierta")
            .Select(v => v.VacanteId)
            .ToListAsync();

        double promedioMatch = 0;

        if (vacantesIds.Any())
        {
            // CAMBIO: Usamos List<double> en lugar de decimal
            var mejoresPuntajes = new List<double>();

            foreach (var vacanteId in vacantesIds)
            {
                var candidatos = await _matchingService.GetRankedCandidatesAsync(vacanteId);

                var mejorCandidato = candidatos.FirstOrDefault();

                // CAMBIO: Usamos double para coincidir con el DTO
                // (PorcentajeMatch es double, así que 'mejorScore' debe ser double)
                double mejorScore = mejorCandidato?.PorcentajeMatch ?? 0;

                mejoresPuntajes.Add(mejorScore);
            }

            if (mejoresPuntajes.Any())
            {
                promedioMatch = mejoresPuntajes.Average();
            }
        }

        // --- 4. RETORNAR DTO ---
        return new KpiReportDto
        {
            TotalVacantesAbiertas = vacantesAbiertas,
            TotalColaboradoresActivos= totalColab,
            MatchPromedioGeneral = Math.Round(promedioMatch, 2)
        };
    }
}