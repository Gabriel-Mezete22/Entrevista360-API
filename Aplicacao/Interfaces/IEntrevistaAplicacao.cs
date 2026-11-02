using Dominio.Entidades;

namespace Aplicacao.Interfaces;

public interface IEntrevistaAplicacao
{
    Task<IEnumerable<Entrevista>> ObterTodasAsync(int? idVaga);
    Task<Entrevista?> ObterPorIdAsync(int id);

    Task<Entrevista> EnviarAsync(int idVaga, int idCandidato, string transcricao);

    Task<bool> ExcluirAsync(int id);
}
