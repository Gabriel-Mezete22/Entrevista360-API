using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Dtos;

public record LoginRequest(
    [Required(ErrorMessage = "Informe o e-mail.")]
    string Email,
    [Required(ErrorMessage = "Informe a senha.")]
    string Senha);

public record RegistrarRequest(
    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(150, ErrorMessage = "O nome pode ter no máximo 150 caracteres.")]
    string Nome,
    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(200, ErrorMessage = "O e-mail pode ter no máximo 200 caracteres.")]
    string Email,
    [Required(ErrorMessage = "Informe a senha.")]
    [MinLength(8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
    [MaxLength(100, ErrorMessage = "A senha pode ter no máximo 100 caracteres.")]
    string Senha);

public record LoginResponse(string Nome, string Email);
