using System.Security.Cryptography;

namespace Aplicacao.Seguranca;

public static class HashDeSenha
{
    private const int TamanhoDoSal = 16;
    private const int TamanhoDoHash = 32;
    private const int Iteracoes = 210_000;
    private static readonly HashAlgorithmName Algoritmo = HashAlgorithmName.SHA256;

    public static string Gerar(string senha)
    {
        var sal = RandomNumberGenerator.GetBytes(TamanhoDoSal);
        var hash = Rfc2898DeriveBytes.Pbkdf2(senha, sal, Iteracoes, Algoritmo, TamanhoDoHash);

        return $"{Iteracoes}.{Convert.ToBase64String(sal)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verificar(string senha, string hashArmazenado)
    {
        var partes = hashArmazenado.Split('.');
        if (partes.Length != 3 || !int.TryParse(partes[0], out var iteracoes))
        {
            return false;
        }

        try
        {
            var sal = Convert.FromBase64String(partes[1]);
            var hashEsperado = Convert.FromBase64String(partes[2]);

            var hashCalculado = Rfc2898DeriveBytes.Pbkdf2(senha, sal, iteracoes, Algoritmo, hashEsperado.Length);

            return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
