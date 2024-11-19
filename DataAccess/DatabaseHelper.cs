using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceStudent.DataAccess
{
    public class DatabaseHelper
    {
        private readonly string connectionstring = "Data Source=LAPTOP-N5C9VL9E\\SQLEXPRESS;Initial Catalog=DataFaceID;User ID=sa;Password=123456;";

        private SqlConnection connection;

        public DatabaseHelper()
        {
            connection = new SqlConnection(connectionstring);

        }
        public void OpenConnection()
        {
            try
            {
                if(connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                    Console.WriteLine("Kết Nối Thành Công");
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Không mở được kết nối : {ex.Message}");
            }
        }

        public void CloseConnection()
        {
            try
            {
                if(connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    Console.WriteLine("Đóng Kết Nối Thành Công");
                }
            }catch(Exception ex)
            {
                Console.WriteLine($"Không Đóng Được Kết Nối: {ex.Message}");
            }
        }
        // Hàm thực thi câu lệnh SQL (Insert, Update, Delete)
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            // Mở kết nối đến cơ sở dữ liệu
            OpenConnection();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    // Thực thi câu lệnh SQL và trả về số lượng bản ghi bị ảnh hưởng
                    return command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi thực thi câu lệnh SQL: {ex.Message}");
                    return -1; // Trả về -1 nếu có lỗi
                }
            }
        }
        // Hàm thực thi câu lệnh SQL truy vấn (Select)
        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            // Mở kết nối đến cơ sở dữ liệu
            OpenConnection();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    try
                    {
                        adapter.Fill(dataTable); // Đổ dữ liệu vào DataTable
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi khi thực thi câu lệnh SQL: {ex.Message}");
                    }
                    return dataTable;
                }
            }
        }
        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            // Mở kết nối đến cơ sở dữ liệu
            OpenConnection();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    // Thực thi câu lệnh và trả về giá trị đầu tiên
                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi thực thi câu lệnh SQL: {ex.Message}");
                    return null; // Trả về null nếu có lỗi
                }
                finally
                {
                    // Đóng kết nối
                    CloseConnection();
                }
            }
        }

        public SqlConnection GetSqlConnection()
        {
            return connection;
        }

       



    }
}
