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

namespace QuanLyMuonSach
{
    public partial class sachtruyen : Form
    {
        public sachtruyen()
        {
            InitializeComponent();
    }
        private string thaoTac = "";


        private void btthoat_Click(object sender, EventArgs e)
        {
            this.Hide();

            Dashboard_book formTrangchu = new Dashboard_book();

            // Hiển thị form TrangChu
            formTrangchu.ShowDialog();
            this.Close();
        }
        private void btnmuonsach_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();

            MuonSach formMuonsach = new MuonSach();

            formMuonsach.ShowDialog();
            this.Close();
        }

        private void btnkhachhang_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();

            khachhang formKhachhang = new khachhang();

            formKhachhang.ShowDialog();
            this.Close();
        }

        private void btnvipham_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();

            vipham formVipham = new vipham();

            formVipham.ShowDialog();
            this.Close();
        }

        private void btnthongke_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();

            thongke formThongke = new thongke();

            formThongke.ShowDialog();
            this.Close();
        }

        private void btnnhanvien_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();

            NhanVien formNhanvien = new NhanVien();

            formNhanvien.ShowDialog();
            this.Close();
        }

        private void btthem_Click(object sender, EventArgs e)
        {
            thaoTac = "them";
            EnableInput(true);
            ClearInput();
            DAO.Connect();
            string sql = "INSERT INTO SachTruyen (TenSach, MaLoaiSach, MaLinhVuc, MaTG, MaNXB, MaNgonNgu, SoTrang, GiaSach, DonGiaThue, SoLuong, Anh, GhiChu) " +
                         "VALUES (@TenSach, @MaLoaiSach, @MaLinhVuc, @MaTG, @MaNXB, @MaNgonNgu, @SoTrang, @GiaSach, @DonGiaThue, @SoLuong, @Anh, @GhiChu)";
            SqlCommand cmd = new SqlCommand(sql, DAO.con);
            cmd.Parameters.AddWithValue("@TenSach", txttensach.Text);
            cmd.Parameters.AddWithValue("@MaLoaiSach", cbtheloai.Text);
            cmd.Parameters.AddWithValue("@MaLinhVuc", cblinhvuc.Text);
            cmd.Parameters.AddWithValue("@MaTG", txtmatacgia.Text);
            cmd.Parameters.AddWithValue("@MaNXB",txtmanxb.Text);
            cmd.Parameters.AddWithValue("@MaNgonNgu", txtmangonngu.Text);
            cmd.Parameters.AddWithValue("@SoTrang", txtsotrang.Text);
            cmd.Parameters.AddWithValue("@GiaSach", txtgiasach.Text);
            cmd.Parameters.AddWithValue("@DonGiaThue", txtdongiathue.Text);
            cmd.Parameters.AddWithValue("@SoLuong", txtsoluong.Text);
            cmd.Parameters.AddWithValue("@Anh", pickhachhang.Text);
            cmd.Parameters.AddWithValue("@GhiChu",  txtghichu.Text);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm sách thành công!");
                LoadSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            DAO.Close();

        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (thaoTac == "them")

            {
                string sql = "INSERT INTO SachTruyen(TenSach, MaLoaiSach, MaLinhVuc, MaTG, MaNXB, MaNgonNgu, SoTrang, GiaSach, DonGiaThue, SoLuong, Anh, GhiChu)" +
                             " VALUES (N'" + txttensach.Text + "', N'" + cbtheloai.Text + "', N'" + cblinhvuc.Text + "', N'" + txtmatacgia.Text + "'," +
                             " N'" +txtmanxb.Text + "', N'" + txtmangonngu.Text + "', " +txtsotrang.Text + ", " + txtgiasach.Text + ", " +
                             txtdongiathue.Text + ", " + txtsoluong.Text + ", N'" + pickhachhang.Text + "', N'" +  txtghichu.Text + "')";

                DAO.Connect();
                SqlCommand cmd = new SqlCommand(sql, DAO.con);
                cmd.ExecuteNonQuery();
                DAO.Close();
                MessageBox.Show("Đã thêm sách.");
            }
            else if (thaoTac == "sua")
            {
                string sql = "UPDATE SachTruyen SET " +
                             "MaLoaiSach = N'" + cbtheloai.Text + "', " +
                             "MaLinhVuc = N'" + cblinhvuc.Text + "', " +
                             "MaTG = N'" + txtmatacgia.Text + "', " +
                             "MaNXB = N'" +txtmanxb.Text + "', " +
                             "MaNgonNgu = N'" + txtmangonngu.Text + "', " +
                             "SoTrang = " +txtsotrang.Text + ", " +
                             "GiaSach = " + txtgiasach.Text + ", " +
                             "DonGiaThue = " + txtdongiathue.Text + ", " +
                             "SoLuong = " + txtsoluong.Text + ", " +
                             "Anh = N'" + pickhachhang.Text + "', " +
                             "GhiChu = N'" +  txtghichu.Text + "' " +
                             "WHERE TenSach = N'" + txttensach.Text + "'";

                DAO.Connect();
                SqlCommand cmd = new SqlCommand(sql, DAO.con);
                cmd.ExecuteNonQuery();
                DAO.Close();
                MessageBox.Show("Đã cập nhật sách.");
            }

            LoadSach();
            ClearInput();
            EnableInput(false);
            thaoTac = "";
        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            ClearInput();
            EnableInput(false);
            thaoTac = "";
        }
        private void ClearInput()
        {
            txttensach.Clear();
            cbtheloai.Text = "";
            cblinhvuc.Text = "";
            txtmatacgia.Clear();
            txtmanxb.Clear();
            txtmangonngu.Clear();
            txtsotrang.Clear();
            txtgiasach.Clear();
            txtdongiathue.Clear();
            txtsoluong.Clear();
            pickhachhang.Text = "";
            txtghichu.Clear();
        }

        private void EnableInput(bool enable)
        {
            txttensach.Enabled = enable;
            cbtheloai.Enabled = enable;
            cblinhvuc.Enabled = enable;
            txtmatacgia.Enabled = enable;
           txtmanxb.Enabled = enable;
            txtmangonngu.Enabled = enable;
           txtsotrang.Enabled = enable;
            txtgiasach.Enabled = enable;
            txtdongiathue.Enabled = enable;
            txtsoluong.Enabled = enable;
            pickhachhang.Enabled = enable;
             txtghichu.Enabled = enable;
        }


        private void btnSua_Click(object sender, EventArgs e)
        {
            thaoTac = "sua";
            EnableInput(true);
            DAO.Connect();
            string sql = "UPDATE SachTruyen SET " +
                         "MaLoaiSach=@MaLoaiSach, MaLinhVuc=@MaLinhVuc, MaTG=@MaTG, MaNXB=@MaNXB, MaNgonNgu=@MaNgonNgu, " +
                         "SoTrang=@SoTrang, GiaSach=@GiaSach, DonGiaThue=@DonGiaThue, SoLuong=@SoLuong, Anh=@Anh, GhiChu=@GhiChu " +
                         "WHERE TenSach=@TenSach";
            SqlCommand cmd = new SqlCommand(sql, DAO.con);
            cmd.Parameters.AddWithValue("@TenSach", txttensach.Text);
            cmd.Parameters.AddWithValue("@MaLoaiSach", cbtheloai.Text);
            cmd.Parameters.AddWithValue("@MaLinhVuc", cblinhvuc.Text);
            cmd.Parameters.AddWithValue("@MaTG", txtmatacgia.Text);
            cmd.Parameters.AddWithValue("@MaNXB",txtmanxb.Text);
            cmd.Parameters.AddWithValue("@MaNgonNgu", txtmangonngu.Text);
            cmd.Parameters.AddWithValue("@SoTrang",txtsotrang.Text);
            cmd.Parameters.AddWithValue("@GiaSach", txtgiasach.Text);
            cmd.Parameters.AddWithValue("@DonGiaThue", txtdongiathue.Text);
            cmd.Parameters.AddWithValue("@SoLuong", txtsoluong.Text);
            cmd.Parameters.AddWithValue("@Anh", pickhachhang.Text);
            cmd.Parameters.AddWithValue("@GhiChu", txtghichu.Text);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Cập nhật thành công!");
                LoadSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            DAO.Close();
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            DAO.Connect();
            string sql = "DELETE FROM SachTruyen WHERE TenSach = @TenSach";
            SqlCommand cmd = new SqlCommand(sql, DAO.con);
            cmd.Parameters.AddWithValue("@TenSach", txttensach.Text);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Xóa thành công!");
                LoadSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            DAO.Close();
        }

        private void LoadSach()
        {
            DAO.Connect();
            string sql = "SELECT * FROM SachTruyen";
            DataTable dt = DAO.LoadDataToTable(sql);
            datakhachhang.DataSource = dt;
            DAO.Close();
        }

        private void datakhachhang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void btnsachtruyen_Click(object sender, EventArgs e)
        {

        }

        private void sachtruyen_Load(object sender, EventArgs e)
        {
            LoadSach();
        }

        private void datakhachhang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txttensach.Text = datakhachhang.Rows[i].Cells["TenSach"].Value.ToString();
                cbtheloai.Text = datakhachhang.Rows[i].Cells["MaLoaiSach"].Value.ToString();
                cblinhvuc.Text = datakhachhang.Rows[i].Cells["MaLinhVuc"].Value.ToString();
                txtmatacgia.Text = datakhachhang.Rows[i].Cells["MaTG"].Value.ToString();
               txtmanxb.Text = datakhachhang.Rows[i].Cells["MaNXB"].Value.ToString();
                txtmangonngu.Text = datakhachhang.Rows[i].Cells["MaNgonNgu"].Value.ToString();
               txtsotrang.Text = datakhachhang.Rows[i].Cells["SoTrang"].Value.ToString();
                txtgiasach.Text = datakhachhang.Rows[i].Cells["GiaSach"].Value.ToString();
                txtdongiathue.Text = datakhachhang.Rows[i].Cells["DonGiaThue"].Value.ToString();
                txtsoluong.Text = datakhachhang.Rows[i].Cells["SoLuong"].Value.ToString();
                pickhachhang.Text = datakhachhang.Rows[i].Cells["Anh"].Value.ToString();
                txtghichu.Text = datakhachhang.Rows[i].Cells["GhiChu"].Value.ToString();
            }
        }

        private void txtmasach_TextChanged(object sender, EventArgs e)
        {

        }
       

        private void pickhachhang_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pickhachhang.Image = Image.FromFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi mở ảnh: " + ex.Message);
                }
            }
        }
    }
}
    
