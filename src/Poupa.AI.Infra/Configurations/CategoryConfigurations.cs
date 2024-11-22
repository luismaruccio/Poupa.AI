using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poupa.AI.Domain.Entities;

namespace Poupa.AI.Infra.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        private const string TableName = "categories";

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(e => new {e.Id, e.UserId});

            builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Type).IsRequired().HasConversion<int>();
            builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false);

            builder.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        }
    }
}
