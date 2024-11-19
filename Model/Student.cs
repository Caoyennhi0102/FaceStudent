using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudent.Model
{
    public  class Student
    {
        public int StudentID { get; set; } // ID tự tăng, khóa chính
        public string StudentName { get; set; } // Tên học sinh
        public string Gender { get; set; } // Giới tính
        public DateTime DateOfBirth { get; set; } // Ngày sinh
        public DateTime StartDate { get; set; } // Ngày bắt đầu
        public string Address { get; set; } // Địa chỉ
        public byte[] StudentImage { get; set; } // Ảnh khuôn mặt học sinh (dạng nhị phân)

        // Constructor không tham số
        public Student() { }

        // Constructor với tham số
        public Student(int studentID, string studentName, string gender, DateTime dateOfBirth, DateTime startDate, string address, byte[] studentImage)
        {
            StudentID = studentID;
            StudentName = studentName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            StartDate = startDate;
            Address = address;
            StudentImage = studentImage;
        }

    }
}
