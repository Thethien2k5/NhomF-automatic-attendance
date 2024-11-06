//Lưu trữ thông tin điêm danh
using System;

namespace FaceDetectionApp.Models
{
    public class AttendanceRecord
    {
        // public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime AttendanceTime { get; set; }

        public bool Status { get; set; }
    }
}
