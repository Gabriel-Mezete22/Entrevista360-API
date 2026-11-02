using Aplicacao.Interfaces;
using Aplicacao.Seguranca;
using Dominio.Entidades;
using Repositorio.Interfaces;

namespace Aplicacao;

public class UsuarioAplicacao : IUsuarioAplicacao
{
    private const int TamanhoMinimoDaSenha = 8;

    private readonly IUsuarioRepositorio _usuarioRepositorio;

    public UsuarioAplicacao(IUsuarioRepositorio usuarioRepositorio)
    {
        _usuarioRepositorio = usuarioRepositorio;
    }

    public async Task<Usuario> RegistrarAsync(string nome, string email, string senha)
    {
        email = NormalizarEmail(email);

        if (senha.Length < TamanhoMinimoDaSenha)
        {
            throw new InvalidOperationException($"A senha deve ter pelo menos {TamanhoMinimoDaSenha} caracteres.");
        }

        if (await _usuarioRepositorio.ObterPorEmailAsync(email) is not null)
        {
            throw new InvalidOperationException("Já existe uma conta com este e-mail.");
        }

        var usuario = new Usuario
        {
            Nome = nome.Trim(),
            Email = email,
            SenhaHash = HashDeSenha.Gerar(senha),
        };

        return await _usuarioRepositorio.CriarAsync(usuario);
    }

    public async Task<Usuario?> AutenticarAsync(string email, string senha)
    {
        var usuario = await _usuarioRepositorio.ObterPorEmailAsync(NormalizarEmail(email));

        if (usuario is null || !HashDeSenha.Verificar(senha, usuario.SenhaHash))
        {
            return null;
        }

        return usuario;
    }

    private static string NormalizarEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }
}
