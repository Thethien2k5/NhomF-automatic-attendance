//kết nối với cơ sở dữ liệu
using Microsoft.EntityFrameworkCore;
using FaceDetectionApp.Models;

namespace FaceDetectionApp
{
    public class FaceDetectionAppContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=FaceDetectionApp.db");
        }
    }
}
