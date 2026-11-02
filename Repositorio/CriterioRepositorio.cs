using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;
using Repositorio.Contexto;
using Repositorio.Interfaces;

namespace Repositorio;

public class CriterioRepositorio : BaseRepositorio<Criterio>, ICriterioRepositorio
{
    public CriterioRepositorio(EntrevistaDbContext contexto) : base(contexto)
    {
    }

    public async Task<bool> AtualizarAsync(Criterio criterio)
    {
        var crit = await _contexto.Criterios.FindAsync(criterio.Id);
        if (crit is null)
            return false;

        crit.Nome = criterio.Nome;
        crit.Descricao = criterio.Descricao;
        crit.Peso = criterio.Peso;

        await _contexto.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EstaEmUsoAsync(int id)
    {
        return await _contexto.AvaliacoesCriterio.AnyAsync(a => a.IdCriterio == id);
    }
}
