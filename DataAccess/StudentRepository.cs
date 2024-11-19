using FaceStudent.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudent.DataAccess
{
    public class StudentRepository
    {
        private DatabaseHelper _databaseHelper;
        public StudentRepository()
        {
            _databaseHelper = new DatabaseHelper();
        }
        public int GetMaxStudentID()
        {
            try
            {
                string query = "SELECT ISNULL(MAX(StudentID), 0) FROM Students"; // Nếu chưa có dữ liệu, trả về 0
                object result = _databaseHelper.ExecuteScalar(query);
                return  Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving max StudentID", ex);
            }
        }


         // Thêm mới một học sinh vào cơ sở dữ liệu
        public int AddStudent(Student student)
        {
            try
            {
                string query = @"
                    INSERT INTO Students (StudentName, Gender, DateOfBirth, StartDate, Address, StudentImage) 
                    VALUES (@StudentName, @Gender, @DateOfBirth, @StartDate, @Address, @StudentImage);
                    SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = {
                    new SqlParameter("@StudentName", SqlDbType.NVarChar) { Value = student.StudentName },
                    new SqlParameter("@Gender", SqlDbType.NVarChar) { Value = student.Gender },
                    new SqlParameter("@DateOfBirth", SqlDbType.DateTime) { Value = student.DateOfBirth },
                    new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = student.StartDate },
                    new SqlParameter("@Address", SqlDbType.NVarChar) { Value = student.Address },
                    new SqlParameter("@StudentImage", SqlDbType.VarBinary) { Value = (object)student.StudentImage ?? DBNull.Value }
                };

                // Trả về ID của học sinh vừa thêm
                return Convert.ToInt32(_databaseHelper.ExecuteScalar(query, parameters));
            }
            catch (Exception ex)
            {
                // Ghi log lỗi hoặc xử lý thêm tại đây
                throw new Exception("Error adding student", ex);
            }
        }

        // Cập nhật thông tin học sinh
        public int UpdateStudent(Student student)
        {
            string query = "UPDATE Students SET StudentName = @StudentName, Gender = @Gender, " +
                           "DateOfBirth = @DateOfBirth, StartDate = @StartDate, Address = @Address, " +
                           "StudentImage = @StudentImage WHERE StudentID = @StudentID";
            SqlParameter[] parameters = {
                new SqlParameter("@StudentID", student.StudentID),
                new SqlParameter("@StudentName", student.StudentName),
                new SqlParameter("@Gender", student.Gender),
                new SqlParameter("@DateOfBirth", student.DateOfBirth),
                new SqlParameter("@StartDate", student.StartDate),
                new SqlParameter("@Address", student.Address),
                new SqlParameter("@StudentImage", student.StudentImage ?? (object)DBNull.Value) // Fix: Use DBNull.Value if StudentImage is null
            };

            return _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        // Xóa học sinh
        public int DeleteStudent(int studentID)
        {
            string query = "DELETE FROM Students WHERE StudentID = @StudentID";
            SqlParameter[] parameters = {
                new SqlParameter("@StudentID", studentID)
            };

            return _databaseHelper.ExecuteNonQuery(query, parameters);
        }
        // Lấy thông tin học sinh theo ID
        public Student GetStudentById(int studentID)
        {
            string query = "SELECT * FROM Students WHERE StudentID = @StudentID";
            SqlParameter[] parameters = {
                new SqlParameter("@StudentID", studentID)
            };

            var dataTable = _databaseHelper.ExecuteQuery(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new Student
                {
                    StudentID = Convert.ToInt32(row["StudentID"]),
                    StudentName = row["StudentName"].ToString(),
                    Gender = row["Gender"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    StartDate = Convert.ToDateTime(row["StartDate"]),
                    Address = row["Address"].ToString(),
                    StudentImage = row["StudentImage"] != DBNull.Value ? (byte[])row["StudentImage"] : null // Fix: Check if the value is DBNull.Value
                };
            }
            return null; // Không tìm thấy học sinh với ID này
        }

        // Lấy tất cả học sinh
        public List<Student> GetAllStudents()
        {
            string query = "SELECT * FROM Students";
            var dataTable = _databaseHelper.ExecuteQuery(query);

            List<Student> students = new List<Student>();
            foreach (DataRow row in dataTable.Rows)
            {
                students.Add(new Student
                {
                    StudentID = Convert.ToInt32(row["StudentID"]),
                    StudentName = row["StudentName"].ToString(),
                    Gender = row["Gender"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    StartDate = Convert.ToDateTime(row["StartDate"]),
                    Address = row["Address"].ToString(),
                    StudentImage = row["StudentImage"] != DBNull.Value ? (byte[])row["StudentImage"] : null // Fix: Check if the value is DBNull.Value
                });
            }
            return students;
        }
        public Student GetStudentByFaceId(string faceId)
        {
            string query = "SELECT * FROM Students WHERE FaceID = @FaceID";
            SqlParameter[] parameters = {
        new SqlParameter("@FaceID", faceId)
    };

            var dataTable = _databaseHelper.ExecuteQuery(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new Student
                {
                    StudentID = Convert.ToInt32(row["StudentID"]),
                    StudentName = row["StudentName"].ToString(),
                    Gender = row["Gender"].ToString(),
                    DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                    StartDate = Convert.ToDateTime(row["StartDate"]),
                    Address = row["Address"].ToString(),
                    StudentImage = row["StudentImage"] != DBNull.Value ? (byte[])row["StudentImage"] : null
                };
            }
            return null;
        }


    }
}
