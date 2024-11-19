using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudent.Model
{
    public class Class
    {
        // Properties tương ứng với các cột trong bảng Class
        public string ClassID { get; set; } // ID của lớp học (khóa chính)
        public string ClassName { get; set; } // Tên lớp học

        // Constructor không tham số
        public Class() { }

        // Constructor với tham số
        public Class(string classID, string className)
        {
            ClassID = classID;
            ClassName = className;
        }
    }
}
