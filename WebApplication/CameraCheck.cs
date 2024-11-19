using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Camera.Services;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using System.Data.Common;
using System.Data.SqlClient;
using DTO;

namespace WebApplication
{

    public partial class CameraCheck : Form      
    {
        //Tạo danh sách học sinh toàn cục
        private List<studentRolClall> StudentsList = new List<studentRolClall>();
        public readonly AttendanceService _attendanceService;

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;

        public CameraCheck()
        {
            InitializeComponent();
            _attendanceService = new AttendanceService();

            //duyệt danh sách các thiết bị camera có trên máy và thêm các thiết bị vào ComboBox
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
            {
                cbCamera.Items.Add(filterInfo.Name);
            }

            cbCamera.SelectedIndex = 0; // Đặt mục đầu tiên làm lựa chọn mặc định trong ComboBox
            Notification.Visible = false; // Ẩn thông báo khi khởi động
            attentionTable.ReadOnly = true;

            LoadStudentList();
            ClassList_SelectedIndexChanged();
        }
        // Bắt đầu camera và Quét QR
        private void btStart_Click(object sender, EventArgs e)
        {
            // Nếu camera đang chạy, dừng camera trước khi khởi động lại
            if (videoCaptureDevice != null && videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice.Stop();
            }
            // Chọn thiết bị camera
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cbCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start(); // Bắt đầu truyền hình ảnh từ camera
            timer1.Start();// Bắt đầu chạy timer quét QR code.
        }
        // Hiển thị Camera
        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            ptbIMG.Image = bitmap; // Hiển thị Camera mới trong `PictureBox`
        }
        // dừng camera
        private void btStop_Click(object sender, EventArgs e)
        {
            ptbIMG.Image = null;
            if (videoCaptureDevice != null && videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice.Stop();
            }
            var SaveStatus = new AttendanceService();
            List< studentRolClall> SaveList = new List< studentRolClall>();
            for (int i = 0; i < StudentsList.Count; i++)
            {
                if (StudentsList[i].Class == ClassList.Text)
                {
                    SaveList.Add(StudentsList[i]);
                }
            }

            SaveStatus.SaveStatus(SaveList);
            //Gui thong bao hoc sin van
            for (int _ = 0; _ < StudentsList.Count; _++)
            {
                if (StudentsList[_].Class == ClassList.Text && !StudentsList[_].status)
                {
                    SaveStatus.Email(StudentsList[_].ID, StudentsList[_].NameStudent, StudentsList[_].Date);
                }
            }
            
        }
        // lựa chọn camera trong comboBox
        private void cbCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kiểm tra nếu camera hiện tại có đang chạy
            if (videoCaptureDevice != null && videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice.Stop();// Dừng camera hiện tại trước khi chuyển sang camera mới
            }
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cbCamera.SelectedIndex].MonikerString);
        }
        // quét QR Code cập nhật điểm danh
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ptbIMG.Image != null)
            {
                BarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode((Bitmap)ptbIMG.Image);

                var check = new AttendanceService();
                

                if (result != null)
                {
                    int studentId = Convert.ToInt32(result.Text);

                    if (!_attendanceService.ValidateStudentId(studentId))
                    {
                        Notification.Text = "Mã " + studentId + " của học sinh không hợp lệ!";
                        Notification.ForeColor = Color.Red;
                        Notification.Visible = true;
                        return;
                    }

                    //bool attendanceRecorded = _attendanceService.RecordAttendance(studentId);
                    
                    for (int i = 0; i < StudentsList.Count; i++)
                    {
                       
                        if(studentId == StudentsList[i].ID && ClassList.Text == StudentsList[i].Class) {
                            if (!StudentsList[i].status)
                            {
                                StudentsList[i].status = true;
                                Notification.Text = "Học sinh " + studentId + " điểm danh thành công!";
                                Notification.ForeColor = Color.Green;
                                LoadAttendanceData(); // Refresh attentionTable after successful attendance
                                                      // Lọc danh sách học sinh dựa trên lớp học được chọn
                                var selectedClass = ClassList.SelectedItem.ToString();

                                // Lọc danh sách theo lớp được chọn
                                var filteredList = StudentsList
                                    .Where(student => student.Class == selectedClass)
                                    .ToList();

                                attentionTable.DataSource = null;
                                for (int j = 0; j < filteredList.Count; j++)
                                {
                                    filteredList[j].STT = j + 1;
                                }
                                attentionTable.DataSource = filteredList;
                            }
                            else
                            {
                                Notification.Text = "Học sinh đã điểm danh!";
                                Notification.ForeColor = Color.Red;
                            }
                            Notification.Visible = true;
                           
                        }
                       
                       
                    }   
                }
            }
        }
        //lấy danh sách học sinh từ db
        private void LoadStudentList()
        {
            attentionTable.DataSource = null;
            attentionTable.Rows.Clear();//Xóa dòng Rows trong bản

            StudentsList.Clear();
            var list = new AttendanceService();
            StudentsList = list.GetDataStudent();

            LoadAttendanceData();
        }
        // cập nhật bảng
        private void LoadAttendanceData()
        {

            // Xóa dữ liệu hiện có trên DataGridView
            attentionTable.DataSource = null;
            // Thêm số thứ tự 
            for (int i = 0; i < StudentsList.Count; i++)
            {
                StudentsList[i].STT = i + 1;
            }
            // Gán danh sách học sinh vào DataSource của DataGridView
            attentionTable.DataSource = StudentsList;
            for (int i = 0; i < StudentsList.Count; i++)
            {
                StudentsList[i].Date = DateTime.Today;
            }
            // Cài đặt chế độ tự động điều chỉnh độ rộng cột
            attentionTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }
        // Kiểm tra camera có đang bật hay không nếu có thì tắt
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (videoCaptureDevice != null && videoCaptureDevice.IsRunning)
            {
                videoCaptureDevice.Stop();
            }
        }

        private void attentionTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Notification_Click(object sender, EventArgs e)
        {

        }

        private void ClassList_SelectedIndexChanged()
        {
            ClassList.Items.Clear();
            // Thêm các lớp học vào ComboBox
            ClassList.Items.Add("Chọn lớp điểm danh"); // Tùy chọn để hiển thị tất cả học sinh
            ClassList.Items.Add("Tất cả"); // Tùy chọn để hiển thị tất cả học sinh

            //Lấy tên lớp từ 
            var ad = new AttendanceService();
            // Lấy danh sách lớp từ hàm GetClass
            var classList = ad.GetClass();

            List<ClassStudent> ListClassTam = new List<ClassStudent>();
            // Thêm tên lớp vào ComboBox
            foreach (var classStudent in classList)
            {
                // Kiểm tra xem classStudent.Class có null không trước khi thêm vào ComboBox
                if (!string.IsNullOrEmpty(classStudent.Class))
                {
                    // Kiểm tra xem tên lớp đã tồn tại trong ListClassTam hay chưa
                    if (!ListClassTam.Any(cs => cs.Class == classStudent.Class))
                    {
                        ListClassTam.Add(classStudent);
                        ClassList.Items.Add(classStudent.Class);
                    }
                }
            }
            // Đăng ký sự kiện SelectedIndexChanged để xử lý khi người dùng chọn lớp
            ClassList.SelectedIndexChanged += new EventHandler(ClassList_Index);

            // Đặt mặc định hiển thị tất cả
            ClassList.SelectedIndex = 0;
        }
        //Thay đổi khi chọn lớp
        private void ClassList_Index(object sender, EventArgs e)
        {
            if (ClassList?.SelectedItem != null)
            {
                // Lọc danh sách học sinh dựa trên lớp học được chọn
                var selectedClass = ClassList.SelectedItem.ToString();

                if (selectedClass == "Tất cả")
                {
                    // Hiển thị toàn bộ danh sách
                    attentionTable.DataSource = StudentsList;
                }
                else
                {
                    // Lọc danh sách theo lớp được chọn
                    var filteredList = StudentsList
                        .Where(student => student.Class == selectedClass)
                        .ToList();

                    attentionTable.DataSource = null;
                    for (int i = 0; i < filteredList.Count; i++)
                    {
                        filteredList[i].STT = i + 1;
                    }
                    attentionTable.DataSource = filteredList;
                }
            }
        }

        private void openClass_Click(object sender, EventArgs e)
        {
           if(!attentionTable.Visible)
            {
                attentionTable.Visible = true;
            }
        }

        private void closeClass_Click(object sender, EventArgs e)
        {
            if (attentionTable.Visible)
            {
                attentionTable.Visible = false;
            }
        }
    }
}
