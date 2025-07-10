using System;
using Microsoft.EntityFrameworkCore;
using FenNailStudio.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FenNailStudio.Infrastructure.Data
{
    public class FenNailStudioDbContext : DbContext
    {
        public FenNailStudioDbContext(DbContextOptions<FenNailStudioDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<NailService> Services { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 客戶配置
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Appointments)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId);

            // 美甲師配置
            modelBuilder.Entity<Technician>()
                .HasMany(t => t.Appointments)
                .WithOne(a => a.Technician)
                .HasForeignKey(a => a.TechnicianId);

            // 服務項目配置
            modelBuilder.Entity<NailService>()
                .HasMany(s => s.Appointments)
                .WithOne(a => a.Service)
                .HasForeignKey(a => a.ServiceId);

            // 預約配置
            modelBuilder.Entity<Appointment>()
                .Property(a => a.AppointmentDateTime)
                .IsRequired();

            // 種子資料
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // 種子資料 - 服務項目
            modelBuilder.Entity<NailService>().HasData(
                new NailService { Id = 1, Name = "卸甲", Description = "卸除舊款式、修剪、拋光和基礎護理。", Price = 500M, DurationMinutes = 60 },
                new NailService { Id = 3, Name = "款式施作", Description = "施作新款式。可選擇單色/優惠款/不挑款/指定款。", Price = 1000M, DurationMinutes = 90 },
                new NailService { Id = 2, Name = "卸甲 + 款式施作", Description = "卸甲服務全，加上施作新款式。可選擇單色/優惠款/不挑款/指定款。", Price = 1200M, DurationMinutes = 120 }
            );

            // 種子資料 - 美甲師
            modelBuilder.Entity<Technician>().HasData(
                new Technician { Id = 1, Name = "謝佩芬", Specialization = "日系 / 渲染 / 貓眼", WorkingHours = "週一至週五 10:00-18:00" },
                new Technician { Id = 2, Name = "陳佳珊", Specialization = "歐美系 / 貼鑽", WorkingHours = "週三至週日 12:00-20:00" }
            );
        }
    }
}
