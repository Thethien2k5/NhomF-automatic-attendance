﻿using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Mozilla;
using DTO;

namespace AdminManagement.Repository
{
    public class DatabaseConnection : IDisposable
    {
        private readonly MySqlConnection _connection;

        public DatabaseConnection()
        {
            // Chuỗi kết nối MySQL - sửa các thông tin phù hợp với cấu hình của bạn
            string connectionString = "Server=localhost;Database=autodata;User ID=root;Password=;";
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }

        //Học sinh
        // Phương thức để lấy danh sách sinh viên từ cơ sở dữ liệu
        public List<Student> GetStudents()
        {
            var students = new List<Student>();//Tạo danh sách học sinh

            using (var cmd = new MySqlCommand("SELECT * FROM thongtinhocsinh", _connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())//Đọc và đưa thông tin học sinh vào danh sách
                    {
                        var student = new Student
                        {
                            ID = reader.GetInt32("ID"),
                            NameStudent = reader.GetString("NameStudent"),
                            Class = reader.GetString("Class"),
                            Parents = reader.GetString("Parents"),
                            ParentEmail = reader.GetString("ParentEmail"),
                        };
                        students.Add(student);
                    }
                }
            }
            return students;
        }
        //Lấy tên lớp
        public List<ClassStudent> GetClass()
        {
            var ClassL = new List<ClassStudent>();
            using (var NameClass = new MySqlCommand("SELECT Class FROM thongtinhocsinh", _connection))
            {
                using (var reader = NameClass.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var Class_ = new ClassStudent
                        {
                            Class = reader.GetString("Class"),
                        };
                        ClassL.Add(Class_);

                    }
                }
            }
            return ClassL;
        }
        // Phương thức để thêm sinh viên mới vào cơ sở dữ liệu
        public void AddStudent(Student student)
        {
            using (
                var cmd = new MySqlCommand(
"INSERT INTO thongtinhocsinh (ID, NameStudent, Class, Parents, ParentEmail) VALUES (@ID, @NameStudent, @Class, @Parents, @ParentEmail)", _connection))
            {
                cmd.Parameters.AddWithValue("@ID", student.ID);
                cmd.Parameters.AddWithValue("@NameStudent", student.NameStudent);
                cmd.Parameters.AddWithValue("@Class", student.Class);
                cmd.Parameters.AddWithValue("@Parents", student.Parents);
                cmd.Parameters.AddWithValue("@ParentEmail", student.ParentEmail);
                cmd.ExecuteNonQuery();
            }
        }
        //Xóa học sinh theo ID
        public void DeleteStudent(int studentID)
        {
            using (var cmd = new MySqlCommand("DELETE FROM thongtinhocsinh WHERE ID = @ID", _connection))
            {
                cmd.Parameters.AddWithValue("@ID", studentID);
                cmd.ExecuteNonQuery();
            }
        }
        //Kiểm tra data có trong bảng hay không
        public bool IsStudentIdExists(int studentId)
        {
            using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM thongtinhocsinh WHERE ID = @ID", _connection))
            {
                cmd.Parameters.AddWithValue("@ID", studentId);
                var result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0;
            }
        }
        //Cập nhật thông tin học sinh
        public void UpdateStudentInDatabase(int ID, string name, string Class, string parents, string ParentEmail)
        {
            using (
                var cmd = new MySqlCommand(
"UPDATE thongtinhocsinh SET NameStudent = @Name, Class = @Class, Parents = @Parents, ParentEmail = @ParentEmail WHERE ID = @ID", _connection))
            {
                cmd.Parameters.AddWithValue("@ID", ID);  // ID không thay đổi
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Class", Class);
                cmd.Parameters.AddWithValue("@Parents", parents);
                cmd.Parameters.AddWithValue("@ParentEmail", ParentEmail);
                cmd.ExecuteNonQuery();
            }
        }
        //
        //
        //Giáo viên
        // Đảm bảo đóng kết nối khi không cần thiết
        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
