using Dominio.Entidades;

namespace Repositorio.Interfaces;

public interface IEntrevistaRepositorio
{
    Task<IEnumerable<Entrevista>> ObterTodasAsync(int? idVaga);

    Task<Entrevista?> ObterPorIdAsync(int id);

    Task<Entrevista> CriarAsync(Entrevista entrevista);
    Task<bool> ExcluirAsync(int id);
}
