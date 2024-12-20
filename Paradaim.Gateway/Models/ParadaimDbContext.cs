using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Paradaim.Gateway.Models
{
    public partial class ParadaimDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        // Constructor to pass DbContextOptions to the base class
        public ParadaimDbContext(DbContextOptions<ParadaimDbContext> options) 
            : base(options)
        {
        }

        // Overriding OnModelCreating to customize the model creation process
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set database collation and character set to support wide range of characters
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")   // Set collation for case-insensitive sorting
                .HasCharSet("utf8mb4");               // Set character set to utf8mb4 for better character support

            // Call the base method to ensure Identity configurations are applied
            base.OnModelCreating(modelBuilder);

            // Optional: If you want to add any custom model creation logic, place it here
            OnModelCreatingPartial(modelBuilder);
        }

        // This partial method is meant for any additional customization if needed
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        // Add DbSets for other models here, if you have them
        // Example: public DbSet<YourEntity> YourEntities { get; set; }
    }
}
