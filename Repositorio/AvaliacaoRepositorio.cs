using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;
using Dominio.Enums;
using Repositorio.Contexto;
using Repositorio.Interfaces;

namespace Repositorio;

public class AvaliacaoRepositorio : BaseRepositorio<Avaliacao>, IAvaliacaoRepositorio
{
    public AvaliacaoRepositorio(EntrevistaDbContext contexto) : base(contexto)
    {
    }

    public async Task SalvarAsync(Avaliacao avaliacao, StatusEntrevista statusDaEntrevista)
    {
        var existente = await _contexto.Avaliacoes
            .Include(a => a.Criterios)
            .FirstOrDefaultAsync(a => a.IdEntrevista == avaliacao.IdEntrevista);

        if (existente is null)
        {
            _contexto.Avaliacoes.Add(avaliacao);
        }
        else
        {
            existente.Parecer = avaliacao.Parecer;
            existente.AtualizadaEm = avaliacao.AtualizadaEm;

            var idsRecebidos = avaliacao.Criterios.Select(c => c.IdCriterio).ToHashSet();
            var removidos = existente.Criterios
                .Where(c => !idsRecebidos.Contains(c.IdCriterio))
                .ToList();
            _contexto.AvaliacoesCriterio.RemoveRange(removidos);

            foreach (var recebido in avaliacao.Criterios)
            {
                var atual = existente.Criterios.FirstOrDefault(c => c.IdCriterio == recebido.IdCriterio);
                if (atual is null)
                {
                    existente.Criterios.Add(recebido);
                }
                else
                {
                    atual.Nota = recebido.Nota;
                    atual.Justificativa = recebido.Justificativa;
                }
            }
        }

        var entrevista = await _contexto.Entrevistas.FindAsync(avaliacao.IdEntrevista);
        if (entrevista is not null)
        {
            entrevista.Status = statusDaEntrevista;
        }

        await _contexto.SaveChangesAsync();
    }
}
