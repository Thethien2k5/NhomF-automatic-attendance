// FaceDetectionHelper.cs
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

public static class FaceDetectionHelper
{
    // Hàm phát hiện khuôn mặt
    public static Rectangle DetectFace(Image<Bgr, Byte> image)
    {
        // Đường dẫn đến file cascade XML (đảm bảo rằng file này đã có trong thư mục dự án)
        var faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");

        // Chuyển ảnh thành ảnh xám để tăng hiệu quả phát hiện
        var grayImage = image.Convert<Gray, byte>();

        // Phát hiện các khuôn mặt
        var faces = faceCascade.DetectMultiScale(
            grayImage,
            1.1,       // Scale Factor
            10,        // Số lượng Neighbor
            new Size(20, 20) // Kích thước tối thiểu của khuôn mặt
        );

        // Nếu phát hiện khuôn mặt, trả về vùng chữ nhật của khuôn mặt đầu tiên
        if (faces.Length > 0)
        {
            return faces[0];
        }

        // Nếu không phát hiện, trả về vùng chữ nhật rỗng
        return Rectangle.Empty;
    }
}
