using FaceStudent.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudent.DataAccess
{
    public  class UserRepository
    {
        private DatabaseHelper _databaseHelper;

        public UserRepository()
        {
            _databaseHelper = new DatabaseHelper();
        }
        // Thêm mới một User vào cơ sở dữ liệu
        public int AddUser(User user)
        {
            string query = "INSERT INTO Users (TeacherID, UserName, Password, IsLocked, Role) " +
                           "VALUES (@TeacherID, @UserName, @Password, @IsLocked, @Role)";
            SqlParameter[] parameters = {
                new SqlParameter("@TeacherID", user.TeacherID),
                new SqlParameter("@UserName", user.UserName),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@IsLocked", user.IsLocked),
                new SqlParameter("@Role", user.Role)
            };

            return _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        public int UpdateUser(User user)
        {
            string query = "UPDATE Users SET TeacherID = @TeacherID, UserName = @UserName, " +
                           "Password = @Password, IsLocked = @IsLocked, Role = @Role WHERE UserID = @UserID";
            SqlParameter[] parameters = {
        new SqlParameter("@UserID", user.UserID),
        new SqlParameter("@TeacherID", user.TeacherID),
        new SqlParameter("@UserName", user.UserName),
        new SqlParameter("@Password", user.Password),
        new SqlParameter("@IsLocked", user.IsLocked),
        new SqlParameter("@Role", user.Role)
    };

            return _databaseHelper.ExecuteNonQuery(query, parameters);
        }
        public int DeleteUser(int userID)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";
            SqlParameter[] parameters = {
        new SqlParameter("@UserID", userID)
    };

            return _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        public User GetUserById(int userID)
        {
            string query = "SELECT * FROM Users WHERE UserID = @UserID";
            SqlParameter[] parameters = {
        new SqlParameter("@UserID", userID)
    };

            var dataTable = _databaseHelper.ExecuteQuery(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new User
                {
                    UserID = Convert.ToInt32(row["UserID"]),
                    TeacherID = Convert.ToInt32(row["TeacherID"]),
                    UserName = row["UserName"].ToString(),
                    Password = row["Password"].ToString(),
                    IsLocked = Convert.ToBoolean(row["IsLocked"]),
                    Role = row["Role"].ToString()
                };
            }
            return null; // Không tìm thấy User với UserID này
        }

        public List<User> GetAllUsers()
        {
            string query = "SELECT * FROM Users";
            var dataTable = _databaseHelper.ExecuteQuery(query);

            List<User> users = new List<User>();
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                users.Add(new User
                {
                    UserID = Convert.ToInt32(row["UserID"]),
                    TeacherID = Convert.ToInt32(row["TeacherID"]),
                    UserName = row["UserName"].ToString(),
                    Password = row["Password"].ToString(),
                    IsLocked = Convert.ToBoolean(row["IsLocked"]),
                    Role = row["Role"].ToString()
                });
            }
            return users;
        }
        // Phương thức kiểm tra đăng nhập và quyền User
        public User Login(string userName, string password)
        {
            string query = "SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password AND IsLocked = 0";
            SqlParameter[] parameters = {
                new SqlParameter("@UserName", userName),
                new SqlParameter("@Password", password)
            };

            var dataTable = _databaseHelper.ExecuteQuery(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                var row = dataTable.Rows[0];
                return new User
                {
                    UserID = Convert.ToInt32(row["UserID"]),
                    TeacherID = Convert.ToInt32(row["TeacherID"]),
                    UserName = row["UserName"].ToString(),
                    Password = row["Password"].ToString(),
                    IsLocked = Convert.ToBoolean(row["IsLocked"]),
                    Role = row["Role"].ToString()
                };
            }
            return null; // Nếu không tìm thấy user hoặc mật khẩu sai, hoặc user bị khóa
        }

        // Phương thức kiểm tra quyền User
        public bool CheckUserRole(int userID, string role)
        {
            string query = "SELECT * FROM Users WHERE UserID = @UserID AND Role = @Role";
            SqlParameter[] parameters = {
                new SqlParameter("@UserID", userID),
                new SqlParameter("@Role", role)
            };

            var dataTable = _databaseHelper.ExecuteQuery(query, parameters);
            return dataTable.Rows.Count > 0;
        }



    }
}
