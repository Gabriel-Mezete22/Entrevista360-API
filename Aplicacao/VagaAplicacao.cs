using Aplicacao.Interfaces;
using Dominio.Entidades;
using Dominio.Enums;
using Repositorio.Interfaces;

namespace Aplicacao;

public class VagaAplicacao : IVagaAplicacao
{
    private const int PesoMinimo = 1;
    private const int PesoMaximo = 5;

    private readonly IVagaRepositorio _vagaRepositorio;
    private readonly ICriterioRepositorio _criterioRepositorio;

    public VagaAplicacao(IVagaRepositorio vagaRepositorio, ICriterioRepositorio criterioRepositorio)
    {
        _vagaRepositorio = vagaRepositorio;
        _criterioRepositorio = criterioRepositorio;
    }

    public async Task<IEnumerable<Vaga>> ObterTodasAsync()
    {
        return await _vagaRepositorio.ObterTodasAsync();
    }

    public async Task<Vaga?> ObterPorIdAsync(int id)
    {
        return await _vagaRepositorio.ObterPorIdAsync(id);
    }

    public async Task<Vaga> CriarAsync(Vaga vaga)
    {
        vaga.Id = 0;
        vaga.Status = StatusVaga.Aberta;
        vaga.CriadaEm = DateTime.UtcNow;
        vaga.Titulo = vaga.Titulo.Trim();
        vaga.Descricao = vaga.Descricao.Trim();

        foreach (var criterio in vaga.Criterios)
        {
            NormalizarCriterio(criterio);
        }
        ValidarCriterios(vaga.Criterios);

        return await _vagaRepositorio.CriarAsync(vaga);
    }

    public async Task<bool> AtualizarAsync(Vaga vaga)
    {
        vaga.Titulo = vaga.Titulo.Trim();
        vaga.Descricao = vaga.Descricao.Trim();

        return await _vagaRepositorio.AtualizarAsync(vaga);
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var vaga = await _vagaRepositorio.ObterPorIdAsync(id);
        if (vaga is null)
        {
            return false;
        }

        if (await _vagaRepositorio.PossuiEntrevistasAsync(id))
        {
            throw new InvalidOperationException("Esta vaga já tem entrevistas e não pode ser excluída. Encerre a vaga para parar de usá-la.");
        }

        return await _vagaRepositorio.ExcluirAsync(id);
    }

    public async Task<Criterio?> AdicionarCriterioAsync(int idVaga, Criterio criterio)
    {
        var vaga = await _vagaRepositorio.ObterPorIdAsync(idVaga);
        if (vaga is null)
        {
            return null;
        }

        criterio.Id = 0;
        criterio.IdVaga = idVaga;
        NormalizarCriterio(criterio);

        ValidarCriterios(vaga.Criterios.Append(criterio));

        return await _criterioRepositorio.CriarAsync(criterio);
    }

    public async Task<bool> AtualizarCriterioAsync(int idVaga, Criterio criterio)
    {
        var vaga = await _vagaRepositorio.ObterPorIdAsync(idVaga);
        if (vaga is null || !vaga.Criterios.Any(c => c.Id == criterio.Id))
        {
            return false;
        }

        criterio.IdVaga = idVaga;
        NormalizarCriterio(criterio);

        var criterios = vaga.Criterios.Where(c => c.Id != criterio.Id).Append(criterio);
        ValidarCriterios(criterios);

        return await _criterioRepositorio.AtualizarAsync(criterio);
    }

    public async Task<bool> ExcluirCriterioAsync(int idVaga, int idCriterio)
    {
        var vaga = await _vagaRepositorio.ObterPorIdAsync(idVaga);
        if (vaga is null || !vaga.Criterios.Any(c => c.Id == idCriterio))
        {
            return false;
        }

        if (await _criterioRepositorio.EstaEmUsoAsync(idCriterio))
        {
            throw new InvalidOperationException("Este critério já recebeu notas em avaliações e não pode ser removido.");
        }

        return await _criterioRepositorio.ExcluirAsync(idCriterio);
    }

    private static void NormalizarCriterio(Criterio criterio)
    {
        criterio.Nome = criterio.Nome.Trim();
        criterio.Descricao = string.IsNullOrWhiteSpace(criterio.Descricao) ? null : criterio.Descricao.Trim();
    }

    private static void ValidarCriterios(IEnumerable<Criterio> criterios)
    {
        var nomesVistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var criterio in criterios)
        {
            if (criterio.Peso < PesoMinimo || criterio.Peso > PesoMaximo)
            {
                throw new InvalidOperationException($"O peso do critério \"{criterio.Nome}\" deve estar entre {PesoMinimo} e {PesoMaximo}.");
            }

            if (!nomesVistos.Add(criterio.Nome))
            {
                throw new InvalidOperationException($"O critério \"{criterio.Nome}\" aparece mais de uma vez nesta vaga.");
            }
        }
    }
}
