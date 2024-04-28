using Intercon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Intercon.Infrastructure.Persistence.Configurations;

public class FileDataConfiguration : IEntityTypeConfiguration<FileData>
{
    public void Configure(EntityTypeBuilder<FileData> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Path).IsRequired();

        builder.Property(i => i.BusinessId).IsRequired(false);
    }
}