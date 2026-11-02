using Api.Controllers.Dtos;
using Aplicacao.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/autenticacao")]
public class AutenticacaoController : ControllerBase
{
    private readonly IUsuarioAplicacao _usuarioAplicacao;

    public AutenticacaoController(IUsuarioAplicacao usuarioAplicacao)
    {
        _usuarioAplicacao = usuarioAplicacao;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var usuario = await _usuarioAplicacao.AutenticarAsync(request.Email, request.Senha);

        if (usuario is null)
        {
            return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
        }

        return Ok(new LoginResponse(usuario.Nome, usuario.Email));
    }

    [HttpPost("registrar")]
    public async Task<ActionResult<LoginResponse>> Registrar(RegistrarRequest request)
    {
        var usuario = await _usuarioAplicacao.RegistrarAsync(request.Nome, request.Email, request.Senha);

        return StatusCode(StatusCodes.Status201Created, new LoginResponse(usuario.Nome, usuario.Email));
    }
}
