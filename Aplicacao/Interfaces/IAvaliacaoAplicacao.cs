using Aplicacao.Modelos;

namespace Aplicacao.Interfaces;

public interface IAvaliacaoAplicacao
{
    Task<RelatorioAvaliacao?> ObterRelatorioAsync(int idEntrevista);

    Task<RelatorioAvaliacao?> SalvarAsync(int idEntrevista, string? parecer, IEnumerable<NotaCriterioEntrada> notas);

    Task<ComparativoVaga?> ObterComparativoAsync(int idVaga);
}
