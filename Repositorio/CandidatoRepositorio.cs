using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;
using Repositorio.Contexto;
using Repositorio.Interfaces;

namespace Repositorio;

public class CandidatoRepositorio : BaseRepositorio<Candidato>, ICandidatoRepositorio
{
    public CandidatoRepositorio(EntrevistaDbContext contexto) : base(contexto)
    {
    }

    public async Task<IEnumerable<Candidato>> ObterTodosAsync()
    {
        return await _contexto.Candidatos
            .AsNoTracking()
            .OrderBy(c => c.Nome)
            .ToListAsync();
    }

    public async Task<Candidato?> ObterPorIdAsync(int id)
    {
        return await _contexto.Candidatos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> AtualizarAsync(Candidato candidato)
    {
        var existente = await _contexto.Candidatos
            .FirstOrDefaultAsync(c => c.Id == candidato.Id);
        if (existente is null)
        {
            return false;
        }

        existente.Nome = candidato.Nome;
        existente.Email = candidato.Email;

        await _contexto.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PossuiEntrevistasAsync(int id)
    {
        return await _contexto.Entrevistas.AnyAsync(e => e.IdCandidato == id);
    }
}
