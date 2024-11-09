using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Poupa.AI.Domain.Entities;

namespace Poupa.AI.Infra.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        private const string TableName = "users";
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(150);
            builder.Property(e => e.Password).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasIndex(e => e.Email).IsUnique();
        }
    }
}
