using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;
using Repositorio.Contexto;
using Repositorio.Interfaces;

namespace Repositorio;

public class VagaRepositorio : BaseRepositorio<Vaga>, IVagaRepositorio
{
    public VagaRepositorio(EntrevistaDbContext contexto) : base(contexto)
    {
    }

    public async Task<IEnumerable<Vaga>> ObterTodasAsync()
    {
        return await _contexto.Vagas
            .AsNoTracking()
            .Include(v => v.Criterios)
            .Include(v => v.Entrevistas)
            .OrderByDescending(v => v.CriadaEm)
            .AsSplitQuery()
            .ToListAsync();
    }

    public async Task<Vaga?> ObterPorIdAsync(int id)
    {
        return await _contexto.Vagas
            .AsNoTracking()
            .Include(v => v.Criterios)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<bool> AtualizarAsync(Vaga vaga)
    {
        var v = await _contexto.Vagas
            .FirstOrDefaultAsync(x => x.Id == vaga.Id);
        if (v is null)
        {
            return false;
        }

        v.Titulo = vaga.Titulo;
        v.Descricao = vaga.Descricao;
        v.Status = vaga.Status;

        await _contexto.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PossuiEntrevistasAsync(int id)
    {
        return await _contexto.Entrevistas.AnyAsync(e => e.IdVaga == id);
    }
}
