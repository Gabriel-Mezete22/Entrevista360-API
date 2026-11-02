using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Entidades;

namespace Repositorio.Configuracoes;

public class CriterioConfiguracoes : IEntityTypeConfiguration<Criterio>
{
    public void Configure(EntityTypeBuilder<Criterio> builder)
    {
        builder.ToTable("Criterios", t => t.HasCheckConstraint("CK_Criterios_Peso", "[Peso] BETWEEN 1 AND 5"));

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Descricao)
            .HasMaxLength(500);

        builder.Property(c => c.Peso)
            .IsRequired();
    }
}
