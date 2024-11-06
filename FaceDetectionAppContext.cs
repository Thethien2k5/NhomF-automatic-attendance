using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using FaceDetectionApp.Models;

namespace FaceDetectionApp
{
    public class FaceDetectionAppContext : IDisposable
    {
        private readonly MySqlConnection _connection;

        public FaceDetectionAppContext()
        {
            // Chuỗi kết nối MySQL - sửa các thông tin phù hợp với cấu hình của bạn
            string connectionString = "Server=localhost;Database=tự động điểm danh;User ID=root;Password=;";
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }

        // Phương thức để thêm sinh viên mới vào cơ sở dữ liệu
        public void AddStudent(Student student)
        {
            using (var cmd = new MySqlCommand("INSERT INTO thongtinhocsinh (Name, FaceData, Class) VALUES (@Name, @FaceData, @Class)", _connection))
            {
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@FaceData", student.FaceData);
                cmd.Parameters.AddWithValue("@Class", student.Class);
                cmd.ExecuteNonQuery();
            }
        }

        // Phương thức để lấy danh sách sinh viên từ cơ sở dữ liệu
        public List<Student> GetStudents()
        {
            var students = new List<Student>();

            using (var cmd = new MySqlCommand("SELECT * FROM thongtinhocsinh", _connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var student = new Student
                    {
                        Id = reader.GetInt32("ID"),
                        Name = reader.GetString("Name"),
                        FaceData = (byte[])reader["FaceData"],
                        Class = reader.GetString("Class")
                    };
                    students.Add(student);
                }
            }

            return students;
        }

        // Phương thức để ghi điểm danh
        public void AddAttendanceRecord(AttendanceRecord record)
        {
            using (var cmd = new MySqlCommand("INSERT INTO thongtindiemdanh (StudentId, AttendanceTime, Status) VALUES (@StudentId, @AttendanceTime, @Status)", _connection))
            {
                cmd.Parameters.AddWithValue("@StudentId", record.StudentId);
                cmd.Parameters.AddWithValue("@AttendanceTime", record.AttendanceTime);
                cmd.Parameters.AddWithValue("@Status", record.Status);
                cmd.ExecuteNonQuery();
            }
        }

        // Đảm bảo đóng kết nối khi không cần thiết
        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
