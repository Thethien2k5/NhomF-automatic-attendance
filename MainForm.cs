// MainForm.cs
// using System;
// using System.Drawing;
// using System.IO;
// using System.Linq;
//tạo giao diện ng dùng
// using System.Windows.Forms;
//sửa lý ảnh
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using FaceDetectionApp.Models;
//
using AForge.Video.DirectShow;
using System.Timers;



namespace FaceDetectionApp
{
    public partial class MainForm : Form
    {
        //khởi tạo đt, chụp và từ nguồn cam
        private VideoCapture _capture = null;
        //tạo đối tượng phát hiện khuôn mặt
        private CascadeClassifier _faceCascade;
        //biến theo dõi chế độ
        private bool isNewStudentMode = true;


        private System.Timers.Timer attendanceTimer;
        private bool isAttendanceActive = false;

        FilterInfoCollection filterInfoCollection;


        public MainForm()
        {
            InitializeCamera();//Tạo camera
            InitializeComponent();//Tạo giao diện
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                cbCamera.Items.Add(filterInfo.Name);
            }
            cbCamera.SelectedIndex = 0;
        }

        //Tạo cam và phát hiện khuôn mặt
        private void InitializeCamera()
        {
            try
            {
                _capture = new VideoCapture(0);//đối tượng tương tác với cam
                _faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");//đối tượng phát hiện khuôn mặt theo mô hình
                Application.Idle += ProcessFrame;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi không thể khởi tạo camera: " + ex.Message);
            }
        }
        //Xử lý khung hình từ cam
        private void ProcessFrame(object sender, EventArgs e)
        {
            try
            {//Tạo đt frame để lưu trữ hình từ cam tamj thời
                using (Mat frame = _capture.QueryFrame())
                {
                    //Nếu đt frame có lưu trữ ảnh
                    if (frame != null)
                    {
                        Image<Bgr, Byte> imageFrame = frame.ToImage<Bgr, Byte>();
                        DetectFaces(imageFrame); // Sửa tên hàm để phản ánh rõ chức năng
                        pictureBox.Image = imageFrame.ToBitmap();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khung hình: " + ex.Message);
                Application.Idle -= ProcessFrame; // Dừng xử lý tiếp nếu máy ảnh bị lỗi
            }

        }
        //Vẻ khung xung quanh khuôn mặt    
        private void DetectAndMarkAttendance(Image<Bgr, Byte> image)
        {
            using (Image<Gray, Byte> grayFrame = image.Convert<Gray, Byte>())
            {
                Rectangle[] faces = _faceCascade.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty);
                foreach (var face in faces)
                {
                    image.Draw(face, new Bgr(Color.Green), 2); // Vẽ một hình chữ nhật màu xanh lá cây xung quanh khuôn mặt được phát hiện
                    RecognizeAndRecordAttendance(image, face);
                }
            }
        }

        //chuyển đổi chế độ đăng ký
        private void SwitchToNewStudentMode(object sender, EventArgs e)
        {
            isNewStudentMode = true;
            txtStudentName.Visible = true;
            txtStudentClass.Visible = true;
            btnCapture.Visible = true;
            //
            dgvAttendance.Visible = false;
            btnStartAttendance.Visible = false;
            btnEndAttendance.Visible = false;
            MessageBox.Show("chuyển sang chế độ Đăng ký học sinh mới.");
        }
        //Chế độ đăng ký
        private void CaptureNewStudentFace(object sender, EventArgs e)
        {
            if (!isNewStudentMode) return; // Chỉ cho phép chụp ở chế độ đăng ký

            using (Mat frame = _capture.QueryFrame())
            {
                if (frame != null)
                {
                    Image<Bgr, Byte> imageFrame = frame.ToImage<Bgr, Byte>();
                    using (Image<Gray, Byte> grayFrame = imageFrame.Convert<Gray, Byte>())
                    {
                        Rectangle[] faces = _faceCascade.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty);

                        if (faces.Length == 0)
                        {
                            MessageBox.Show("Không phát hiện được khuôn mặt. Vui lòng thử lại.");
                            return;
                        }

                        var face = faces[0]; // Giả sử chỉ có một khuôn mặt để đăng ký
                        RegisterNewStudent(imageFrame, face);
                    }
                }
            }
        }
        //Đăng lý học sinh mới
        private void RegisterNewStudent(Image<Bgr, Byte> image, Rectangle faceRect)
        {
            Image<Bgr, Byte> faceImage = image.Copy(faceRect).Resize(100, 100, Inter.Linear);
            byte[] faceData = faceImage.ToJpegData();

            string studentName = txtStudentName.Text;
            string studentClass = txtStudentClass.Text;

            if (string.IsNullOrWhiteSpace(studentName) || string.IsNullOrWhiteSpace(studentClass))
            {
                MessageBox.Show("Vui lòng nhập tên học sinh và lớp.");
                return;
            }

            using (var db = new FaceDetectionAppContext())
            {
                var newStudent = new Student
                {
                    Name = studentName,
                    Class = studentClass,
                    FaceData = faceData
                };

                db.AddStudent(newStudent);
                MessageBox.Show($"Sinh viên mới {studentName} đã đăng ký thành công.");
            }
        }



