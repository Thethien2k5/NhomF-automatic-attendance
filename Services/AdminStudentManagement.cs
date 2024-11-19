using AdminManagement.Repository;
using DTO;



namespace AdminManagement.Services
{
    public partial class MainAdmin
    {
        //Thay đổi thông tin 1 học sinh trong data base
        public bool TableStudentsCellEndEdit(int ID, string name, string studentClass, string parents, string ParentEmail)
        {
            using (var db = new DatabaseConnection())
            {
                db.UpdateStudentInDatabase(ID, name, studentClass, parents, ParentEmail);
            }
            return true;
        }
        //Lấy danh sách học sinh ra
        public List<Student> GetLoadStudentList()
        {
            List<Student> StudentsList = new List<Student>();
            using (var db = new DatabaseConnection())
            {
                var students = db.GetStudents();
                StudentsList.Clear();
                foreach (var student in students)
                {
                    StudentsList.Add(student);
                }
            }
            return StudentsList;
        }
        //Thêm học sinh và kiểm tra trước khi thêm
        public void AdminAddStudent(Student student)
        {
            // Kiểm tra ID: phải là số nguyên 9 chữ số
            if (student.ID.ToString().Length != 5)
            {
                throw new Exception("ID không hợp lệ. ID phải là số nguyên gồm 5 chữ số.");
            }

            // Kiểm tra tên: mỗi chữ cái đầu phải viết hoa
            if (student.NameStudent != null && !IsValidName(student.NameStudent))
            {
                throw new Exception("Tên không hợp lệ. Tên phải có chữ cái đầu viết hoa ở mỗi từ.");
            }

            // Kiểm tra lớp
            if (string.IsNullOrWhiteSpace(student.Class))
            {
                throw new Exception("Tên lớp không được để trống.");
            }

            // Kiểm tra tên phụ huynh
            if (string.IsNullOrWhiteSpace(student.Parents))
            {
                throw new Exception("Tên phụ huynh không được để trống.");
            }

            // Kiểm tra email phụ huynh
            if (student.ParentEmail != null && !IsValidEmail(student.ParentEmail))
            {
                throw new Exception("Email phụ huynh không hợp lệ.");
            }

            // Nếu tất cả kiểm tra đều hợp lệ, thêm học sinh vào cơ sở dữ liệu
            using (var db = new DatabaseConnection())
            {
                try
                {
                    db.AddStudent(student); // Thêm học sinh vào cơ sở dữ liệu
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi AdminAddStudent: " + ex.Message);
                }
            }
        }

        // Kiểm tra định dạng email hợp lệ
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            // Biểu thức chính quy để kiểm tra email
            var regex = new System.Text.RegularExpressions.Regex(
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            return regex.IsMatch(email);
        }


        // Kiểm tra tên hợp lệ: mỗi từ có chữ cái đầu viết hoa
        private bool IsValidName(string name)
        {
            return name.Split(' ').All(word => char.IsUpper(word[0]) && word.Skip(1).All(char.IsLower));
        }

        //Kiểm tra học sinh
        public bool AdminCheckStudent(int studentId)
        {
            var db = new DatabaseConnection();
            if (db.IsStudentIdExists(studentId))
            {
                return true;
            }
            return false;
        }
        //Xoa hoc sinh
        public void AdminDeleteStudent(int studentId)
        {
            try
            {
                var db = new DatabaseConnection();
                db.DeleteStudent(studentId);
            }
            catch (Exception ex)
            {

                throw new Exception("Lỗi AdminDeleteStudent: " + ex.Message);
            }

        }
        //lấy lớp
        public List<ClassStudent> AdminManagementGetClass()
        {
            List<ClassStudent> ClassStudents = new List<ClassStudent>();
            using (var db = new DatabaseConnection())
            {
                var ClassList = db.GetClass();
                ClassStudents.Clear();
                foreach (var _ in ClassList)
                {
                    ClassStudents.Add(_);
                }
            }
            return ClassStudents;
        }
    }
}
