using AdminManagement.WebApplication;

namespace AdminManagement
{
    static class Program
    {
        [STAThread]//cho biết luồng chính (main thread) 
        //của ứng dụng sử dụng Single-Threaded Apartment (STA) model, phù hợp với các ứng dụng Windows Forms 
        static void Main()
        {
            Application.EnableVisualStyles();// Bật tính năng Visual Styles cho phép ứng dụng sử dụng các theme giao diện của hệ điều hành
            Application.SetCompatibleTextRenderingDefault(false);//Tắt tính năng tương thích hiển thị văn bản mặc định

            try
            {
                Application.Run(new MainFormAdmin());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ứng dụng: " + ex.Message);
            }
        }
    }
}