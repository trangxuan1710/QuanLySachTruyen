using System;
using System.Windows.Forms;
using Guna.UI2.WinForms; // Thêm namespace cho Guna2

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
            if (txttentk != null && txttentk.Text == "NET" && txtmatkhau.Text == "123")
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
    }
}