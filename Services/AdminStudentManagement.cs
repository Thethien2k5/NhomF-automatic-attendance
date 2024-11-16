using System;
using System.Collections.Generic;
using System.Linq;
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
        //Thêm học sinh
        public void AdminAddStudent(Student student)
        {
            using (var db = new DatabaseConnection())
            {
                try
                {
                    db.AddStudent(student);
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi AdminAddStudent: " + ex.Message);
                }
            }
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
