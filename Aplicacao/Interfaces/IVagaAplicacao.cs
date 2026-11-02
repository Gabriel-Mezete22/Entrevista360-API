using Dominio.Entidades;

namespace Aplicacao.Interfaces;

public interface IVagaAplicacao
{
    Task<IEnumerable<Vaga>> ObterTodasAsync();
    Task<Vaga?> ObterPorIdAsync(int id);

    Task<Vaga> CriarAsync(Vaga vaga);

    Task<bool> AtualizarAsync(Vaga vaga);

    Task<bool> ExcluirAsync(int id);

    Task<Criterio?> AdicionarCriterioAsync(int idVaga, Criterio criterio);
    Task<bool> AtualizarCriterioAsync(int idVaga, Criterio criterio);
    Task<bool> ExcluirCriterioAsync(int idVaga, int idCriterio);
}
