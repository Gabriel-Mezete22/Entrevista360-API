using Repositorio.Contexto;

namespace Repositorio;

public abstract class BaseRepositorio<T> where T : class
{
    protected readonly EntrevistaDbContext _contexto;

    protected BaseRepositorio(EntrevistaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<T> CriarAsync(T entidade)
    {
        _contexto.Set<T>().Add(entidade);
        await _contexto.SaveChangesAsync();
        return entidade;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var entidade = await _contexto.Set<T>().FindAsync(id);
        if (entidade is null) return false;

        _contexto.Set<T>().Remove(entidade);
        await _contexto.SaveChangesAsync();
        return true;
    }
}
