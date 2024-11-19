using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceStudent
{
    public partial class HomeCheckin : Form
    {
        public HomeCheckin()
        {
            InitializeComponent();
        }

        private void tiệnÍchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.ShowDialog();
            this.Hide();
        }

        private void điểmDanhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FaceID faceID = new FaceID();
            faceID.ShowDialog();
            this.Hide();
        }
    }
}
