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
    public partial class NhanVien : Form
    {
        public NhanVien()
        {
            InitializeComponent();
            DAO.Connect();
            LoadData();
        }
        private bool isAddingNew = false;
        private DataTable nhanvienTable;

        private void NhanVien_Load(object sender, EventArgs e)
        {
            loadGridNhanVien();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT NV.MaNV, NV.TenNV, CL.TenCa, NV.NamSinh, NV.GioiTinh, NV.DiaChi, NV.DienThoai, NV.LuongThang FROM NhanVien AS NV INNER JOIN CaLam AS CL ON NV.MaCa = CL.MaCa;";

                nhanvienTable = DAO.LoadDataToTable(query);

                if (GridNhanvien != null)
                    GridNhanvien.DataSource = nhanvienTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }
        }

        private void loadGridNhanVien()
        {
            string sql = "SELECT NV.MaNV, NV.TenNV, CL.TenCa, NV.NamSinh, NV.GioiTinh, NV.DiaChi, NV.DienThoai, NV.LuongThang FROM NhanVien AS NV INNER JOIN CaLam AS CL ON NV.MaCa = CL.MaCa;";
            DataTable dt = DAO.LoadDataToTable(sql);
            GridNhanvien.DataSource = dt;
            GridNhanvien.Columns[0].HeaderText = "Mã nhân viên";
            GridNhanvien.Columns[1].HeaderText = "Tên nhân viên";
            GridNhanvien.Columns[2].HeaderText = "Ca làm";
            GridNhanvien.Columns[3].HeaderText = "Ngày sinh";
            GridNhanvien.Columns[4].HeaderText = "Giới tính";
            GridNhanvien.Columns[5].HeaderText = "Địa chỉ";
            GridNhanvien.Columns[6].HeaderText = "Điện thoại";
            GridNhanvien.Columns[7].HeaderText = "Lương tháng";
            GridNhanvien.AllowUserToAddRows = false;
        }

        private void GridNhanvien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = GridNhanvien.Rows[e.RowIndex];
                txtmanv.Text = row.Cells["MaNV"].Value?.ToString();
                txttennv.Text = row.Cells["TenNV"].Value?.ToString();
                txtcalam.Text = row.Cells["TenCa"].Value?.ToString();
                txtdiachi.Text = row.Cells["DiaChi"].Value?.ToString();
                mdienthoai.Text = row.Cells["DienThoai"].Value?.ToString();

                if (row.Cells["NamSinh"].Value != DBNull.Value)
                {
                    if (DateTime.TryParse(row.Cells["NamSinh"].Value.ToString(), out DateTime ngaySinhValue))
                    {
                        mngaysinh.Text = ngaySinhValue.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        mngaysinh.Text = "";
                    }
                }
                else
                {
                    mngaysinh.Text = "";
                }

                if (row.Cells["GioiTinh"].Value != DBNull.Value)
                {
                    ckgioitinh.Checked = row.Cells["GioiTinh"].Value.ToString().Equals("Nam", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    ckgioitinh.Checked = false;
                }
                txtluongthang.Text = row.Cells["LuongThang"].Value?.ToString();
                MaNV = txtmanv.Text; // Lưu MaNV của hàng được chọn

                // Sau khi click vào hàng, có thể bật các nút Sửa, Xóa
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                btnThem.Enabled = true; // Vẫn cho phép thêm mới
                btnLuu.Enabled = false; // Không cho phép lưu khi chỉ mới chọn
                EnableInputFields(false); // Khóa các trường để người dùng không sửa lung tung
            }
        }

           private void EnableInputFields(bool enable)
        {
            txtmanv.Enabled = enable;
            txttennv.Enabled = enable;
            txtcalam.Enabled = enable;
            txtdiachi.Enabled = enable;
            mdienthoai.Enabled = enable;
            mngaysinh.Enabled = enable;
            ckgioitinh.Enabled = enable;
            txtluongthang.Enabled = enable;
        }

        private string MaNV = "";

        private void btnThem_Click(object sender, EventArgs e)
        {
            isAddingNew = true;
            MaNV = "";
            txtmanv.Text = "";
            txttennv.Text = "";
            txtcalam.Text = "";
            txtdiachi.Text = "";
            mdienthoai.Text = "";
            mngaysinh.Text = "";
            ckgioitinh.Checked = false;
            txtluongthang.Text = "";
            EnableInputFields(true);
            txtmanv.Enabled = true;
            txtmanv.Focus();
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Validate input fields (giữ nguyên phần này)
            if (string.IsNullOrWhiteSpace(txtmanv.Text) ||
                string.IsNullOrWhiteSpace(txttennv.Text) ||
                string.IsNullOrWhiteSpace(txtcalam.Text) ||
                string.IsNullOrWhiteSpace(txtdiachi.Text) ||
                string.IsNullOrWhiteSpace(mdienthoai.Text.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "")) ||
                string.IsNullOrWhiteSpace(mngaysinh.Text.Replace("/", "").Replace(" ", "")) ||
                string.IsNullOrWhiteSpace(txtluongthang.Text))
            {
                MessageBox.Show("Yêu cầu nhập đủ thông tin!");
                return;
            }

            string maNhanVien = txtmanv.Text.Trim();
            string tenNhanVien = txttennv.Text.Trim();
            string caLam = txtcalam.Text.Trim();
            string diaChi = txtdiachi.Text.Trim();
            string dienThoai = mdienthoai.Text.Trim();
            DateTime ngaySinh;
            bool gioiTinhChecked = ckgioitinh.Checked;
            string gioiTinh = gioiTinhChecked ? "Nam" : "Nữ"; // Lưu vào DB dưới dạng chuỗi "Nam" hoặc "Nữ"
            decimal luongThang;

            if (!DateTime.TryParseExact(mngaysinh.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out ngaySinh))
            {
                MessageBox.Show("Ngày sinh không hợp lệ. Vui lòng nhập theo định dạng DD/MM/YYYY.");
                return;
            }

            if (!decimal.TryParse(txtluongthang.Text, out luongThang))
            {
                MessageBox.Show("Lương tháng không hợp lệ. Vui lòng nhập số.");
                return;
            }

            // Phân biệt chế độ Thêm mới hay Cập nhật
            if (isAddingNew)
            {
                // Chế độ THÊM MỚI
                // Kiểm tra trùng MaNV chỉ khi thêm mới
                DataRow existingRow = nhanvienTable.AsEnumerable().FirstOrDefault(row => row.Field<string>("MaNV").Trim().Equals(maNhanVien, StringComparison.OrdinalIgnoreCase));
                if (existingRow != null)
                {
                    MessageBox.Show("Đã có thông tin nhân viên với Mã Nhân Viên này! Vui lòng nhập lại Mã khác.");
                    txtmanv.Focus();
                    return;
                }

                try
                {
                    string insertQuery = "INSERT INTO NhanVien (MaNV, TenNV, MaCa, DiaChi, DienThoai, NamSinh, GioiTinh, LuongThang) " +
                                         "VALUES (@MaNhanVien, @TenNhanVien, @CaLam, @DiaChi, @DienThoai, @NgaySinh, @GioiTinh, @LuongThang)";

                    SqlCommand cmd = new SqlCommand(insertQuery, DAO.con);

                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);
                    cmd.Parameters.AddWithValue("@TenNhanVien", tenNhanVien);
                    cmd.Parameters.AddWithValue("@CaLam", caLam);
                    cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                    cmd.Parameters.AddWithValue("@DienThoai", dienThoai);
                    cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                    cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh); // Use "Nam" or "Nữ"
                    cmd.Parameters.AddWithValue("@LuongThang", luongThang);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm thông tin nhân viên thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi thêm thông tin nhân viên!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi ghi dữ liệu vào bảng NhanVien: " + ex.Message);
                }
            }
            else
            {
                if (!MaNV.Equals(maNhanVien, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Bạn không được thay đổi Mã Nhân Viên khi đang cập nhật thông tin!");
                    txtmanv.Text = MaNV; // Khôi phục lại MaNV ban đầu
                    txtmanv.Focus();
                    return;
                }

                try
                {
                    string updateQuery = "UPDATE NhanVien SET TenNV = @TenNhanVien, MaCa = @CaLam, DiaChi = @DiaChi, " +
                                         "DienThoai = @DienThoai, NamSinh = @NgaySinh, GioiTinh = @GioiTinh, LuongThang = @LuongThang " +
                                         "WHERE MaNV = @MaNhanVien";

                    SqlCommand cmd = new SqlCommand(updateQuery, DAO.con);

                    cmd.Parameters.AddWithValue("@TenNhanVien", tenNhanVien);
                    cmd.Parameters.AddWithValue("@CaLam", caLam);
                    cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                    cmd.Parameters.AddWithValue("@DienThoai", dienThoai);
                    cmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                    cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                    cmd.Parameters.AddWithValue("@LuongThang", luongThang);
                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin nhân viên thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không có thông tin nhân viên để cập nhật!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật dữ liệu: " + ex.Message);
                }
            }
            isAddingNew = false;
            EnableInputFields(false);
            txtmanv.Enabled = false;
            LoadData();
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmanv.Text))
            {
                MessageBox.Show("Vui lòng chọn một nhân viên để sửa!");
                return;
            }

            isAddingNew = false; // Đặt trạng thái là đang cập nhật

            EnableInputFields(true); // Bật tất cả các trường nhập liệu
            txtmanv.Enabled = false; // KHÔNG cho phép sửa Mã NV khi đang ở chế độ sửa
            txttennv.Focus(); // Di chuyển con trỏ đến tên nhân viên

            // Cập nhật trạng thái các nút
            btnThem.Enabled = false;
            btnSua.Enabled = false; // Không thể "Sửa" khi đang "Sửa"
            btnXoa.Enabled = false;
            btnLuu.Enabled = true; // Bật nút Lưu
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtmanv.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string maNhanVien = txtmanv.Text.Trim();
                try
                {
                    string deleteQuery = "DELETE FROM NhanVien WHERE MaNV = @MaNhanVien";
                    SqlCommand cmd = new SqlCommand(deleteQuery, DAO.con);
                    cmd.Parameters.AddWithValue("@MaNhanVien", maNhanVien);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa thông tin nhân viên thành công!");
                        // Clear fields after deletion
                        txtmanv.Text = "";
                        txttennv.Text = "";
                        txtcalam.Text = "";
                        txtdiachi.Text = "";
                        mdienthoai.Text = "";
                        mngaysinh.Text = "";
                        ckgioitinh.Checked = false;
                        txtluongthang.Text = "";
                        MaNV = ""; // Clear stored MaNV
                        EnableInputFields(true); // Enable fields for new input or further actions
                        LoadData();
                        loadGridNhanVien();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy nhân viên để xóa!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa dữ liệu: " + ex.Message);
                }
            }

        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnsachtruyen_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();
            sachtruyen formSachtruyen = new sachtruyen();
            formSachtruyen.ShowDialog();
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
            khachhang formKH = new khachhang();
            formKH.ShowDialog();
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

        private void btnHuy_Click(object sender, EventArgs e)
        {
            txtmanv.Text = "";
            txttennv.Text = "";
            txtcalam.Text = "";
            txtdiachi.Text = "";
            mdienthoai.Text = "";
            mngaysinh.Text = "";
            ckgioitinh.Checked = false;
            txtluongthang.Text = "";
            MaNV = "";
            btnThem.Enabled = true;
            btnSua.Enabled = true; 
            btnXoa.Enabled = true;
            btnLuu.Enabled = false;
        }
    }
}
