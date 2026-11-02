using Aplicacao.Interfaces;
using Aplicacao.Modelos;
using Dominio.Entidades;
using Dominio.Enums;
using Repositorio.Interfaces;

namespace Aplicacao;

public class AvaliacaoAplicacao : IAvaliacaoAplicacao
{
    private const decimal NotaMinima = 0m;
    private const decimal NotaMaxima = 10m;

    private readonly IAvaliacaoRepositorio _avaliacaoRepositorio;
    private readonly IEntrevistaRepositorio _entrevistaRepositorio;
    private readonly IVagaRepositorio _vagaRepositorio;

    public AvaliacaoAplicacao(
        IAvaliacaoRepositorio avaliacaoRepositorio,
        IEntrevistaRepositorio entrevistaRepositorio,
        IVagaRepositorio vagaRepositorio)
    {
        _avaliacaoRepositorio = avaliacaoRepositorio;
        _entrevistaRepositorio = entrevistaRepositorio;
        _vagaRepositorio = vagaRepositorio;
    }

    public async Task<RelatorioAvaliacao?> ObterRelatorioAsync(int idEntrevista)
    {
        var entrevista = await _entrevistaRepositorio.ObterPorIdAsync(idEntrevista);
        if (entrevista is null)
        {
            return null;
        }

        return MontarRelatorio(entrevista);
    }

    public async Task<RelatorioAvaliacao?> SalvarAsync(int idEntrevista, string? parecer, IEnumerable<NotaCriterioEntrada> notas)
    {
        var entrevista = await _entrevistaRepositorio.ObterPorIdAsync(idEntrevista);
        if (entrevista is null)
        {
            return null;
        }

        var criteriosDaVaga = entrevista.Vaga!.Criterios.ToDictionary(c => c.Id);

        var notasInformadas = notas.Where(n => n.Nota.HasValue).ToList();

        foreach (var item in notasInformadas)
        {
            if (!criteriosDaVaga.ContainsKey(item.IdCriterio))
            {
                throw new InvalidOperationException("Há uma nota para um critério que não pertence à vaga desta entrevista.");
            }

            var nota = item.Nota!.Value;
            if (nota < NotaMinima || nota > NotaMaxima)
            {
                throw new InvalidOperationException($"As notas devem estar entre {NotaMinima:0} e {NotaMaxima:0}.");
            }
        }

        if (notasInformadas.GroupBy(n => n.IdCriterio).Any(g => g.Count() > 1))
        {
            throw new InvalidOperationException("Cada critério pode receber apenas uma nota.");
        }

        var agora = DateTime.UtcNow;

        var avaliacao = new Avaliacao
        {
            IdEntrevista = idEntrevista,
            Parecer = LimparTexto(parecer),
            CriadaEm = agora,
            AtualizadaEm = agora,
            Criterios = notasInformadas.Select(n => new AvaliacaoCriterio
            {
                IdCriterio = n.IdCriterio,
                Nota = Math.Round(n.Nota!.Value, 2),
                Justificativa = LimparTexto(n.Justificativa),
            }).ToList(),
        };

        var todosAvaliados = criteriosDaVaga.Count > 0 && notasInformadas.Count == criteriosDaVaga.Count;
        var novoStatus = todosAvaliados ? StatusEntrevista.Avaliada : StatusEntrevista.Enviada;

        await _avaliacaoRepositorio.SalvarAsync(avaliacao, novoStatus);

        return await ObterRelatorioAsync(idEntrevista);
    }

    public async Task<ComparativoVaga?> ObterComparativoAsync(int idVaga)
    {
        var vaga = await _vagaRepositorio.ObterPorIdAsync(idVaga);
        if (vaga is null)
        {
            return null;
        }

        var criterios = vaga.Criterios.OrderBy(c => c.Id).ToList();
        var entrevistas = await _entrevistaRepositorio.ObterTodasAsync(idVaga);

        var linhas = entrevistas
            .Select(entrevista =>
            {
                var notasPorCriterio = entrevista.Avaliacao?.Criterios
                    .ToDictionary(c => c.IdCriterio, c => c.Nota)
                    ?? new Dictionary<int, decimal>();

                var notas = criterios
                    .Select(c => notasPorCriterio.TryGetValue(c.Id, out var nota) ? (decimal?)nota : null)
                    .ToList();

                return new LinhaComparativo(entrevista, entrevista.Avaliacao?.NotaGeral, notas);
            })
            .OrderByDescending(l => l.NotaGeral.HasValue)
            .ThenByDescending(l => l.NotaGeral)
            .ToList();

        return new ComparativoVaga(vaga, criterios, linhas);
    }

    private static RelatorioAvaliacao MontarRelatorio(Entrevista entrevista)
    {
        var avaliacao = entrevista.Avaliacao;

        var notasPorCriterio = avaliacao?.Criterios.ToDictionary(c => c.IdCriterio)
            ?? new Dictionary<int, AvaliacaoCriterio>();

        var linhas = entrevista.Vaga!.Criterios
            .OrderBy(c => c.Id)
            .Select(criterio =>
            {
                notasPorCriterio.TryGetValue(criterio.Id, out var nota);

                return new LinhaRelatorio(
                    criterio.Id,
                    criterio.Nome,
                    criterio.Descricao,
                    criterio.Peso,
                    nota?.Nota,
                    nota?.Justificativa);
            })
            .ToList();

        return new RelatorioAvaliacao(
            entrevista,
            avaliacao?.Parecer,
            avaliacao?.NotaGeral,
            avaliacao?.AtualizadaEm,
            linhas);
    }

    private static string? LimparTexto(string? texto)
    {
        return string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
    }
}
