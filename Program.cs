using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

class FaceDetection
{
    static void Main(string[] args)
    {
        // Đường dẫn đến file cascade cho nhận diện khuôn mặt
        string faceCascadePath = "haarcascade_frontalface_default.xml";

        // Tạo đối tượng CascadeClassifier để sử dụng bộ nhận diện khuôn mặt
        CascadeClassifier faceCascade = new CascadeClassifier(faceCascadePath);

        // Khởi tạo capture từ webcam (camera index = 0)
        VideoCapture capture = new VideoCapture(0);

        if (!capture.IsOpened)
        {
            Console.WriteLine("Không thể mở webcam.");
            return;
        }

        Console.WriteLine("Nhấn ESC để thoát.");

        using (Mat frame = new Mat())
        {
            while (true)
            {
                // Lấy frame từ webcam
                capture.Read(frame);

                if (frame.IsEmpty)
                {
                    Console.WriteLine("Không thể nhận frame từ webcam.");
                    break;
                }

                // Chuyển đổi frame sang ảnh grayscale (ảnh xám)
                using (Mat grayFrame = new Mat())
                {
                    CvInvoke.CvtColor(frame, grayFrame, ColorConversion.Bgr2Gray);

                    // Tăng cường độ tương phản cho ảnh grayscale
                    CvInvoke.EqualizeHist(grayFrame, grayFrame);

                    // Phát hiện khuôn mặt
                    Rectangle[] facesDetected = faceCascade.DetectMultiScale(
                        grayFrame,
                        1.1,  // Scale factor
                        10,   // Min neighbors
                        new Size(20, 20), // Kích thước tối thiểu của khuôn mặt
                        Size.Empty         // Kích thước tối đa của khuôn mặt
                    );

                    // Vẽ hình chữ nhật xung quanh các khuôn mặt phát hiện được
                    foreach (Rectangle face in facesDetected)
                    {
                        CvInvoke.Rectangle(frame, face, new MCvScalar(0, 255, 0), 2);
                    }

                    // Hiển thị frame đã xử lý
                    CvInvoke.Imshow("Face Detection", frame);

                    // Nhấn phím ESC để thoát
                    if (CvInvoke.WaitKey(30) == 27)
                    {
                        break;
                    }
                }
            }
        }

        capture.Dispose();
        CvInvoke.DestroyAllWindows();
    }
}
