using FaceStudent.DataAccess;
using FaceStudent.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceStudent
{
    public partial class ManamentStudent : Form
    {
        private StudentRepository _studentRepository;
        public ManamentStudent()
        {
            InitializeComponent();
            _studentRepository = new StudentRepository();
            
        }
        private void LoadStudentsToGrid()
        {
            var students = _studentRepository.GetAllStudents();
            dataGridViewStudents.DataSource = students;
        }
        

       

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int studentID = int.Parse(txtStudentID.Text);
                _studentRepository.DeleteStudent(studentID);
                MessageBox.Show("Xóa học sinh thành công!");
                LoadStudentsToGrid();
                ClearText();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png",
                Title = "Chọn ảnh học sinh"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image = Image.FromFile(openFileDialog.FileName);
            }
        }
            // Lấy ảnh dưới dạng mảng byte
        private byte[] GetImageBytes()
        {
            if (pictureBox.Image == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                pictureBox.Image.Save(ms, pictureBox.Image.RawFormat);
                return ms.ToArray();
            }
        }

        private void dataGridViewStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewStudents.SelectedRows.Count > 0)
            {
                var row = dataGridViewStudents.SelectedRows[0];
                txtStudentID.Text = row.Cells["StudentID"].Value.ToString();
                txtStudentName.Text = row.Cells["StudentName"].Value.ToString();
                txtGender.Text = row.Cells["Gender"].Value.ToString();
                txtDateOfBirth.Text = row.Cells["DateOfBirth"].Value.ToString();
                txtStartDate.Text = row.Cells["StartDate"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();

                if (row.Cells["StudentImage"].Value != DBNull.Value)
                {
                    byte[] imageBytes = (byte[])row.Cells["StudentImage"].Value;
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pictureBox.Image = null;
                }
            }
        }
        public void ClearText()
        {
            txtStudentID.Clear();
            txtStudentName.Clear();
            txtDateOfBirth.Clear();
            txtStartDate.Clear();
            txtAddress.Clear();
            pictureBox.Image = null;

            
        }

        

        private void ManamentStudent_Load(object sender, EventArgs e)
        {
            try
            {
                // Lấy StudentID cao nhất từ cơ sở dữ liệu
                int maxStudentID = _studentRepository.GetMaxStudentID();

                // Hiển thị ID tiếp theo lên TextBox 
                txtStudentID.Text = (maxStudentID + 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải StudentID: " + ex.Message);
            }
        }

        private void btnUploadImage_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png",
                Title = "Chọn ảnh học sinh"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image = Image.FromFile(openFileDialog.FileName);
            }

        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            try
            {
                var student = new Student
                {
                    StudentName = txtStudentName.Text,
                    Gender = txtGender.Text,
                    DateOfBirth = DateTime.Parse(txtDateOfBirth.Text),
                    StartDate = DateTime.Parse(txtStartDate.Text),
                    Address = txtAddress.Text,
                    StudentImage = GetImageBytes()
                };

                _studentRepository.AddStudent(student);
                MessageBox.Show("Thêm học sinh thành công!");
                LoadStudentsToGrid();
                ClearText();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            try
            {
                var student = new Student
                {
                    StudentID = int.Parse(txtStudentID.Text),
                    StudentName = txtStudentName.Text,
                    Gender = txtGender.Text,
                    DateOfBirth = DateTime.Parse(txtDateOfBirth.Text),
                    StartDate = DateTime.Parse(txtStartDate.Text),
                    Address = txtAddress.Text,
                    StudentImage = GetImageBytes()
                };

                _studentRepository.UpdateStudent(student);
                MessageBox.Show("Cập nhật thông tin học sinh thành công!");
                LoadStudentsToGrid();
                ClearText();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            try
            {
                int studentID = int.Parse(txtStudentID.Text);
                _studentRepository.DeleteStudent(studentID);
                MessageBox.Show("Xóa học sinh thành công!");
                LoadStudentsToGrid();
                ClearText();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void btExit_Click_1(object sender, EventArgs e)
        {
            HomeTeacher homeTeacher = new HomeTeacher();
            homeTeacher.Show();
            this.Hide();

        }
    }


}
