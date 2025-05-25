using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLySachTruyen;

namespace QuanLyMuonSach
{
    public partial class khachhang : Form
    {
        public khachhang()
        {
            InitializeComponent();
        }

        private void clear()
        {

            txtmakhachhang.Enabled = true;
            txtmakhachhang.Text = "";
            txttenkhachhang.Text = "";
        }
        private void btthem_Click(object sender, EventArgs e)
        {
            clear();
            btthem.Enabled = false;
            btluu.Enabled = true;
            btxoa.Enabled = false;
            btsua.Enabled = false;
        }

        private void btsua_Click(object sender, EventArgs e)
        {
            string sql = "UPDATE KhachHang SET TenKhachHang = N'" + txttenkhachhang.Text.Trim() +
                         "' WHERE MaKhachHang = '" + txtmakhachhang.Text.Trim() + "'";
            SqlCommand cmd = new SqlCommand(sql, DAO.con);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Dữ liệu đã được sửa thành công");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Dữ liệu không được cập nhật vì " + ex.Message);
            }
            loadDataToGridView();
        }

        private void khachhang_Load(object sender, EventArgs e)
        {
            try
            {
                DAO.Connect();
                loadDataToGridView();
                txtmakhachhang.Enabled = false;
                btluu.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void loadDataToGridView()
        {
            string sql = "SELECT MaKhachHang, TenKhachHang FROM KhachHang";
            DataTable dt = DAO.LoadDataToTable(sql);
            datakhachhang.DataSource = dt;
            datakhachhang.Columns[0].HeaderText = "Mã khách hàng";
            datakhachhang.Columns[1].HeaderText = "Tên khách hàng";
            datakhachhang.Columns[0].Width = 150;
            datakhachhang.Columns[1].Width = 200;
            datakhachhang.AllowUserToAddRows = false;
            datakhachhang.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void btxoa_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM KhachHang WHERE MaKhachHang = '" + txtmakhachhang.Text.Trim() + "'";
            if (datakhachhang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xóa");
                return;
            }
            if (txtmakhachhang.Text.Trim() == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào để xóa");
                return;
            }
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand(sql, DAO.con);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa thành công");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xóa vì " + ex.Message);
                }
            }
            loadDataToGridView();
            clear();
        }

        private void btluu_Click(object sender, EventArgs e)
        {
            if (!checkdata())
                return;

            if (checkKey(txtmakhachhang.Text.Trim()))
            {
                MessageBox.Show("Mã này đã tồn tại. Vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmakhachhang.Focus();
                return;
            }

            string sql = "INSERT INTO KhachHang (MaKhachHang, TenKhachHang) " +
                         "VALUES ('" + txtmakhachhang.Text.Trim() + "', " +
                         "N'" + txttenkhachhang.Text.Trim() + "')";

            SqlCommand sqlCommand = new SqlCommand(sql, DAO.con);
            try
            {
                sqlCommand.ExecuteNonQuery();
                loadDataToGridView();
                MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm : " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool checkdata()
        {
            if (txtmakhachhang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmakhachhang.Focus();
                return false;
            }
            if (txttenkhachhang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txttenkhachhang.Focus();
                return false;
            }
            return true;
        }
        private bool checkKey(string key)
        {
            string sql = "SELECT COUNT(*) FROM KhachHang WHERE MaKhachHang = '" + key + "'";
            SqlCommand sqlCommand = new SqlCommand(sql, DAO.con);
            int count = (int)sqlCommand.ExecuteScalar();
            return count > 0;
        }

        private void btthoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void datakhachhang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btthem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmakhachhang.Focus();
                return;
            }
            if (datakhachhang.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để hiển thị");
                return;
            }
            txtmakhachhang.Text = datakhachhang.CurrentRow.Cells[0].Value.ToString();
            txttenkhachhang.Text = datakhachhang.CurrentRow.Cells[1].Value.ToString();

        }
    }
}
