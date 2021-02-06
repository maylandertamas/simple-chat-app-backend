using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnBackendApp.Entities;

namespace SnBackendApp.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(u => u.Id);

            builder
                .Property(u => u.Id)
                .UseIdentityColumn()
                // Setup manually id start value to prevent id collision with seed data
                .HasIdentityOptions(startValue: 4);

            builder
                .Property(u => u.Username)
                .HasMaxLength(127)
                .IsRequired();

            builder.ToTable("users", "public");
        }
    }
}
