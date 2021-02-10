using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatApp.Entities;


namespace SimpleChatApp.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasKey(m => m.Id);

            // Adds starts value for auto incrementing id property
            // Prevent conflicts with seeded data on table inserts
            builder
                .Property(m => m.Id)
                .UseIdentityColumn()
                .HasIdentityOptions(startValue: 60);

            builder
                .Property(m => m.Text)
                .HasMaxLength(1024)
                .IsRequired();

            // Use postgresql now() function to set default createion date on inserts
            // Preventing DateTime add tick tracking bug with seeded data on creating new migration files
            builder
                .Property(m => m.CreatedAt)
                .HasDefaultValueSql("now()");

            builder.ToTable("messages", "public");

        }

    }
}
