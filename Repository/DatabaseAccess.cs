using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using MySql.Data.MySqlClient;

namespace Camera.Repository
{
    public class DatabaseAccess
    {
        private readonly MySqlConnection _connection;

        public DatabaseAccess()
        {
            string connectionString = "Server=localhost; Database=automaticattendancesoftware; User ID=root; Password=;";
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }

        // Lấy danh sách các ID học sinh hợp lệ từ bảng studentinfo
        public bool CheckValidStudentIds(int ID)
        {
            using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM studentinformation WHERE IDStudent = @IDStudent", _connection))
            {
                cmd.Parameters.AddWithValue("@IDStudent", ID);
                var result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0;
            }
        }

        // Đánh dấu điểm danh cho học sinh và lưu vào bảng attendance
        public bool MarkAttendance(string studentId)
        {
            // Lưu điểm danh vào bảng attentioncheck
            string insertQuery = "INSERT INTO attendance (IDStudent, NameStudent, Status, DateAttendance ) " +
                                 "SELECT IDStudent , NameStudent , 1, NOW() FROM studentinformation WHERE IDStudent = @studentId";

            MySqlCommand insertCmd = new MySqlCommand(insertQuery, _connection);
            insertCmd.Parameters.AddWithValue("@studentId", studentId);

            // Thực thi câu lệnh SQL để lưu điểm danh
            MySqlTransaction transaction = _connection.BeginTransaction();
            try
            {
                insertCmd.Transaction = transaction;
                insertCmd.ExecuteNonQuery();
                transaction.Commit();
                return true; // Điểm danh thành công
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false; // Xử lý lỗi
            }
        }

        // Lấy dữ liệu điểm danh từ bảng 
        public List<studentRolClall> GetAttendanceData()
        {
           var dataTable = new List<studentRolClall>();

            using (var NameClass = new MySqlCommand("SELECT IDStudent, NameStudent, Class FROM  studentinformation", _connection))
            {
                using (var reader = NameClass.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _ = new studentRolClall
                        {
                            ID = reader.GetInt32("IDStudent"),
                            NameStudent = reader.GetString("NameStudent"),
                            Class = reader.GetString("Class"),
                        };
                        dataTable.Add(_);

                    }
                }
            }
            return dataTable;
        }
        //Lấy tên lớp
        public List<ClassStudent> GetDataClass()
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
        //Luu hoc sinh
        public void AddStudentStatus(List<studentRolClall> student)
        { 

            var _ = new studentRolClall();
            for (int i = 0; i < student.Count; i++)
            {
                _ = student[i];
                using (var cmd = new MySqlCommand(
                   "INSERT INTO attendance (IDStudent, NameStudent, Class, Date, Status) VALUES (@IDStudent, @NameStudent, @Class, @Date, @Status)", _connection))
                {
                    // Thêm tham số vào câu lệnh SQL
                    cmd.Parameters.AddWithValue("@IDStudent", _.ID);
                    cmd.Parameters.AddWithValue("@NameStudent", _.NameStudent);
                    cmd.Parameters.AddWithValue("@Class", _.Class);
                    cmd.Parameters.AddWithValue("@Date", _.Date);
                    cmd.Parameters.AddWithValue("@Status", _.status);

                    // Thực thi câu lệnh SQL
                    cmd.ExecuteNonQuery();
                }

            }
           
        }
        public string GetEmail(int Id)
        {
            // Sử dụng câu lệnh SQL để truy vấn email phụ huynh theo ID học sinh
            using (var cmd = new MySqlCommand("SELECT ParentsEmail FROM studentinformation WHERE IDStudent = @IDStudent", _connection))
            {
                // Thêm tham số ID học sinh vào câu lệnh SQL
                cmd.Parameters.AddWithValue("@IDStudent", Id);

                // Mở kết nối cơ sở dữ liệu và thực thi câu lệnh
                using (var reader = cmd.ExecuteReader())
                {
                    // Kiểm tra xem có kết quả trả về không
                    if (reader.Read())
                    {
                        // Lấy giá trị email từ cột "ParentsEmail"
                        var email = reader.GetString("ParentsEmail");
                        return email; // Trả về email phụ huynh
                    }
                    else
                    {
                        // Nếu không có kết quả, có thể trả về null hoặc thông báo lỗi
                        return null; // Không tìm thấy email
                    }
                }
            }
        }

    }
}
