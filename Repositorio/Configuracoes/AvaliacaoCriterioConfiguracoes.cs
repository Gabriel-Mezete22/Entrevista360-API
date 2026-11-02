using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Entidades;

namespace Repositorio.Configuracoes;

public class AvaliacaoCriterioConfiguracoes : IEntityTypeConfiguration<AvaliacaoCriterio>
{
    public void Configure(EntityTypeBuilder<AvaliacaoCriterio> builder)
    {
        builder.ToTable("AvaliacoesCriterio", t => t.HasCheckConstraint("CK_AvaliacoesCriterio_Nota", "[Nota] BETWEEN 0 AND 10"));

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nota)
            .HasPrecision(4, 2)
            .IsRequired();

        builder.Property(a => a.Justificativa)
            .HasMaxLength(1000);

        builder.HasOne(a => a.Avaliacao)
            .WithMany(av => av.Criterios)
            .HasForeignKey(a => a.IdAvaliacao)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Criterio)
            .WithMany()
            .HasForeignKey(a => a.IdCriterio)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(a => new { a.IdAvaliacao, a.IdCriterio })
            .IsUnique();
    }
}
