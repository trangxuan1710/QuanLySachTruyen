using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Application.Exit();
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

        private void btnsachtruyen_Click(object sender, EventArgs e)
        {
            this.Hide();
            sachtruyen formSachtruyen = new sachtruyen();
            formSachtruyen.ShowDialog();
            this.Close();
        }

        private void btnkhachhang_Click(object sender, EventArgs e)
        {
            {
                this.Hide();
                khachhang formKH = new khachhang();
                formKH.ShowDialog();
                this.Close();
            }
        }

        private void btnvipham_Click(object sender, EventArgs e)
        {
            this.Hide();
            vipham formVipham = new vipham();
            formVipham.ShowDialog();
            this.Close();
        }

        private void btnthongke_Click(object sender, EventArgs e)
        {
            this.Hide();
            thongke formThongke = new thongke();
            formThongke.ShowDialog();
            this.Close();
        }
    }
}
