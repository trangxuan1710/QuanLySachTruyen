using System;
using System.Windows.Forms;
using QuanLyMuonSach;
using QuanLySachTruyen;

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
            if (txtten.Text == "NET" && txtmk.Text == "123")
            {
                Dashboard_book dbs = new Dashboard_book();
                dbs.Show();
                this.Hide();
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}