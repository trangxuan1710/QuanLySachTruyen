using System;
using System.Windows.Forms;
using QuanLyMuonSach;

namespace QuanLyMuonSach
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btndangnhap_Click(object sender, EventArgs e)
        {
            if (txttentk.Text == "NET" && txtmatkhau.Text == "123")
            {
                this.Hide();
                Dashboard_book dbs = new Dashboard_book();
                dbs.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txttentk_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtmatkhau_TextChanged(object sender, EventArgs e)
        {

        }
    }
}