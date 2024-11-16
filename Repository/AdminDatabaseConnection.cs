using System;
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
            string connectionString = "Server=localhost;Database=automaticattendancesoftware;User ID=root;Password=;";
            _connection = new MySqlConnection(connectionString);
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        //Học sinh
        // Phương thức để lấy danh sách sinh viên từ cơ sở dữ liệu
        public List<Student> GetStudents()
        {
            var students = new List<Student>();//Tạo danh sách học sinh

            using (var cmd = new MySqlCommand("SELECT * FROM studentinformation", _connection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())//Đọc và đưa thông tin học sinh vào danh sách
                    {
                        var student = new Student
                        {
                            ID = reader.GetInt32("IDStudent"),
                            NameStudent = reader.GetString("NameStudent"),
                            Class = reader.GetString("Class"),
                            Parents = reader.GetString("Parents"),
                            ParentEmail = reader.GetString("ParentsEmail"),
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
            using (var NameClass = new MySqlCommand("SELECT Class FROM studentinformation", _connection))
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
            try
            {
                using (var cmd = new MySqlCommand(
                    "INSERT INTO studentinformation (IDStudent, NameStudent, Class, Parents, ParentsEmail) VALUES (@IDStudent, @NameStudent, @Class, @Parents, @ParentEmail)", _connection))
                {
                    // Thêm tham số vào câu lệnh SQL
                    cmd.Parameters.AddWithValue("@IDStudent", student.ID);
                    cmd.Parameters.AddWithValue("@NameStudent", student.NameStudent);
                    cmd.Parameters.AddWithValue("@Class", student.Class);
                    cmd.Parameters.AddWithValue("@Parents", student.Parents);
                    cmd.Parameters.AddWithValue("@ParentEmail", student.ParentEmail);

                    // Thực thi câu lệnh SQL
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi AdminAddStudent: " + ex.Message);
            }
        }

        //Xóa học sinh theo ID
        public void DeleteStudent(int studentID)
        {
            try
            {
                using (var cmd = new MySqlCommand("DELETE FROM studentinformation WHERE IDStudent = @IDStudent", _connection))
                {
                    cmd.Parameters.AddWithValue("@IDStudent", studentID);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Không tìm thấy học sinh với ID: " + studentID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi DeleteStudent: " + ex.Message);
            }
        }

        //Kiểm tra data có trong bảng hay không
        public bool IsStudentIdExists(int studentId)
        {
            try
            {
                using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM studentinformation WHERE IDStudent = @IDStudent", _connection))
                {

                    cmd.Parameters.AddWithValue("@IDStudent", studentId);
                    var result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi IsStudentTdExists: " + ex.Message);
            }

        }
        //Cập nhật thông tin học sinh
        public void UpdateStudentInDatabase(int ID, string name, string Class, string parents, string ParentEmail)
        {
            using (
                var cmd = new MySqlCommand(
"UPDATE studentinformation SET NameStudent = @Name, Class = @Class, Parents = @Parents, ParentsEmail = @ParentEmail WHERE IDStudent = @IDStudent", _connection))
            {
                cmd.Parameters.AddWithValue("@IDStudent", ID);  // ID không thay đổi
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
