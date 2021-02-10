using Microsoft.EntityFrameworkCore;
using SimpleChatApp.Data.Configurations;
using SimpleChatApp.Entities;
using System.Collections.Generic;

namespace SimpleChatApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        // DB Sets
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        /// <summary>
        /// Add configurations for models
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());

            // Seed some data for configured entities
            Seed(modelBuilder);
        }

        /// <summary>
        /// Helper function to seed data
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void Seed(ModelBuilder modelBuilder)
        {
            // Create dummy users
            var users = new List<User>()
            {
            new User { Id = 1, Username = "Test User" },
            new User { Id = 2, Username = "John Doe" },
            new User { Id = 3, Username = "Jane Doe" },
            };

            var messages = new List<Message>();

            // Counter for proper message ids
            var messageIdCounter = 0;
            
            // Seed 10 messages for every user
            foreach(var user in users)
            {
                for(int i = 0; i < 10; i++)
                {
                    ++messageIdCounter;
                    messages.Add(new Message { Id = messageIdCounter, Text = $"Test message number {i} by {user.Username}", UserId = user.Id });
                }
            }

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Message>().HasData(messages);
        }
    }
}
