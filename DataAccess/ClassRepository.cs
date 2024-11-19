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
    public  class ClassRepository
    {
        private DatabaseHelper _databaseHelper;

        public ClassRepository()
        {
            _databaseHelper = new DatabaseHelper();
        }

        // Thêm một lớp mới vào cơ sở dữ liệu
        public void AddClass(Class newClass)
        {
            string query = "INSERT INTO Class (ClassID, ClassName) VALUES (@ClassID, @ClassName)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ClassID", SqlDbType.Char) { Value = newClass.ClassID },
                new SqlParameter("@ClassName", SqlDbType.NVarChar) { Value = newClass.ClassName }
            };

            _databaseHelper.OpenConnection();
            _databaseHelper.ExecuteNonQuery(query, parameters);
            _databaseHelper.CloseConnection();
        }
        // Lấy danh sách tất cả các lớp
        public List<Class> GetAllClasses()
        {
            List<Class> classes = new List<Class>();
            string query = "SELECT * FROM Class";

            _databaseHelper.OpenConnection();
            DataTable dataTable = _databaseHelper.ExecuteQuery(query);

            foreach (DataRow row in dataTable.Rows)
            {
                classes.Add(new Class
                {
                    ClassID = row["ClassID"].ToString(),
                    ClassName = row["ClassName"].ToString()
                });
            }

            _databaseHelper.CloseConnection();
            return classes;
        }
        // Cập nhật thông tin một lớp
        public void UpdateClass(Class updatedClass)
        {
            string query = "UPDATE Class SET ClassName = @ClassName WHERE ClassID = @ClassID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ClassID", SqlDbType.Char) { Value = updatedClass.ClassID },
                new SqlParameter("@ClassName", SqlDbType.NVarChar) { Value = updatedClass.ClassName }
            };

            _databaseHelper.OpenConnection();
            _databaseHelper.ExecuteNonQuery(query, parameters);
            _databaseHelper.CloseConnection();
        }
        // Xóa một lớp theo ClassID
        public void DeleteClass(string classID)
        {
            string query = "DELETE FROM Class WHERE ClassID = @ClassID";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@ClassID", SqlDbType.Char) { Value = classID }
            };

            _databaseHelper.OpenConnection();
           _databaseHelper.ExecuteNonQuery(query, parameters);
            _databaseHelper.CloseConnection();
        }


    }
}
