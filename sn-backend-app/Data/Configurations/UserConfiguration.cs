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
                // Adds starts value for auto incrementing id property
                // Prevent conflicts with seeded data on table inserts
                .HasIdentityOptions(startValue: 4);

            builder
                .Property(u => u.Username)
                .HasMaxLength(127)
                .IsRequired();

            builder.ToTable("users", "public");
        }
    }
}