        // Đã chuyển sang chế độ Điểm danh.
        private void SwitchToAttendanceMode(object sender, EventArgs e)
        {
            isNewStudentMode = false;
            txtStudentName.Visible = false;
            txtStudentClass.Visible = false;
            btnCapture.Visible = false;
            //
            dgvAttendance.Visible = true;
            btnStartAttendance.Visible = true;
            btnEndAttendance.Visible = true;
            // dgvAttendance.Items.Clear();
            MessageBox.Show("Đã chuyển sang chế độ Điểm danh.");
        }
        //chế độ điểm danh
        private void RecognizeAndRecordAttendance(Image<Bgr, Byte> image, Rectangle faceRect)
        {
            Image<Bgr, Byte> faceImage = image.Copy(faceRect).Resize(100, 100, Inter.Linear);
            byte[] faceData = faceImage.ToJpegData();

            using (var db = new FaceDetectionAppContext())
            {
                var students = db.GetStudents();

                // Tìm sinh viên có dữ liệu khuôn mặt trùng khớp
                var student = students.FirstOrDefault(s => s.FaceData.SequenceEqual(faceData));

                // if (student != null)
                // {
                //     var attendanceRecord = new AttendanceRecord
                //     {
                //         StudentId = student.Id,
                //         AttendanceTime = DateTime.Now,
                //         Status = true // Đánh dấu là có mặt
                //     };

                //     db.AddAttendanceRecord(attendanceRecord);
                //     lstAttendance.Items.Add($"{student.Name} - Có mặt tại {attendanceRecord.AttendanceTime}");
                // }
                // else
                // {
                //     lstAttendance.Items.Add("Học sinh không được công nhận");
                // }
            }
        }
        // Phát hiện và vẽ khung màu đỏ quanh khuôn mặt, chuyển màu xanh lá khi nhận diện thành công
        private void DetectFaces(Image<Bgr, Byte> image)
        {
            using (Image<Gray, Byte> grayFrame = image.Convert<Gray, Byte>())
            {
                Rectangle[] faces = _faceCascade.DetectMultiScale(grayFrame, 1.1, 10, Size.Empty);
                foreach (var face in faces)
                {
                    // Mặc định vẽ khung đỏ xung quanh tất cả khuôn mặt được phát hiện
                    image.Draw(face, new Bgr(Color.Red), 2);

                    // Chỉ nhận diện khi đang ở chế độ điểm danh
                    // if (!isNewStudentMode)
                    // {
                    //     if (RecognizeFace(image, face))
                    //     {
                    //         image.Draw(face, new Bgr(Color.Green), 2); // Đổi sang khung xanh lá nếu nhận diện thành công
                    //     }
                    // }
                }
            }
        }
        //Đối chiếu kết quả với database
        // private bool RecognizeFace(Image<Bgr, Byte> image, Rectangle faceRect)
        // {
        //     // Resize ảnh khuôn mặt để so sánh với dữ liệu trong database
        //     Image<Bgr, Byte> faceImage = image.Copy(faceRect).Resize(100, 100, Inter.Linear);
        //     byte[] faceData = faceImage.ToJpegData();

        //     using (var db = new FaceDetectionAppContext())
        //     {
        //         var students = db.GetStudents();

        //         // Tìm sinh viên có dữ liệu khuôn mặt trùng khớp
        //         var student = students.FirstOrDefault(s => s.FaceData.SequenceEqual(faceData));

