using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudent.Model
{
    public class FaceCheck
    {
        // Properties tương ứng với các cột trong bảng FaceCheck
        public int FaceCheckID { get; set; } // ID tự tăng, khóa chính
        public int StudentID { get; set; } // Khóa ngoại tham chiếu đến Student
        public byte[] FaceImage { get; set; } // Ảnh khuôn mặt (dạng nhị phân)
        public DateTime DateCheck { get; set; } // Ngày điểm danh (mặc định là ngày hiện tại)
        public bool Status { get; set; } // Trạng thái điểm danh (true: có mặt, false: vắng mặt)

        // Constructor không tham số
        public FaceCheck() { }

        // Constructor với tham số
        public FaceCheck(int faceCheckID, int studentID, byte[] faceImage, DateTime dateCheck, bool status)
        {
            FaceCheckID = faceCheckID;
            StudentID = studentID;
            FaceImage = faceImage;
            DateCheck = dateCheck;
            Status = status;
        }
    }
}
