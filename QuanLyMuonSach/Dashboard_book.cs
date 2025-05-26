using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySachTruyen;

namespace QuanLyMuonSach
{
    public partial class Dashboard_book : Form
    {
        public Dashboard_book()
        {
            InitializeComponent();
        }

        private void btthoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnmuonsach_Click(object sender, EventArgs e)
        {
            this.Hide();

            MuonSach formMuonsach = new MuonSach();

            // Hiển thị form TrangChu
            formMuonsach.ShowDialog();
            this.Close();
        }

        private void btnnhanvien_Click(object sender, EventArgs e)
        {
            this.Hide();

            NhanVien formNhanvien = new NhanVien();

            // Hiển thị form TrangChu
            formNhanvien.ShowDialog();
            this.Close();
        }
    }
}
