using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FaceStudent.DataAccess;
using FaceStudent.Model;

namespace FaceStudent
{
    public partial class Form1 : Form
    {
        private readonly UserRepository _userRepository;
        public Form1()
        {
            InitializeComponent();
            _userRepository = new UserRepository();
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text;
            string password = txtPassword.Text;

            User user = _userRepository.Login(username, password);
            if(user != null)
            {
                if(_userRepository.CheckUserRole(user.UserID, "Admin"))
                {
                    MessageBox.Show("Đăng nhập thành công! Bạn là Admin.");

                }
                else if(_userRepository.CheckUserRole(user.UserID, "User"))
                {
                    MessageBox.Show("Đăng nhập thành công! Bạn là User");
                    HomeCheckin homeCheckin = new HomeCheckin();
                    homeCheckin.ShowDialog();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Checkpass_CheckedChanged(object sender, EventArgs e)
        {
            if (Checkpass.Checked)
            {
                txtPassword.PasswordChar = (char)0;
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
