using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using FaceStudent.Model;

namespace FaceStudent.DataAccess
{
    public class FaceCheckRepository
    {
        private DatabaseHelper _databaseHelper;

        public FaceCheckRepository()
        {
            _databaseHelper = new DatabaseHelper();
        }

        // Thêm bản ghi FaceCheck vào cơ sở dữ liệu
        public int AddFaceCheck(FaceCheck faceCheck)
        {
            string query = "INSERT INTO FaceCheck (StudentID, FaceImage, DateCheck, Status) " +
                           "VALUES (@StudentID, @FaceImage, @DateCheck, @Status)";
            SqlParameter[] parameters = {
                new SqlParameter("@StudentID", faceCheck.StudentID),
                new SqlParameter("@FaceImage", faceCheck.FaceImage),
                new SqlParameter("@DateCheck", faceCheck.DateCheck),
                new SqlParameter("@Status", faceCheck.Status)
            };

            return _databaseHelper.ExecuteNonQuery(query, parameters);
        }

        // Lấy hình ảnh khuôn mặt theo ID sinh viên
        public byte[] GetFaceImageByStudentID(int studentID)
        {
            string query = "SELECT FaceImage FROM FaceCheck WHERE StudentID = @StudentID";
            SqlParameter[] parameters = {
                new SqlParameter("@StudentID", studentID)
            };

            var dataTable = _databaseHelper.ExecuteQuery(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                return (byte[])dataTable.Rows[0]["FaceImage"];
            }
            return null;
        }

        // So sánh khuôn mặt (hash-based comparison)
        public bool CompareFaces(int studentID, byte[] inputFaceImage)
        {
            // Lấy ảnh khuôn mặt đã lưu
            byte[] storedFaceImage = GetFaceImageByStudentID(studentID);
            if (storedFaceImage == null) return false;

            // Tính hash của ảnh đầu vào và ảnh lưu trữ
            string inputHash = ComputeImageHash(inputFaceImage);
            string storedHash = ComputeImageHash(storedFaceImage);

            // So sánh hash
            return inputHash == storedHash;
        }

        // Chuyển đổi ảnh thành hash SHA256
        private string ComputeImageHash(byte[] imageBytes)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(imageBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Chuyển đổi hình ảnh từ Image thành byte[]
        public byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        // Chuyển đổi từ byte[] thành Image
        public Image ByteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
        public Student GetStudentByID(int studentID)
        {
            string query = "SELECT StudentID, Name FROM Students WHERE StudentID = @StudentID";
            SqlParameter[] parameters = {
                new SqlParameter("@StudentID", studentID)
            };

            var dataTable = _databaseHelper.ExecuteQuery(query, parameters);
            if (dataTable.Rows.Count > 0)
            {
                // Giả sử lớp Student có các thuộc tính StudentID và Name
                var student = new Student
                {
                    StudentID = (int)dataTable.Rows[0]["StudentID"],
                   StudentName = dataTable.Rows[0]["Name"].ToString()
                };
                return student;
            }
            return null;  // Trả về null nếu không tìm thấy học sinh
        }
    }
}
