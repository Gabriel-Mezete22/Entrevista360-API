using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Api.Servicos;

public static class LeitorTranscricao
{
    public static readonly string[] ExtensoesPermitidas = [".txt", ".docx"];

    private static readonly XNamespace W = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

    public static bool ExtensaoPermitida(string nomeArquivo)
    {
        return ExtensoesPermitidas.Contains(Path.GetExtension(nomeArquivo), StringComparer.OrdinalIgnoreCase);
    }

    public static async Task<string?> LerAsync(IFormFile arquivo)
    {
        await using var fluxo = arquivo.OpenReadStream();

        if (string.Equals(Path.GetExtension(arquivo.FileName), ".docx", StringComparison.OrdinalIgnoreCase))
        {
            return await LerDocxAsync(fluxo);
        }

        using var leitor = new StreamReader(fluxo);
        return await leitor.ReadToEndAsync();
    }

    private static async Task<string?> LerDocxAsync(Stream fluxo)
    {
        try
        {
            await using var zip = await ZipArchive.CreateAsync(fluxo, ZipArchiveMode.Read, leaveOpen: false, entryNameEncoding: null);
            var documento = zip.GetEntry("word/document.xml");
            if (documento is null)
            {
                return null;
            }

            await using var conteudo = await documento.OpenAsync();
            var xml = await XDocument.LoadAsync(conteudo, LoadOptions.None, CancellationToken.None);

            var texto = new StringBuilder();
            foreach (var paragrafo in xml.Descendants(W + "p"))
            {
                foreach (var elemento in paragrafo.Descendants())
                {
                    if (elemento.Name == W + "t")
                    {
                        texto.Append(elemento.Value);
                    }
                    else if (elemento.Name == W + "tab")
                    {
                        texto.Append('\t');
                    }
                    else if (elemento.Name == W + "br" || elemento.Name == W + "cr")
                    {
                        texto.Append('\n');
                    }
                }

                texto.Append('\n');
            }

            return texto.ToString();
        }
        catch (Exception e) when (e is InvalidDataException or XmlException)
        {
            return null;
        }
    }
}
