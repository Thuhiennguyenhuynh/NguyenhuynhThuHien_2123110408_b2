
using NguyenhuynhThuHien.Domain.Entity;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace NguyenhuynhThuHien.Domain.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Dentist> Dentists { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Chair> Chairs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Cấu hình bảng Patient (Phone là duy nhất)
            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.Phone)
                .IsUnique();
            modelBuilder.Entity<Patient>()
                .Property(p => p.Phone)
                .HasMaxLength(15)
                .IsRequired();

            // 2. Cấu hình bảng Service (Cấu hình DECIMAL cho Price)
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(10,2)");

            // 3. Cấu hình bảng Appointment
            // Ràng buộc Unique: Tránh 1 nha sĩ (Dentist) bị trùng lịch (StartTime)
            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.DentistId, a.StartTime })
                .IsUnique()
                .HasDatabaseName("UX_Dentist_Time");

            // Ràng buộc Unique: Tránh 1 ghế (Chair) bị trùng lịch (StartTime)
            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.ChairId, a.StartTime })
                .IsUnique()
                .HasDatabaseName("UX_Chair_Time");

            // Ràng buộc Khóa ngoại (Tránh xóa dây chuyền - Cascade Delete)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Dentist)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DentistId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Chair)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.ChairId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Receptionist>()
                .HasOne(r => r.User)
                .WithOne()
                .HasForeignKey<Receptionist>(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
