using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Camera.Repository;
using ZXing;
using DTO;

namespace Camera.Services
{
    public class AttendanceService
    {
        private readonly DatabaseAccess _databaseAccess;

        public AttendanceService()
        {
            _databaseAccess = new DatabaseAccess();
        }

        // Kiểm tra mã học sinh hợp lệ
        public bool ValidateStudentId(int studentId)
        {
            var db = new DatabaseAccess();
            return db.CheckValidStudentIds(studentId);
        }

        // Ghi nhận điểm danh cho học sinh
        public bool RecordAttendance(string studentId)
        {
            return _databaseAccess.MarkAttendance(studentId);
        }

        // Lấy dữ liệu điểm danh
        public List<studentRolClall> GetDataStudent()
        {
            return _databaseAccess.GetAttendanceData();
        }
        // Lấy dữ liệu lớp
        public List<ClassStudent> GetClass()
        {
            return _databaseAccess.GetDataClass();
        }
        public void SaveStatus(List<studentRolClall> save)
        {
            var cmd = new DatabaseAccess();
            cmd.AddStudentStatus(save);
        }
      
        //Thong bao hoc sinh van
        public void Email(int ID, string Name, DateTime date)
        {  

                    var cmd = new DatabaseAccess();
                    string to = cmd.GetEmail(ID);
                    // Địa chỉ email người nhận

                    // Gán trực tiếp thông tin email vào biến
                    string from = "huytg7989@ut.edu.vn";   // Địa chỉ email người gửi
                    string pass = "Cuongngu@2005";          // Mật khẩu ứng dụng email (App Password)
                    string subject = "Thông báo từ hệ thống tự động điểm danh"; // Tiêu đề email
                    string content = $@"<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}
        h1 {{
            color: #4CAF50;
            font-size: 24px;
            text-align: center;
        }}
        p {{
            font-size: 16px;
            color: #333;
        }}
        .highlight {{
            font-weight: bold;
            color: #FF5722;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 14px;
            color: #666;
            text-align: center;
        }}
    </style>
</head>
<body>
    <h1>Thông Báo Vắng Mặt</h1>
    <p>Kính gửi Quý phụ huynh,</p>
    <p>Chúng tôi xin thông báo rằng học sinh <span class='highlight'>{Name}</span> đã 
    vắng mặt trong buổi điểm danh ngày <span class='highlight'>{date}</span>.</p>
    <p>Việc điểm danh đóng vai trò quan trọng trong việc theo dõi tình hình học tập của học sinh, 
    vì vậy chúng tôi mong muốn nhận được sự phối hợp từ phía Quý phụ huynh.</p>
    <p>Nếu có bất kỳ lý do chính đáng nào cho việc vắng mặt này, xin vui lòng thông báo lại với giáo viên chủ nhiệm 
    hoặc liên hệ trực tiếp với nhà trường để xác minh thông tin.</p>
    <div class='footer'>
        <p>Trân trọng cảm ơn,<br>Ban Giám Hiệu Trường</p>
    </div>
</body>
</html>";

                    // Tạo một đối tượng MailMessage để thiết lập thông tin email
                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(from, "Người gửi Hệ thống tự động điểm danh"), // Thêm tên hiển thị
                        Subject = subject,
                        Body = content,
                        IsBodyHtml = true // Kích hoạt định dạng HTML
                    };
                    mail.To.Add(to);

                    // Cấu hình SMTP để gửi email qua máy chủ Gmail
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                    {
                        EnableSsl = true, // Bật bảo mật SSL/TLS
                        Port = 587, // Cổng SMTP
                        DeliveryMethod = SmtpDeliveryMethod.Network, // Gửi qua mạng internet
                        Credentials = new NetworkCredential(from, pass)
                    };

                    try
                    {
                        // Thực hiện gửi email
                        smtp.Send(mail);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Hiển thị lỗi nếu có
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }


        }

