using System;
using System.Drawing;
using System.Windows.Forms;
using AdminManagement.Services;
using DTO;

namespace AdminManagement.WebApplication
{
    public partial class MainFormAdmin : Form
    {
        //Tạo danh sách học sinh toàn cục
        private List<Student> StudentsList = new List<Student>();
        public MainFormAdmin()
        {
            InitializeComponent();
        }
        // Cập nhật kích thước của bảng khi form thay đổi kích thước
        private void MainFormAdmin_Resize(object? sender, EventArgs e)
        {
            TableStudents.Width = (int)(ClientSize.Width * 0.9); // 80% chiều rộng
            TableStudents.Height = (int)(ClientSize.Height * 0.8); // 70% chiều cao

        }
        //Cập nhật thông tin theo thay đổi trên form
        // Xử lý sự kiện khi người dùng chỉnh sửa xong ô trong bảng
        private void TableStudents_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            // Tắt sự kiện tạm thời để tránh gọi lại sự kiện khi thay đổi ô
            TableStudents.CellEndEdit -= TableStudents_CellEndEdit;

            var adminService = new MainAdmin();
            var check = false;

            // Lấy dòng hiện tại mà người dùng đã chỉnh sửa
            var editedRow = TableStudents.Rows[e.RowIndex];
            // Kiểm tra xem dòng có khác null và giá trị ID có hợp lệ không
            if (editedRow != null && editedRow.Cells["ID"].Value != null)
            {
                // Lấy thông tin học sinh từ dòng đã chỉnh sửa
                int ID = Convert.ToInt32(editedRow.Cells["ID"].Value);
                string name = editedRow.Cells["NameStudent"].Value?.ToString() ?? string.Empty;
                string studentClass = editedRow.Cells["Class"].Value?.ToString() ?? string.Empty;
                string parents = editedRow.Cells["Parents"].Value?.ToString() ?? string.Empty;
                string ParentEmail = editedRow.Cells["ParentEmail"].Value?.ToString() ?? string.Empty;
                // Gọi phương thức để cập nhật cơ sở dữ liệu
                check = adminService.TableStudentsCellEndEdit(ID, name, studentClass, parents, ParentEmail);
            }
            if (check)
            {
                MessageBox.Show("Đã cập nhật thông tin");
            }
            else
            {
                MessageBox.Show("Chưa cập nhật thông tin");
            }
            LoadStudentList();
        }
        //lấy danh sách học sinh từ db
        private void LoadStudentList()
        {
            TableStudents.DataSource = null;
            TableStudents.Rows.Clear();//Xóa dòng Rows trong bản

            StudentsList.Clear();
            var list = new MainAdmin();
            StudentsList = list.GetLoadStudentList();

            LoadDataIntoTableStudents();
        }
        //In lên bản
        private void LoadDataIntoTableStudents()
        {
            // Thêm số thứ tự 
            for (int i = 0; i < StudentsList.Count; i++)
            {
                StudentsList[i].STT = i + 1;
            }
            // Xóa dữ liệu hiện có trên DataGridView
            TableStudents.DataSource = null;
            // Gán danh sách học sinh vào DataSource của DataGridView
            TableStudents.DataSource = StudentsList;
            // Cài đặt chế độ tự động điều chỉnh độ rộng cột
            TableStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        //thêm dữ liệu học sinh mới
        private void AddStudent(object? sender, EventArgs e)
        {
            var adminService = new MainAdmin();
            Student student = new Student();
            if (int.TryParse(IDStudentAdd.Text, out int id))
            {
                student.ID = id; // Gán giá trị vào student.ID nếu thành công
            }
            else
            {
                MessageBox.Show("ID không hợp lệ.");
                return;
            }
            student.NameStudent = NameStudentAdd.Text;
            student.Class = ClassStudentAdd.Text;
            student.Parents = ParentsStudentAdd.Text;
            student.ParentEmail = ParentEmailStudentAdd.Text;
            // Gọi phương thức AddStudent để thêm học sinh vào cơ sở dữ liệu
            try
            {
                adminService.AdminAddStudent(student);
                // Thêm học sinh vào cơ sở dữ liệu
                MessageBox.Show("Đã thêm thành công.");
                LoadStudentList(); // Load lại danh sách
                IDStudentAdd.Clear();
                NameStudentAdd.Clear();
                ClassStudentAdd.Clear();
                ParentsStudentAdd.Clear();
                ParentEmailStudentAdd.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message);  // Thông báo lỗi nếu có
            }
        }
        //Đưa dữ liệu đi để xóa hoc sinh khỏi danh sách
        private void RemoveStudentFromList(object? sender, EventArgs e)
        {
            var adminService = new MainAdmin();
            if (int.TryParse(Delete.Text, out int studentId))  // Chuyển chuỗi thành int và kiểm tra
            {
                if (adminService.AdminCheckStudent(studentId))
                {
                    adminService.AdminDeleteStudent(studentId);
                    Delete.Clear();
                    MessageBox.Show("Đã xóa.");
                    LoadStudentList();
                }
                else
                {
                    MessageBox.Show("ID học sinh không tồn tại.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập ID hợp lệ.");
            }

        }
        //Cập nhật thông tin
        private void StudentEdit_Click(object? sender, EventArgs e)
        {
            StudentAdd.Visible = false;
            StudentEdit.Visible = false;
            StudentDelete.Visible = false;
            //
            TableStudents.ReadOnly = false;
            TableStudents.Columns["ID"].ReadOnly = true;
            TableStudents.Columns["STT"].ReadOnly = true;
            //
            StudentEditText.Visible = true;
            confirm.Visible = true;

        }
        //
        //Quản lý học sinh
        private void StudentManagement_Click(object? sender, System.EventArgs e)
        {
            //
            StudentManagement.Visible = false;
            TableStudents.Visible = true;
            ClassList.Visible = true;
            StudentBackMenu.Visible = true;
            StudentDelete.Visible = true;
            StudentEdit.Visible = true;
            StudentAdd.Visible = true;
        }
        //Nut trở về 
        private void StudentMenu_Click(object? sender, EventArgs e)
        {


            //về menu
            if (StudentManagement.Visible == false &&
                (StudentBackMenu.Visible == true) &&
                (TableStudents.Visible == true) &&
                (ClassList.Visible == true) &&
                (StudentDelete.Visible == true) &&
                (StudentEdit.Visible == true) &&
                (StudentAdd.Visible == true)
            )
            {
                StudentManagement.Visible = true;
                //
                StudentBackMenu.Visible = false;
                TableStudents.Visible = false;
                ClassList.Visible = false;
                StudentDelete.Visible = false;
                StudentEdit.Visible = false;
                StudentAdd.Visible = false;
            }
            //về danh sách học sinh từ add
            if ((IDStudentAddText.Visible == true) &&
             (IDStudentAdd.Visible == true) &&
              (NameStudentAddText.Visible == true) &&
               (NameStudentAdd.Visible == true) &&
                (ClassStudentAddText.Visible == true) &&
                 (ClassStudentAdd.Visible == true) &&
                  (ParentEmailStudentAddText.Visible == true) &&
                   (ParentEmailStudentAdd.Visible == true) &&
                    (ParentsStudentAddText.Visible == true) &&
                     (ParentsStudentAdd.Visible == true) &&
                     add.Visible == true)
            {
                //nhập thông tin học sinh
                IDStudentAddText.Visible = false;
                IDStudentAdd.Visible = false;
                NameStudentAddText.Visible = false;
                NameStudentAdd.Visible = false;
                ClassStudentAddText.Visible = false;
                ClassStudentAdd.Visible = false;
                ParentEmailStudentAddText.Visible = false;
                ParentEmailStudentAdd.Visible = false;
                ParentsStudentAddText.Visible = false;
                ParentsStudentAdd.Visible = false;
                add.Visible = false;

                StudentManagement.Visible = false;
                //
                TableStudents.Visible = true;
                ClassList.Visible = true;
                StudentAdd.Visible = true;
                StudentDelete.Visible = true;
                StudentEdit.Visible = true;
                StudentBackMenu.Visible = true;
            }
            //về danh sách từ delete
            if ((DeleteStudentText.Visible == true) &&
             (Delete.Visible == true) &&
              (DeleteButton.Visible == true))
            {
                DeleteStudentText.Visible = false;
                Delete.Visible = false;
                DeleteButton.Visible = false;
                //
                StudentDelete.Visible = true;
                StudentEdit.Visible = true;
                StudentAdd.Visible = true;
            }
            //về từ edit
            if (StudentEditText.Visible == true && confirm.Visible == true)
            {
                StudentAdd.Visible = true;
                StudentEdit.Visible = true;
                StudentDelete.Visible = true;
                //
                TableStudents.ReadOnly = true;
                //
                StudentEditText.Visible = false;
                confirm.Visible = false;
            }
        }
        //Nút Thêm
        private void StudentAdd_Click(object? sender, EventArgs e)
        {
            StudentAdd.Visible = false;
            //form nhập
            IDStudentAddText.Visible = true;
            IDStudentAdd.Visible = true;
            NameStudentAddText.Visible = true;
            NameStudentAdd.Visible = true;
            ClassStudentAddText.Visible = true;
            ClassStudentAdd.Visible = true;
            ParentEmailStudentAddText.Visible = true;
            ParentEmailStudentAdd.Visible = true;
            ParentsStudentAddText.Visible = true;
            ParentsStudentAdd.Visible = true;
            add.Visible = true;
            //
            TableStudents.Visible = false;
            ClassList.Visible = false;
            StudentDelete.Visible = false;
            StudentEdit.Visible = false;
        }
        //Xóa học sinh
        private void DeleteStudent(object? sender, EventArgs e)
        {
            DeleteStudentText.Visible = true;
            Delete.Visible = true;
            DeleteButton.Visible = true;
            //
            StudentDelete.Visible = false;
            StudentEdit.Visible = false;
            StudentAdd.Visible = false;
            //
        }
        //
        private DataGridView TableStudents = new DataGridView();
        private ComboBox ClassList = new ComboBox();
        private Button StudentManagement = new Button();
        private Button StudentBackMenu = new Button();
        //
        //Thêm
        private Button StudentAdd = new Button();
        private Label IDStudentAddText = new Label();
        private TextBox IDStudentAdd = new TextBox();
        private Label NameStudentAddText = new Label();
        private TextBox NameStudentAdd = new TextBox();
        private Label ClassStudentAddText = new Label();
        private TextBox ClassStudentAdd = new TextBox();
        private Label ParentEmailStudentAddText = new Label();
        private TextBox ParentEmailStudentAdd = new TextBox();
        private Label ParentsStudentAddText = new Label();
        private TextBox ParentsStudentAdd = new TextBox();
        private Button add = new Button();
        //
        private Button StudentDelete = new Button();
        private Label DeleteStudentText = new Label();
        private TextBox Delete = new TextBox();
        private Button DeleteButton = new Button();
        //
        private Button StudentEdit = new Button();
        private Label StudentEditText = new Label();
        private Button confirm = new Button();


        private void InitializeComponent()
        {
            // Cài đặt các thuộc tính cho TableStudents
            TableStudents.Location = new Point(70, 100);
            TableStudents.Name = "TableStudents";
            TableStudents.Height = (int)(ClientSize.Height * 0.7); // 70% chiều cao
            TableStudents.Width = (int)(ClientSize.Width * 0.8); // 80% chiều rộng
            TableStudents.ReadOnly = true;//Chỉ đọc k cho phép chỉnh sửa
            TableStudents.Visible = false;
            //
            // Đăng ký sự kiện trong phương thức InitializeComponent hoặc trong constructor
            TableStudents.CellEndEdit += TableStudents_CellEndEdit;
            //
            //
            //Lọc học sinh theo lớp
            ClassList.Location = new Point(70, 70);
            ClassList.Name = "ClassList";
            ClassList.Width = (int)(ClientSize.Width * 0.7);
            ClassList.Visible = false;
            //Nút bật bản quản lí học sinh
            StudentManagement.Location = new Point(200, 50);
            StudentManagement.Name = "StudentManagement";
            StudentManagement.Size = new Size(400, 100);
            StudentManagement.Text = "Quản lý học sinh";
            StudentManagement.Font = new Font(StudentManagement.Font.FontFamily, 24);
            StudentManagement.Click += new EventHandler(StudentManagement_Click);
            StudentManagement.Visible = true;
            //Nút backMenu
            StudentBackMenu.Location = new Point(0, 25);
            StudentBackMenu.Name = "StudentBackMenu";
            StudentBackMenu.Size = new Size(70, 40);
            StudentBackMenu.Text = "Trở lại";
            StudentBackMenu.Click += new EventHandler(StudentMenu_Click);
            StudentBackMenu.Visible = false;
            //Nút thêm học sinh
            StudentAdd.Location = new Point(400, 10);
            StudentAdd.Name = "StudentAdd";
            StudentAdd.Size = new Size(70, 80);
            StudentAdd.Text = "Thêm";
            StudentAdd.Click += new EventHandler(StudentAdd_Click);
            StudentAdd.Visible = false;
            //Thêm Id học sinh
            IDStudentAddText.Location = new Point(300, 75);
            IDStudentAddText.Text = "ID";
            IDStudentAddText.Visible = false;
            IDStudentAdd.Location = new Point(300, 100);
            IDStudentAdd.Name = "IDStudent";
            IDStudentAdd.Size = new Size(200, 100);
            IDStudentAdd.Visible = false;
            //Thêm Name học sinh
            NameStudentAddText.Location = new Point(300, 175);
            NameStudentAddText.Text = "Họ và Tên";
            NameStudentAddText.Visible = false;
            NameStudentAdd.Location = new Point(300, 200);
            NameStudentAdd.Name = "Học và Tên";
            NameStudentAdd.Size = new Size(200, 100);
            NameStudentAdd.Visible = false;
            //Thêm Class học sinh
            ClassStudentAddText.Location = new Point(300, 275);
            ClassStudentAddText.Text = "Lớp";
            ClassStudentAddText.Visible = false;
            ClassStudentAdd.Location = new Point(300, 300);
            ClassStudentAdd.Name = "ClassStudent";
            ClassStudentAdd.Size = new Size(200, 100);
            ClassStudentAdd.Visible = false;
            //Thêm phụ huynh học sinh
            ParentsStudentAddText.Location = new Point(300, 375);
            ParentsStudentAddText.Text = "Phụ Huynh";
            ParentsStudentAddText.Visible = false;
            ParentsStudentAdd.Location = new Point(300, 400);
            ParentsStudentAdd.Name = "ParentsStudent";
            ParentsStudentAdd.Size = new Size(200, 100);
            ParentsStudentAdd.Visible = false;
            //Thêm Địa chỉ học sinh
            ParentEmailStudentAddText.Location = new Point(300, 475);
            ParentEmailStudentAddText.Text = "Email của phụ huynh";
            ParentEmailStudentAddText.Size = new Size(200, 20);
            ParentEmailStudentAddText.Visible = false;
            ParentEmailStudentAdd.Location = new Point(300, 500);
            ParentEmailStudentAdd.Name = "ParentEmailStudent";
            ParentEmailStudentAdd.Size = new Size(200, 10);
            ParentEmailStudentAdd.Visible = false;
            //nút add
            add.Location = new Point(350, 550);
            add.Name = "Add";
            add.Size = new Size(100, 30);
            add.Text = "Thêm học sinh";
            add.Click += new EventHandler(AddStudent);
            add.Visible = false;
            //
            //
            //Xóa học sinh
            //NNút xóa học sinh
            StudentDelete.Location = new Point(500, 10);
            StudentDelete.Name = "StudentDelete";
            StudentDelete.Size = new Size(70, 80);
            StudentDelete.Text = "Xóa";
            StudentDelete.Click += new EventHandler(DeleteStudent);
            StudentDelete.Visible = false;
            //Nút xóa
            DeleteButton.Location = new Point(720, 50);
            DeleteButton.Size = new Size(100, 30);
            DeleteButton.Text = "Xóa";
            DeleteButton.Click += new EventHandler(RemoveStudentFromList);
            DeleteButton.Visible = false;
            //Id học sinh cần xóa 
            DeleteStudentText.Location = new Point(500, 30);
            DeleteStudentText.Text = "Nhập ID học sinh cần xóa";
            DeleteStudentText.Visible = false;
            Delete.Location = new Point(500, 55);
            Delete.Size = new Size(200, 100);
            Delete.Visible = false;
            //
            //sửa thông tin
            //Nút sửa
            StudentEdit.Location = new Point(600, 10);
            StudentEdit.Name = "StudentEdit";
            StudentEdit.Size = new Size(90, 80);
            StudentEdit.Text = "Sửa thông tin";
            StudentEdit.Click += new EventHandler(StudentEdit_Click);
            StudentEdit.Visible = false;
            //dòng thông báo cho phép chỉnh sửa
            StudentEditText.Location = new Point(375, 50);
            StudentEditText.Name = "StudentEditText";
            StudentEditText.Size = new Size(200, 40);
            StudentEditText.Text = "Chế độ chỉnh sửa \n(Chú ý: không thể sửa STT và ID)";
            StudentEditText.Visible = false;
            confirm.Location = new Point(575, 50);
            confirm.Name = "confirm";
            confirm.Size = new Size(90, 30);
            confirm.Text = "Xác nhận";
            confirm.Visible = false;
            // Cài đặt form chính
            ClientSize = new Size(800, 600);
            WindowState = FormWindowState.Maximized;
            ActiveControl = null;
            //
            Controls.Add(TableStudents);
            Controls.Add(ClassList);
            Controls.Add(StudentManagement);
            Controls.Add(StudentBackMenu);
            Controls.Add(StudentAdd);
            Controls.Add(StudentDelete);
            Controls.Add(StudentEdit);
            //
            Controls.Add(IDStudentAddText);
            Controls.Add(IDStudentAdd);
            Controls.Add(NameStudentAddText);
            Controls.Add(NameStudentAdd);
            Controls.Add(ClassStudentAddText);
            Controls.Add(ClassStudentAdd);
            Controls.Add(ParentEmailStudentAddText);
            Controls.Add(ParentEmailStudentAdd);
            Controls.Add(ParentsStudentAddText);
            Controls.Add(ParentsStudentAdd);
            Controls.Add(add);
            //
            Controls.Add(DeleteButton);
            Controls.Add(DeleteStudentText);
            Controls.Add(Delete);
            //
            Controls.Add(StudentEditText);
            Controls.Add(confirm);
            //
            // Thêm sự kiện Resize để tự động điều chỉnh kích thước bảng khi thay đổi kích thước form
            Resize += new EventHandler(MainFormAdmin_Resize);
            //
            Text = "Quản Lý Học Sinh";
            Name = "MainFormAdmin";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
