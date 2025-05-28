using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyMuonSach;
using QuanLySachTruyen;

namespace QuanLyMuonSach
{
    public partial class khachhang : Form
    {
        public khachhang()
        {
            InitializeComponent();
        }

        private void btnmuonsach_Click(object sender, EventArgs e)
        {
            this.Hide();

            MuonSach formMuonsach = new MuonSach();

            formMuonsach.ShowDialog();
            this.Close();
        }
        private void btnnhanvien_Click(object sender, EventArgs e)
        {
            this.Hide();

            NhanVien formNhanvien = new NhanVien();

            formNhanvien.ShowDialog();
            this.Close();
        }
        private void btnthongke_Click(object sender, EventArgs e)
        {
            this.Hide();

            thongke formThongke = new thongke();

            formThongke.ShowDialog();
            this.Close();
        }
        private void btnvipham_Click(object sender, EventArgs e)
        {
            this.Hide();

            vipham formVipham   = new vipham();

            formVipham.ShowDialog();
            this.Close();
        }
        private void btnkhachhang_Click(object sender, EventArgs e)
        {
            this.Hide();

            khachhang formKhachhang = new khachhang();

            formKhachhang.ShowDialog();
            this.Close();
        }

        private void btthoat_Click(object sender, EventArgs e)
        {
            this.Hide();

            Dashboard_book formTrangchu = new Dashboard_book();

            // Hiển thị form TrangChu
            formTrangchu.ShowDialog();
            this.Close();
        }
    }
}
