using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Entidades;

namespace Repositorio.Configuracoes;

public class AvaliacaoConfiguracoes : IEntityTypeConfiguration<Avaliacao>
{
    public void Configure(EntityTypeBuilder<Avaliacao> builder)
    {
        builder.ToTable("Avaliacoes");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Parecer)
            .HasMaxLength(2000);

        builder.Ignore(a => a.NotaGeral);

        builder.HasOne(a => a.Entrevista)
            .WithOne(e => e.Avaliacao)
            .HasForeignKey<Avaliacao>(a => a.IdEntrevista)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.IdEntrevista)
            .IsUnique();
    }
}
