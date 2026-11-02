using Dominio.Entidades;

namespace Aplicacao.Interfaces;

public interface ICandidatoAplicacao
{
    Task<IEnumerable<Candidato>> ObterTodosAsync();
    Task<Candidato?> ObterPorIdAsync(int id);
    Task<Candidato> CriarAsync(Candidato candidato);
    Task<bool> AtualizarAsync(Candidato candidato);

    Task<bool> ExcluirAsync(int id);
}
