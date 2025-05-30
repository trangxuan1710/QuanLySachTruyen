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
    public partial class khachhang : Form
    {
        public khachhang()
        {
            InitializeComponent();
        }

        private void btnmuonsach_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();

            MuonSach formMuonsach = new MuonSach();

            formMuonsach.ShowDialog();
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
        private void btnthongke_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();

            thongke formThongke = new thongke();

            formThongke.ShowDialog();
            this.Close();
        }
        private void btnvipham_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();

            vipham formVipham   = new vipham();

            formVipham.ShowDialog();
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

        private void btthoat_Click(object sender, EventArgs e)
        {
            this.Hide();

            Dashboard_book formTrangchu = new Dashboard_book();

            // Hiển thị form TrangChu
            formTrangchu.ShowDialog();
            this.Close();
        }

        private void LoadData()
        {
            DAO.Connect();  // Mở kết nối

            string sql = "SELECT * FROM KhachHang";
            DataTable dt = DAO.LoadDataToTable(sql);
           datakhachhang.DataSource = dt;

            DAO.Close();    // Đóng kết nối
        }

        private void FormKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void btthem_Click(object sender, EventArgs e)
        {
            DateTime ngaySinh;
            if (!DateTime.TryParseExact(mngaysinh.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaySinh))
            {
                MessageBox.Show("Ngày sinh không hợp lệ! Vui lòng nhập đúng định dạng dd/MM/yyyy");
                return;
            }

            DAO.Connect();

            string ma = txtmakhachhang.Text;
            string ten = txttenkhachhang.Text;
            string diachi = txtdiachi.Text;
            string ngaysinh = mngaysinh.Text;
            string gioitinh = checkgioitinh.Checked ? "Nam" : "Nữ";

            string sql = "INSERT INTO KhachHang (MaKhach, TenKhach, NgaySinh, GioiTinh, DiaChi) " +
                         "VALUES (@Ma, @Ten, @NgaySinh, @GioiTinh, @DiaChi)";

            SqlCommand cmd = new SqlCommand(sql, DAO.con);
            cmd.Parameters.AddWithValue("@Ma", ma);
            cmd.Parameters.AddWithValue("@Ten", ten);
            cmd.Parameters.AddWithValue("@NgaySinh", ngaysinh);
            cmd.Parameters.AddWithValue("@GioiTinh", gioitinh);
            cmd.Parameters.AddWithValue("@DiaChi", diachi);

            cmd.ExecuteNonQuery();
            DAO.Close();

            LoadData(); // Load lại bảng sau khi thêm
        }

        private void datakhachhang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void khachhang_Load(object sender, EventArgs e)
        {

        }

        private void btsua_Click(object sender, EventArgs e)
        {
            DAO.Connect();

            string ma = txtmakhachhang.Text;
            string ten = txttenkhachhang.Text;
            string diachi = txtdiachi.Text;
            string ngaysinh = mngaysinh.Text;
            string gioitinh = checkgioitinh.Checked ? "Nam" : "Nữ";

            string sql = "UPDATE KhachHang SET TenKhach = @Ten, NgaySinh = @NgaySinh, GioiTinh = @GioiTinh, DiaChi = @DiaChi " +
                         "WHERE MaKhach = @Ma";

            SqlCommand cmd = new SqlCommand(sql, DAO.con);
            cmd.Parameters.AddWithValue("@Ma", ma);
            cmd.Parameters.AddWithValue("@Ten", ten);
            cmd.Parameters.AddWithValue("@NgaySinh", ngaysinh);
            cmd.Parameters.AddWithValue("@GioiTinh", gioitinh);
            cmd.Parameters.AddWithValue("@DiaChi", diachi);

            cmd.ExecuteNonQuery();
            DAO.Close();

            LoadData();
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            DAO.Connect();

            string ma = txtmakhachhang.Text;
            string sql = "DELETE FROM KhachHang WHERE MaKhach = @Ma";

            SqlCommand cmd = new SqlCommand(sql, DAO.con);
            cmd.Parameters.AddWithValue("@Ma", ma);

            cmd.ExecuteNonQuery();
            DAO.Close();

            LoadData();
        }

        private void datakhachhang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0)
            {
                txtmakhachhang.Text = datakhachhang.Rows[i].Cells["MaKhach"].Value.ToString();
                txttenkhachhang.Text = datakhachhang.Rows[i].Cells["TenKhach"].Value.ToString();
                mngaysinh.Text = datakhachhang.Rows[i].Cells["NgaySinh"].Value.ToString();
                txtdiachi.Text = datakhachhang.Rows[i].Cells["DiaChi"].Value.ToString();
                string gioiTinh = datakhachhang.Rows[i].Cells["GioiTinh"].Value.ToString();
                checkgioitinh.Checked = gioiTinh == "Nam";
                checknu.Checked = gioiTinh == "Nữ";
            }
        }

        private void btluu_Click(object sender, EventArgs e)
        {
            string ma = txtmakhachhang.Text;
            string ten = txttenkhachhang.Text;
            string diachi = txtdiachi.Text;
            string ngaysinh = mngaysinh.Text;
            string gioitinh = checkgioitinh.Checked ? "Nam" : "Nữ";

            DAO.Connect();

            // Kiểm tra khách hàng đã tồn tại hay chưa
            string sqlCheck = "SELECT COUNT(*) FROM KhachHang WHERE MaKhach = @Ma";
            SqlCommand cmdCheck = new SqlCommand(sqlCheck, DAO.con);
            cmdCheck.Parameters.AddWithValue("@Ma", ma);

            int count = (int)cmdCheck.ExecuteScalar();

            if (count == 0)
            {
                // Thêm mới
                string sqlInsert = "INSERT INTO KhachHang (MaKhach, TenKhach, NgaySinh, GioiTinh, DiaChi) " +
                                   "VALUES (@Ma, @Ten, @NgaySinh, @GioiTinh, @DiaChi)";
                SqlCommand cmdInsert = new SqlCommand(sqlInsert, DAO.con);
                cmdInsert.Parameters.AddWithValue("@Ma", ma);
                cmdInsert.Parameters.AddWithValue("@Ten", ten);
                cmdInsert.Parameters.AddWithValue("@NgaySinh", ngaysinh);
                cmdInsert.Parameters.AddWithValue("@GioiTinh", gioitinh);
                cmdInsert.Parameters.AddWithValue("@DiaChi", diachi);
                cmdInsert.ExecuteNonQuery();

                MessageBox.Show("Đã thêm khách hàng mới.");
            }
            else
            {
                // Sửa khách hàng cũ
                string sqlUpdate = "UPDATE KhachHang SET TenKhach = @Ten, NgaySinh = @NgaySinh, " +
                                   "GioiTinh = @GioiTinh, DiaChi = @DiaChi WHERE MaKhach = @Ma";
                SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, DAO.con);
                cmdUpdate.Parameters.AddWithValue("@Ma", ma);
                cmdUpdate.Parameters.AddWithValue("@Ten", ten);
                cmdUpdate.Parameters.AddWithValue("@NgaySinh", ngaysinh);
                cmdUpdate.Parameters.AddWithValue("@GioiTinh", gioitinh);
                cmdUpdate.Parameters.AddWithValue("@DiaChi", diachi);
                cmdUpdate.ExecuteNonQuery();

                MessageBox.Show("Đã cập nhật thông tin khách hàng.");
            }

            DAO.Close();
            LoadData(); // load lại dữ liệu
        }


        private void bthuy_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu trong các TextBox
            txtmakhachhang.Text = "";
            txttenkhachhang.Text = "";
            txtdiachi.Text = "";
            mngaysinh.Text = "";

            // Reset giới tính về mặc định (ví dụ: Nam)
            checkgioitinh.Checked = true;

            // (Nếu có DataGridView hoặc trạng thái cần reset thêm, xử lý tại đây)

            // Đưa focus về ô đầu tiên
            txtmakhachhang.Focus();
        }

        private void btnsachtruyen_Click(object sender, EventArgs e)
        {
           sachtruyen f = new sachtruyen(); // FormSach là form quản lý Sách Truyện
            f.ShowDialog();              // Mở form Sách Truyện dưới dạng dialog
        }
    }
    }

