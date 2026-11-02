using Dominio.Entidades;

namespace Repositorio.Interfaces;

public interface IUsuarioRepositorio
{
    Task<Usuario?> ObterPorIdAsync(int id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<Usuario> CriarAsync(Usuario usuario);
}
