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
    public partial class sachtruyen : Form
    {
        public sachtruyen()
        {
            InitializeComponent();
        }

        private void sachtruyen_Load(object sender, EventArgs e)
        {
            try
            {
                DAO.Connect();
                loadDataToGridView();
                txtmasach.Enabled = false;
                btluu.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void loadDataToGridView()
        {
            string sql = "SELECT MaSach, TenSach FROM SachTruyen";
            DataTable dt = DAO.LoadDataToTable(sql);
            datasachtruyen.DataSource = dt;
            datasachtruyen.Columns[0].HeaderText = "Mã sách";
            datasachtruyen.Columns[1].HeaderText = "Tên sách";
            datasachtruyen.Columns[0].Width = 150;
            datasachtruyen.Columns[1].Width = 200;
            datasachtruyen.AllowUserToAddRows = false;
            datasachtruyen.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void datasachtruyen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btthem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmasach.Focus();
                return;
            }
            if (datasachtruyen.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để hiển thị");
                return;
            }
            txtmasach.Text = datasachtruyen.CurrentRow.Cells[0].Value.ToString();
            txttensach.Text = datasachtruyen.CurrentRow.Cells[1].Value.ToString();

        }
        private void clear()
        {

            txtmasach.Enabled = true;
            txtmasach.Text = "";
            txttensach.Text = "";
        }

        private void btthem_Click(object sender, EventArgs e)
        {
            clear();
            btthem.Enabled = false;
            btluu.Enabled = true;
            btxoa.Enabled = false;
            btsua.Enabled = false;
        }

        private void bthuy_Click(object sender, EventArgs e)
        {
            clear();
            btthem.Enabled = true;
            btluu.Enabled = false;
            btsua.Enabled = true;
            btxoa.Enabled = true;
        }

        private void btluu_Click(object sender, EventArgs e)
        {
            if (!checkdata())
                return;

            if (checkKey(txtmasach.Text.Trim()))
            {
                MessageBox.Show("Mã này đã tồn tại. Vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmasach.Focus();
                return;
            }

            string sql = "INSERT INTO SachTruyen (MaSach, TenSach) " +
                         "VALUES ('" + txtmasach.Text.Trim() + "', " +
                         "N'" + txttensach.Text.Trim() + "')";

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
            if (txtmasach.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã sách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmasach.Focus();
                return false;
            }
            if (txttensach.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên sách", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txttensach.Focus();
                return false;
            }
            return true;
        }
        private bool checkKey(string key)
        {
            string sql = "SELECT COUNT(*) FROM SachTruyen WHERE MaSach = '" + key + "'";
            SqlCommand sqlCommand = new SqlCommand(sql, DAO.con);
            int count = (int)sqlCommand.ExecuteScalar();
            return count > 0;
        }

        private void btsua_Click(object sender, EventArgs e)
        {
            string sql = "UPDATE SachTruyen SET TenSach = N'" + txttensach.Text.Trim() +
                         "' WHERE MaSach = '" + txtmasach.Text.Trim() + "'";
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

        private void btxoa_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM Sachtruyen WHERE MaSach = '" + txtmasach.Text.Trim() + "'";
            if (datasachtruyen.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xóa");
                return;
            }
            if (txtmasach.Text.Trim() == "")
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

        private void btthoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
