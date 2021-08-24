using BataCMS.Data.Models;
using COHApp.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace BataCMS.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> 
    {

         public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
        {        
            
        }

        public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Dev_HPADB_Data;Trusted_Connection=True;MultipleActiveResultSets=true");

                return new AppDbContext(optionsBuilder.Options);
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .HasIndex(b => b.CategoryName).IsUnique(true);
            modelBuilder.Entity<ApplicationUser>().HasAlternateKey(p => new { p.IDNumber});
            modelBuilder.Entity<ApplicationUser>().HasIndex(p => p.PhoneNumber).IsUnique();


        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<HPAFacility> HPAFacilities { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<MemberSubscription> MemberSubscriptions { get; set; }
        public DbSet<MemberUser> MemberUsers { get; set; }
        public DbSet<MemberApplication> MemberApplications { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<MemberCertificate> MemberCertificates { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<DispatchedService> DispatchedServices { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}
