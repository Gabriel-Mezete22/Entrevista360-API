using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Repositorio.Contexto;

public class DataUtcConverter : ValueConverter<DateTime, DateTime>
{
    public DataUtcConverter()
        : base(
            data => data,
            data => DateTime.SpecifyKind(data, DateTimeKind.Utc))
    {
    }
}