        //         if (student != null)
        //         {
        //             var attendanceRecord = new AttendanceRecord
        //             {
        //                 StudentId = student.Id,
        //                 AttendanceTime = DateTime.Now,
        //                 Status = true // Đánh dấu là có mặt
        //             };

        //             db.AddAttendanceRecord(attendanceRecord);
        //             lstAttendance.Items.Add($"{student.Name} - Có mặt tại {attendanceRecord.AttendanceTime}");
        //             return true;
        //         }
        //         else
        //         {
        //             lstAttendance.Items.Add("Học sinh không được công nhận");
        //         }
        //     }
        //     return false;
        // }

        private void StartAttendance(object sender, EventArgs e)
        {
            isAttendanceActive = true;
            LoadStudentsToAttendanceGrid();
            attendanceTimer.Start();
        }

        private void EndAttendance(object sender, EventArgs e)
        {
            isAttendanceActive = false;
            attendanceTimer.Stop();
            MarkUnattendedStudents();
        }

        private void LoadStudentsToAttendanceGrid()
        {
            dgvAttendance.Rows.Clear();

            using (var db = new FaceDetectionAppContext())
            {
                var students = db.GetStudents();
                foreach (var student in students)
                {
                    dgvAttendance.Rows.Add(student.Name, "X"); // Mặc định là "X" (chưa điểm danh)
                }
            }
        }

        private void MarkUnattendedStudents()
        {
            foreach (DataGridViewRow row in dgvAttendance.Rows)
            {
                if (row.Cells["AttendanceStatus"].Value.ToString() != "✔")
                {
                    row.Cells["AttendanceStatus"].Value = "X"; // Đánh dấu X cho những học sinh chưa điểm danh
                }
            }
        }



