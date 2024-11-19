using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudent.Model
{
    public class Teacher
    {
        // Properties tương ứng với các cột trong bảng Teacher
        public int TeacherID { get; set; } // ID tự tăng, khóa chính
        public string ClassID { get; set; } // Khóa ngoại tham chiếu đến Class
        public string TeacherName { get; set; } // Tên giáo viên
        public string Gender { get; set; } // Giới tính
        public DateTime DateOfBirth { get; set; } // Ngày sinh
        public DateTime StartDate { get; set; } // Ngày bắt đầu làm việc

        // Constructor không tham số
        public Teacher() { }

        // Constructor với tham số
        public Teacher(int teacherID, string classID, string teacherName, string gender, DateTime dateOfBirth, DateTime startDate)
        {
            TeacherID = teacherID;
            ClassID = classID;
            TeacherName = teacherName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            StartDate = startDate;
        }
    }
}
