using HotelManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HotelManagement.Models
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; } // Manage the Hotels table
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public DbSet<Role> Roles { get; set; } // Add Role table


        // Default connection string name
        public HotelDbContext() : base("HotelDbContext")
        {
            // Set the database initializer
            Database.SetInitializer(new HotelDbInitializer());
        }

        // Database initializer for seeding data
        public class HotelDbInitializer : DropCreateDatabaseIfModelChanges<HotelDbContext>
        {
            protected override void Seed(HotelDbContext context)
            {
                var roles = new List<Role>
    {
        new Role { RoleName = "SuperAdmin", Description = "Full access to all features" },
        new Role { RoleName = "Manager", Description = "Manage hotels, rooms, and bookings" },
        new Role { RoleName = "Staff", Description = "Limited access to view bookings and rooms" }
    };

                roles.ForEach(role => context.Roles.Add(role));
                context.SaveChanges();

                // Assign SuperAdmin role to the default admin
                var superAdminRole = context.Roles.Single(r => r.RoleName == "SuperAdmin");
                context.Admins.Add(new Admin { Username = "superadmin", Password = "password123", RoleId = superAdminRole.RoleId });

                context.SaveChanges();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasRequired(a => a.Role) // Admin must have a role
                .WithMany(r => r.Admins) // Role can have many admins
                .HasForeignKey(a => a.RoleId); // Foreign key relationship

            base.OnModelCreating(modelBuilder);
        }
        //protected override void Seed(HotelDbContext context)
        //{
        //    context.Admins.Add(new Admin { Username = "admin", Password = "password123" }); // Replace with a secure hashed password
        //    base.Seed(context);
        //}


    }
}