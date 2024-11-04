using System;
using System.Windows.Forms;

namespace FaceDetectionApp
{
    static class Program
    {
        [STAThread]//cho biết luồng chính (main thread) 
        //của ứng dụng sử dụng Single-Threaded Apartment (STA) model, phù hợp với các ứng dụng Windows Forms 
        static void Main()
        {
            Application.EnableVisualStyles();// Bật tính năng Visual Styles cho phép ứng dụng sử dụng các theme giao diện của hệ điều hành
            Application.SetCompatibleTextRenderingDefault(false);//Tắt tính năng tương thích hiển thị văn bản mặc định
            Application.Run(new MainForm());
        }
    }
}