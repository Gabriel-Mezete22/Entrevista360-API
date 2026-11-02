using Microsoft.EntityFrameworkCore;
using Dominio.Entidades;
using Repositorio.Contexto;
using Repositorio.Interfaces;

namespace Repositorio;

public class UsuarioRepositorio : BaseRepositorio<Usuario>, IUsuarioRepositorio
{
    public UsuarioRepositorio(EntrevistaDbContext contexto) : base(contexto)
    {
    }

    public async Task<Usuario?> ObterPorIdAsync(int id)
    {
        return await _contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await _contexto.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
