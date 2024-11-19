using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudent.Model
{
    public class User
    {
        // Properties tương ứng với các cột trong bảng Users
        public int UserID { get; set; } // ID tự tăng, khóa chính
        public int TeacherID { get; set; } // Khóa ngoại tham chiếu đến bảng Teacher
        public string UserName { get; set; } // Tên đăng nhập
        public string Password { get; set; } // Mật khẩu
        public bool IsLocked { get; set; } // Trạng thái khóa tài khoản (true: bị khóa, false: hoạt động)
        public string Role { get; set; }
        // Constructor không tham số
        public User() { }

        // Constructor với tham số
        public User(int userID, int teacherID, string userName, string password, bool isLocked, string role)
        {
            UserID = userID;
            TeacherID = teacherID;
            UserName = userName;
            Password = password;
            IsLocked = isLocked;
            Role = role;
        }
    }
}
