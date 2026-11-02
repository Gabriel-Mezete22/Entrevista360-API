using Dominio.Entidades;

namespace Aplicacao.Interfaces;

public interface IUsuarioAplicacao
{
    Task<Usuario> RegistrarAsync(string nome, string email, string senha);

    Task<Usuario?> AutenticarAsync(string email, string senha);
}