        //Cho phép chọn loại camera
        private void cbCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                _capture.Stop();
            }
            _capture = new VideoCapture(filterInfoCollection[cbCamera.SelectedIndex].MonikerString);
        }

        //  tự động gọi khi form được đóng
        protected override void OnClosed(EventArgs e)
        {
            _capture?.Dispose();
            base.OnClosed(e);
        }
    }

    partial class MainForm
    {
        //Tạo biến
        private System.Windows.Forms.PictureBox pictureBox;
        //
        private System.Windows.Forms.TextBox txtStudentName;
        private System.Windows.Forms.TextBox txtStudentClass;
        private System.Windows.Forms.Button btnNewStudentMode;
        //
        private System.Windows.Forms.Button btnAttendanceMode;
        private System.Windows.Forms.Button btnCapture;
        // private System.Windows.Forms.ListBox lstAttendance;

        private DataGridView dgvAttendance;
        private Button btnStartAttendance;
        private Button btnEndAttendance;

        //
        private System.Windows.Forms.ComboBox cbCamera;


        //Tạo form giao diện
        private void InitializeComponent()
        {
            //Hộp Ảnh
            this.pictureBox = new System.Windows.Forms.PictureBox();
            //Chế độ thêm học sinh mới
            this.txtStudentName = new System.Windows.Forms.TextBox();
            this.txtStudentClass = new System.Windows.Forms.TextBox();
            this.btnNewStudentMode = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            //Chế độ điểm danh
            this.btnAttendanceMode = new System.Windows.Forms.Button();
            // this.lstAttendance = new System.Windows.Forms.ListBox();
            this.dgvAttendance = new System.Windows.Forms.DataGridView();
            this.btnStartAttendance = new System.Windows.Forms.Button();
            this.btnEndAttendance = new System.Windows.Forms.Button();
            //Tuy chọn camerra
            this.cbCamera = new System.Windows.Forms.ComboBox();


            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();

            // 
            // Hộp hình ảnh
            // 
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(400, 400);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // btnNewStudentMode : btnChế độ sinh viên mới
            // 
            this.btnNewStudentMode.Location = new System.Drawing.Point(450, 10);
            this.btnNewStudentMode.Name = "btnNewStudentMode";
            this.btnNewStudentMode.Size = new System.Drawing.Size(150, 40);
            this.btnNewStudentMode.Text = "Thêm sinh viên mới";
            this.btnNewStudentMode.BackColor = Color.FromArgb(59, 218, 216);
            this.btnNewStudentMode.Click += new EventHandler(SwitchToNewStudentMode);
            // txtStudentName : txtTên học sinh
            // 
            this.txtStudentName.Location = new System.Drawing.Point(490, 55);
            this.txtStudentName.Name = "txtStudentName";
            this.txtStudentName.Size = new System.Drawing.Size(200, 20);
            this.txtStudentName.PlaceholderText = "Tên học sinh mới";
            this.txtStudentName.Visible = false;
            // txtStudentClass : txtLớp học sinh
            // 
            this.txtStudentClass.Location = new System.Drawing.Point(490, 90);
            this.txtStudentClass.Name = "txtStudentClass";
            this.txtStudentClass.Size = new System.Drawing.Size(200, 20);
            this.txtStudentClass.PlaceholderText = "Lớp";
            this.txtStudentClass.Visible = false;
            // btnCapture : btnChụp
            //
            this.btnCapture.Location = new System.Drawing.Point(490, 220);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(200, 30);
            this.btnCapture.Text = "Chụp";
            this.btnCapture.BackColor = Color.FromArgb(59, 218, 216);
            this.btnCapture.Visible = false;
            this.btnCapture.Click += new EventHandler(CaptureNewStudentFace);

            // 
            // btnAttendanceMode : btnChế độ điểm danh
            // 
            this.btnAttendanceMode.Location = new System.Drawing.Point(600, 10);
            this.btnAttendanceMode.Name = "btnAttendanceMode";
            this.btnAttendanceMode.Size = new System.Drawing.Size(150, 40);
            this.btnAttendanceMode.Text = "Tự động điểm danh";
            this.btnAttendanceMode.BackColor = Color.FromArgb(59, 218, 216);
            this.btnAttendanceMode.Click += new EventHandler(SwitchToAttendanceMode);

            // lstAttendance : lstĐiểm danh Bảng
            // this.lstAttendance.Location = new System.Drawing.Point(450, 50);
            // this.lstAttendance.Name = "lstAttendance";
            // this.lstAttendance.Size = new System.Drawing.Size(300, 400);
            // this.lstAttendance.Visible = false;
            // Tạo bảng danh sách điểm danh
            this.dgvAttendance = new DataGridView();
            this.dgvAttendance.Location = new System.Drawing.Point(450, 50);
            this.dgvAttendance.Size = new System.Drawing.Size(300, 400);
            this.dgvAttendance.Columns.Add("StudentName", "Tên học sinh");
            this.dgvAttendance.Columns.Add("AttendanceStatus", "Trạng thái");
            this.dgvAttendance.Visible = false;
            // Tạo nút bắt đầu điểm danh
            this.btnStartAttendance = new Button();
            this.btnStartAttendance.Location = new System.Drawing.Point(450, 500);
            this.btnStartAttendance.Size = new System.Drawing.Size(150, 40);
            this.btnStartAttendance.Text = "Bắt đầu điểm danh";
            this.btnStartAttendance.Visible = false;
            this.btnStartAttendance.Click += new EventHandler(StartAttendance);

            // Tạo nút kết thúc điểm danh
            this.btnEndAttendance = new Button();
            this.btnEndAttendance.Location = new System.Drawing.Point(600, 500);
            this.btnEndAttendance.Size = new System.Drawing.Size(150, 40);
            this.btnEndAttendance.Text = "Kết thúc điểm danh";
            this.btnEndAttendance.Visible = false;
            this.btnEndAttendance.Click += new EventHandler(EndAttendance);


            //tuy chọn camera
            this.cbCamera.Location = new System.Drawing.Point(90, 430);
            this.cbCamera.Name = "cbCamera";
            this.cbCamera.Size = new System.Drawing.Size(238, 24);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.pictureBox);
            //
            this.Controls.Add(this.txtStudentName);
            this.Controls.Add(this.txtStudentClass);
            this.Controls.Add(this.btnNewStudentMode);
            //
            this.Controls.Add(this.btnAttendanceMode);
            this.Controls.Add(this.btnCapture);
            // this.Controls.Add(this.lstAttendance);
            this.Controls.Add(dgvAttendance);
            this.Controls.Add(btnStartAttendance);
            this.Controls.Add(btnEndAttendance);
            //
            this.Controls.Add(this.cbCamera);
            this.Name = "MainForm";
            this.Text = "Nhận diện khuôn mặt - Tự động điểm danh";

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
