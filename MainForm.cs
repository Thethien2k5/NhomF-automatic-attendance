// MainForm.cs
using System;
using System.Drawing;
using System.IO;
using System.Linq;
//tạo giao diện ng dùng
using System.Windows.Forms;
//sửa lý ảnh
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using FaceDetectionApp.Models;

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

        public MainForm()
        {
            InitializeComponent();
            InitializeCamera();
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
                        if (!isNewStudentMode) // Tự động phát hiện chế độ Điểm danh
                        {
                            DetectAndMarkAttendance(imageFrame);
                        }
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

        //chế độ điểm danh
        private void RecognizeAndRecordAttendance(Image<Bgr, Byte> image, Rectangle faceRect)
        {
            Image<Bgr, Byte> faceImage = image.Copy(faceRect).Resize(100, 100, Inter.Linear);
            byte[] faceData = faceImage.ToJpegData();

            using (var db = new FaceDetectionAppContext())
            {
                var student = db.Students.FirstOrDefault(s => s.FaceData.SequenceEqual(faceData));
                if (student != null)
                {
                    var attendanceRecord = new AttendanceRecord
                    {
                        StudentId = student.Id,
                    };

                    db.AttendanceRecords.Add(attendanceRecord);
                    db.SaveChanges();

                    // Cập nhật hộp danh sách với tên học sinh và trạng thái tham dự
                    lstAttendance.Items.Add($"{student.Name} - Có mặt tại {attendanceRecord.AttendanceTime}");
                }
                else
                {
                    lstAttendance.Items.Add("Học sinh không được công nhận");
                }
            }
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
            // Cắt và resize ảnh khuôn mặt
            Image<Bgr, Byte> faceImage = image.Copy(faceRect).Resize(100, 100, Inter.Linear);
            // Chuyển đổi ảnh thành mảng byte
            byte[] faceData = faceImage.ToJpegData();

            // Nhận thông tin chi tiết về học sinh mới từ các trường nhập liệu
            string studentName = txtStudentName.Text;
            string studentClass = txtStudentClass.Text;

            //Kiểm tra tên và lớp có thông tin hay không
            if (string.IsNullOrWhiteSpace(studentName) || string.IsNullOrWhiteSpace(studentClass))
            {
                MessageBox.Show("Vui lòng nhập tên học sinh và lớp.");
                return;
            }

            //đảm bảo kết nối database được đóng lại sau khi dùng
            using (var db = new FaceDetectionAppContext())
            {
                //Tạo đt mới
                var newStudent = new Student
                {
                    Name = studentName,
                    Class = studentClass,
                    FaceData = faceData
                };

                //thêm đt vào database
                db.Students.Add(newStudent);
                db.SaveChanges();//lưu

                MessageBox.Show($"Sinh viên mới {studentName} đã đăng ký thành công.");
            }
        }
        //chuyển đổi chế độ đăng ký
        private void SwitchToNewStudentMode(object sender, EventArgs e)
        {
            isNewStudentMode = true;
            txtStudentName.Visible = true;
            txtStudentClass.Visible = true;
            btnCapture.Visible = true;
            lstAttendance.Visible = false;
            MessageBox.Show("chuyển sang chế độ Đăng ký học sinh mới.");
        }

        // Đã chuyển sang chế độ Điểm danh.
        private void SwitchToAttendanceMode(object sender, EventArgs e)
        {
            isNewStudentMode = false;
            txtStudentName.Visible = false;
            txtStudentClass.Visible = false;
            btnCapture.Visible = false;
            lstAttendance.Visible = true;
            lstAttendance.Items.Clear();
            MessageBox.Show("Đã chuyển sang chế độ Điểm danh.");
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
        private System.Windows.Forms.TextBox txtStudentName;
        private System.Windows.Forms.TextBox txtStudentClass;
        private System.Windows.Forms.Button btnNewStudentMode;
        private System.Windows.Forms.Button btnAttendanceMode;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.ListBox lstAttendance;

        //Tạo form giao diện
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.txtStudentName = new System.Windows.Forms.TextBox();
            this.txtStudentClass = new System.Windows.Forms.TextBox();
            this.btnNewStudentMode = new System.Windows.Forms.Button();
            this.btnAttendanceMode = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.lstAttendance = new System.Windows.Forms.ListBox();

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
            // txtStudentName : txtTên học sinh
            // 
            this.txtStudentName.Location = new System.Drawing.Point(490, 55);
            this.txtStudentName.Name = "txtStudentName";
            this.txtStudentName.Size = new System.Drawing.Size(200, 20);
            this.txtStudentName.PlaceholderText = "Tên học sinh mới";
            this.txtStudentName.Visible = false;

            // 
            // txtStudentClass : txtLớp học sinh
            // 
            this.txtStudentClass.Location = new System.Drawing.Point(490, 90);
            this.txtStudentClass.Name = "txtStudentClass";
            this.txtStudentClass.Size = new System.Drawing.Size(200, 20);
            this.txtStudentClass.PlaceholderText = "Lớp";
            this.txtStudentClass.Visible = false;

            // 
            // btnNewStudentMode : btnChế độ sinh viên mới
            // 
            this.btnNewStudentMode.Location = new System.Drawing.Point(450, 10);
            this.btnNewStudentMode.Name = "btnNewStudentMode";
            this.btnNewStudentMode.Size = new System.Drawing.Size(150, 40);
            this.btnNewStudentMode.Text = "Thêm sinh viên mới";
            //
            this.btnNewStudentMode.BackColor = Color.FromArgb(59, 218, 216);
            this.btnNewStudentMode.Click += new EventHandler(SwitchToNewStudentMode);
            // 
            // btnAttendanceMode : btnChế độ điểm danh
            // 
            this.btnAttendanceMode.Location = new System.Drawing.Point(600, 10);
            this.btnAttendanceMode.Name = "btnAttendanceMode";
            this.btnAttendanceMode.Size = new System.Drawing.Size(150, 40);
            this.btnAttendanceMode.Text = "Tự động điểm danh";
            //
            this.btnAttendanceMode.BackColor = Color.FromArgb(59, 218, 216);
            this.btnAttendanceMode.Click += new EventHandler(SwitchToAttendanceMode);

            // btnCapture : btnChụp
            this.btnCapture.Location = new System.Drawing.Point(490, 220);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(200, 30);
            this.btnCapture.Text = "Chụp";
            this.btnCapture.BackColor = Color.FromArgb(59, 218, 216);
            this.btnCapture.Visible = false;
            this.btnCapture.Click += new EventHandler(CaptureNewStudentFace);

            // lstAttendance : lstĐiểm danh
            this.lstAttendance.Location = new System.Drawing.Point(450, 50);
            this.lstAttendance.Name = "lstAttendance";
            this.lstAttendance.Size = new System.Drawing.Size(300, 400);
            this.lstAttendance.Visible = false;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.txtStudentName);
            this.Controls.Add(this.txtStudentClass);
            this.Controls.Add(this.btnNewStudentMode);
            this.Controls.Add(this.btnAttendanceMode);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.lstAttendance);
            this.Name = "MainForm";
            this.Text = "Nhận diện khuôn mặt - Tự động điểm danh";

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
