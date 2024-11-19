using FaceStudent.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudent.DataAccess
{
    public class TeacherRepository
    {
        private DatabaseHelper _databaseHelper;

        public TeacherRepository()
        {
            _databaseHelper = new DatabaseHelper();
        }

        // Thêm mới một giáo viên vào cơ sở dữ liệu
        public int AddTeacher(Teacher teacher)
        {
            string query = "INSERT INTO Teacher (ClassID, TeacherName, Gender, DateOfBirth, StartDate) " +
                           "VALUES (@ClassID, @TeacherName, @Gender, @DateOfBirth, @StartDate)";
            SqlParameter[] parameters = {
                new SqlParameter("@ClassID", teacher.ClassID),
                new SqlParameter("@TeacherName", teacher.TeacherName),
                new SqlParameter("@Gender", teacher.Gender),
                new SqlParameter("@DateOfBirth", teacher.DateOfBirth),
                new SqlParameter("@StartDate", teacher.StartDate)
            };

            return _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        // Cập nhật thông tin giáo viên
        public int UpdateTeacher(Teacher teacher)
        {
            string query = "UPDATE Teacher SET ClassID = @ClassID, TeacherName = @TeacherName, " +
                           "Gender = @Gender, DateOfBirth = @DateOfBirth, StartDate = @StartDate WHERE TeacherID = @TeacherID";
            SqlParameter[] parameters = {
                new SqlParameter("@TeacherID", teacher.TeacherID),
                new SqlParameter("@ClassID", teacher.ClassID),
                new SqlParameter("@TeacherName", teacher.TeacherName),
                new SqlParameter("@Gender", teacher.Gender),
                new SqlParameter("@DateOfBirth", teacher.DateOfBirth),
                new SqlParameter("@StartDate", teacher.StartDate)
            };

            return _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        // Xóa một giáo viên
        public int DeleteTeacher(int teacherID)
        {
            string query = "DELETE FROM Teacher WHERE TeacherID = @TeacherID";
            SqlParameter[] parameters = {
                new SqlParameter("@TeacherID", teacherID)
            };

            return _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        // Lấy giáo viên theo ID
        public Teacher GetTeacherById(int teacherID)
        {
            string query = "SELECT * FROM Teacher WHERE TeacherID = @TeacherID";
            SqlParameter[] parameters = {
                new SqlParameter("@TeacherID", teacherID)
            };

            var dataTable = _databaseHelper.ExecuteQuery(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new Teacher
                {
                    TeacherID = Convert.ToInt32(row["TeacherID"]),
                    ClassID = row["ClassID"].ToString(),
                    TeacherName = row["TeacherName"].ToString(),
                    Gender = row["Gender"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    StartDate = Convert.ToDateTime(row["StartDate"])
                };
            }
            return null; // Không tìm thấy giáo viên với TeacherID này
        }

        // Lấy tất cả giáo viên
        public List<Teacher> GetAllTeachers()
        {
            string query = "SELECT * FROM Teacher";
            var dataTable = _databaseHelper.ExecuteQuery(query);

            List<Teacher> teachers = new List<Teacher>();
            foreach (DataRow row in dataTable.Rows)
            {
                teachers.Add(new Teacher
                {
                    TeacherID = Convert.ToInt32(row["TeacherID"]),
                    ClassID = row["ClassID"].ToString(),
                    TeacherName = row["TeacherName"].ToString(),
                    Gender = row["Gender"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    StartDate = Convert.ToDateTime(row["StartDate"])
                });
            }
            return teachers;
        }
    }
}
