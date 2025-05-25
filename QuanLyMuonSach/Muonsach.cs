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

namespace btl
{
    public partial class MuonSach : Form
    {
        private string originalMaThue = "";
        private bool isAddingNew = false;
        private DataTable muonSachTable;
        public MuonSach()
        {
            InitializeComponent();
            DAO.Connect();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT MaThue, MaKhach, MaNV, NgayThue, TienDatCoc FROM ThueSach";

                muonSachTable = DAO.LoadDataToTable(query);

                if (DataGridViewMuon != null)
                    DataGridViewMuon.DataSource = muonSachTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void loadDataToGridViewMuon()
        {
            string sql = "Select * from ThueSach";
            DataTable dt = DAO.LoadDataToTable(sql);
            DataGridViewMuon.DataSource = dt;
            DataGridViewMuon.Columns[0].HeaderText = "Mã thuê";
            DataGridViewMuon.Columns[1].HeaderText = "Mã khách";
            DataGridViewMuon.Columns[2].HeaderText = "Mã nhân viên";
            DataGridViewMuon.Columns[3].HeaderText = "Ngày thuê";
            DataGridViewMuon.Columns[4].HeaderText = "Tiền cọc";
            DataGridViewMuon.AllowUserToAddRows = false;
            DataGridViewMuon.EditMode = DataGridViewEditMode.EditProgrammatically;
        }
        
        private void EnableInputFields(bool enable)
        {
            // txtmathue.Enabled = enable; 
            txtmakhach.Enabled = enable;
            txtmanv.Enabled = enable;
            dngaythue.Enabled = enable;
            txttiencoc.Enabled = enable;
        }

        private void MuonSach_Load(object sender, EventArgs e)
        {
            loadDataToGridViewMuon();
        }
        private void bsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmathue.Text))
            {
                MessageBox.Show("Vui lòng chọn một thông tin mượn để sửa!");
                return;
            }

            isAddingNew = false; // Đặt trạng thái là đang cập nhật

            EnableInputFields(true); // Bật tất cả các trường nhập liệu
            txtmathue.Enabled = false; // KHÔNG cho phép sửa Mã thuê khi đang ở chế độ sửa
            txtmakhach.Focus(); // Di chuyển con trỏ đến mã khách

