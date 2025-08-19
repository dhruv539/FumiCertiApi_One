using FumicertiApi.Models;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel;


namespace FumicertiApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<Branch> branches { get; set; }
        public DbSet<AfoMember> afo_members { get; set; }
        public DbSet<ImgData> ImgData { get; set; }
        public DbSet<Notify> Notifies { get; set; }
        public DbSet<Models.Container> Containers { get; set; }    
        public DbSet<Certi> Certi { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<ReportData> ReportDatas { get; set; }
       public DbSet<Year> Years { get; set; }
        public DbSet<Bill> Bills { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.UserEmail).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.UserMobile).IsUnique();
        }
    }
}
