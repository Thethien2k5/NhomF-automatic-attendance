//Lưu trữ thông tin học sinh
using System.ComponentModel.DataAnnotations;

namespace FaceDetectionApp.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] FaceData { get; set; } //Lưu trữ dữ liệu khuôn mặt dưới dạng một mảng byte

        public string Class { get; set; } // Thuộc tính mới để lưu trữ lớp học của học sinh
    }
}
