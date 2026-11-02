using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;
using Repositorio.Contexto;
using Repositorio.Interfaces;

namespace Repositorio;

public class EntrevistaRepositorio : BaseRepositorio<Entrevista>, IEntrevistaRepositorio
{
    public EntrevistaRepositorio(EntrevistaDbContext contexto) : base(contexto)
    {
    }

    public async Task<IEnumerable<Entrevista>> ObterTodasAsync(int? idVaga)
    {
        IQueryable<Entrevista> consulta = _contexto.Entrevistas
            .AsNoTracking()
            .Include(e => e.Vaga)
            .Include(e => e.Candidato)
            .Include(e => e.Avaliacao)
                .ThenInclude(a => a!.Criterios)
                    .ThenInclude(ac => ac.Criterio);

        if (idVaga is not null)
        {
            consulta = consulta.Where(e => e.IdVaga == idVaga.Value);
        }

        return await consulta
            .OrderByDescending(e => e.DataEnvio)
            .ToListAsync();
    }

    public async Task<Entrevista?> ObterPorIdAsync(int id)
    {
        return await _contexto.Entrevistas
            .AsNoTracking()
            .Include(e => e.Vaga)
                .ThenInclude(v => v!.Criterios)
            .Include(e => e.Candidato)
            .Include(e => e.Avaliacao)
                .ThenInclude(a => a!.Criterios)
                    .ThenInclude(ac => ac.Criterio)
            .AsSplitQuery()
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}