            // Cập nhật trạng thái các nút
            bthem.Enabled = false;
            bsua.Enabled = false;
            bxoa.Enabled = false;
            bluu.Enabled = true; // Bật nút Lưu
        }

        private void bluu_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra tính hợp lệ: các trường không được để trống
            if (string.IsNullOrWhiteSpace(txtmathue.Text) ||
                string.IsNullOrWhiteSpace(txtmakhach.Text) ||
                string.IsNullOrWhiteSpace(txtmanv.Text) ||
                string.IsNullOrWhiteSpace(txttiencoc.Text))
            {
                MessageBox.Show("Yêu cầu nhập đủ thông tin!");
                return;
            }

            string maThue = txtmathue.Text.Trim();
            string maKhach = txtmakhach.Text.Trim();
            string maNhanVien = txtmanv.Text.Trim();
            DateTime ngayThue = dngaythue.Value.Date; // Lấy chỉ ngày, bỏ giờ phút giây

            // Chuyển đổi tiền cọc sang decimal
            if (!decimal.TryParse(txttiencoc.Text.Trim(), out decimal tienCoc))
            {
                MessageBox.Show("Tiền cọc không hợp lệ. Vui lòng nhập số.");
                return;
            }


            if (isAddingNew)
            {
                // Chế độ THÊM MỚI
                // Kiểm tra trùng MaThue
                DataRow existingMaThueRow = muonSachTable.AsEnumerable().FirstOrDefault(row => row.Field<string>("MaThue").Trim().Equals(maThue, StringComparison.OrdinalIgnoreCase));
                if (existingMaThueRow != null)
                {
                    MessageBox.Show("Mã thuê đã tồn tại! Vui lòng nhập mã thuê khác.");
                    txtmathue.Focus();
                    return;
                }

                try
                {
                    string insertQuery = "INSERT INTO ThueSach (MaThue, MaKhach, MaNV, NgayThue, TienDatCoc) " +
                                         "VALUES (@MaThue, @MaKhach, @MaNhanVien, @NgayThue, @TienCoc)";
                    SqlCommand cmd = new SqlCommand(insertQuery, DAO.con);

                    cmd.Parameters.AddWithValue("@MaThue", maThue);
                    cmd.Parameters.AddWithValue("@MaKhach", maKhach);
                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                    cmd.Parameters.AddWithValue("@NgayThue", ngayThue);
                    cmd.Parameters.AddWithValue("@TienCoc", tienCoc);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm thông tin mượn thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi thêm thông tin mượn!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi ghi dữ liệu: " + ex.Message);
                }
            }
            else // Chế độ CẬP NHẬT
            {
                // Đảm bảo không thay đổi MaThue khi sửa
                if (!maThue.Equals(originalMaThue, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Bạn không được thay đổi Mã Thuê khi đang cập nhật thông tin!");
                    txtmathue.Text = originalMaThue; // Khôi phục lại MaThue ban đầu
                    txtmathue.Focus();
                    return;
                }

                try
                {
                    string updateQuery = "UPDATE ThueSach " +
                                         "SET MaKhach = @MaKhach, " +
                                         "MaNV = @MaNhanVien, " +
                                         "NgayThue = @NgayThue, " +
                                         "TienDatCoc = @TienCoc " +
                                         "WHERE MaThue = @MaThue"; // Sử dụng MaThue để xác định bản ghi

                    SqlCommand cmd = new SqlCommand(updateQuery, DAO.con);

                    cmd.Parameters.AddWithValue("@MaKhach", maKhach);
                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                    cmd.Parameters.AddWithValue("@NgayThue", ngayThue);
                    cmd.Parameters.AddWithValue("@TienCoc", tienCoc);
                    cmd.Parameters.AddWithValue("@MaThue", maThue); // Đây là MaThue dùng trong WHERE clause

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin mượn thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không có thông tin mượn!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message);
                }
            }

            // Sau khi lưu hoặc cập nhật xong, đưa về trạng thái ban đầu
            isAddingNew = false;
            EnableInputFields(false);
            txtmathue.Enabled = false; // Luôn khóa Mã thuê sau khi thao tác
            LoadData(); // Tải lại dữ liệu vào DataGridView

            // Cập nhật trạng thái các nút về mặc định
            bthem.Enabled = true;
            bsua.Enabled = true;
            bxoa.Enabled = true;
            bluu.Enabled = false;
        }

        private void bthoat_Click(object sender, EventArgs e)
        {
            this.Hide();

            Dashboard_book formTrangChu = new Dashboard_book();

            // Hiển thị form TrangChu
            formTrangChu.ShowDialog();
            this.Close();
        }

        private void bxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmathue.Text))
            {
                MessageBox.Show("Vui lòng chọn thông tin mượn cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn chắc chắn muốn xóa thông tin mượn này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string maThueToDelete = txtmathue.Text.Trim();
                try
                {
                    string deleteQuery = "DELETE FROM ThueSach WHERE MaThue = @MaThue";
                    SqlCommand cmd = new SqlCommand(deleteQuery, DAO.con);
                    cmd.Parameters.AddWithValue("@MaThue", maThueToDelete);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa thông tin mượn thành công!");
                        // Xóa trắng các trường sau khi xóa
                        txtmathue.Text = "";
                        txtmakhach.Text = "";
                        txtmanv.Text = "";
                        dngaythue.Value = DateTime.Now;
                        txttiencoc.Text = "";
                        originalMaThue = "";
                        EnableInputFields(false); // Khóa các trường sau khi xóa

                        LoadData(); // Tải lại dữ liệu
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin mượn để xóa!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa dữ liệu: " + ex.Message);
                }
            }
        }

        private void DataGridViewMuon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGridViewMuon.Rows[e.RowIndex];
                txtmathue.Text = row.Cells["MaThue"].Value?.ToString();
                txtmakhach.Text = row.Cells["MaKhach"].Value?.ToString();
                txtmanv.Text = row.Cells["MaNV"].Value?.ToString();

                if (row.Cells["NgayThue"].Value != DBNull.Value && DateTime.TryParse(row.Cells["NgayThue"].Value.ToString(), out DateTime ngayThueValue))
                {
                    dngaythue.Value = ngayThueValue;
                }
                else
                {
                    dngaythue.Value = DateTime.Now; // Đặt giá trị mặc định nếu ngày thuê không hợp lệ/null
                }

                txttiencoc.Text = row.Cells["TienDatCoc"].Value?.ToString();
                originalMaThue = txtmathue.Text; // Lưu Mã Thuê của hàng đang chọn

                // Cập nhật trạng thái các nút
                isAddingNew = false; // Không phải chế độ thêm mới
                EnableInputFields(false); // Khóa các trường (chỉ xem thông tin)
                txtmathue.Enabled = false; // Luôn khóa Mã thuê khi chọn

                bthem.Enabled = true;
                bsua.Enabled = true; // Bật nút Sửa
                bxoa.Enabled = true; // Bật nút Xóa
                bluu.Enabled = false; // Vô hiệu hóa nút Lưu
            }
        }

        private void bthem_Click(object sender, EventArgs e)
        {
            isAddingNew = true; // Đặt trạng thái là đang thêm mới
            originalMaThue = ""; // Xóa mã thuê gốc để biết đây là bản ghi mới

            // Xóa trắng các trường nhập liệu
            txtmathue.Text = "";
            txtmakhach.Text = "";
            txtmanv.Text = "";
            dngaythue.Value = DateTime.Now; // Đặt ngày thuê là ngày hiện tại
            txttiencoc.Text = "";

            EnableInputFields(true); // Bật tất cả các trường nhập liệu
            txtmathue.Enabled = true; // Cho phép nhập Mã thuê khi thêm mới
            txtmathue.Focus(); // Di chuyển con trỏ vào ô Mã thuê

            // Cập nhật trạng thái các nút
            bthem.Enabled = true;
            bsua.Enabled = false;
            bxoa.Enabled = false;
            bluu.Enabled = true; // Bật nút Lưu
        }
    }
}
