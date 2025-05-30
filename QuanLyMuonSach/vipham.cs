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
using QuanLyMuonSach;

namespace QuanLyMuonSach
{
    public partial class vipham : Form
    {
        public vipham()
        {
            InitializeComponent();
        }

        private void vipham_Load(object sender, EventArgs e)
        {
            try
            {
                DAO.Connect();
                loadDataToGridView();
                txtmavipham.Enabled = false;
                btluu.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void loadDataToGridView()
        {
            string sql = "SELECT MaViPham,TenViPham FROM ViPham";
            DataTable dt = DAO.LoadDataToTable(sql);
            datavipham.DataSource = dt;
            datavipham.Columns[0].HeaderText = "Mã vi phạm";
            datavipham.Columns[1].HeaderText = "Tên vi phạm";
            datavipham.Columns[0].Width = 150;
            datavipham.Columns[1].Width = 200;
            datavipham.AllowUserToAddRows = false;
            datavipham.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void datavipham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btthem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtmavipham.Focus();
                return;
            }
            if (datavipham.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để hiển thị");
                return;
            }
            txtmavipham.Text = datavipham.CurrentRow.Cells[0].Value.ToString();
            txttenvipham.Text = datavipham.CurrentRow.Cells[1].Value.ToString();

        }
        private void clear()
        {

            txtmavipham.Enabled = true;
            txtmavipham.Text = "";
            txttenvipham.Text = "";
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

            if (checkKey(txtmavipham.Text.Trim()))
            {
                MessageBox.Show("Mã này đã tồn tại. Vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmavipham.Focus();
                return;
            }

            string sql = "INSERT INTO ViPham (MaViPham, TenViPham) " +
                         "VALUES ('" + txtmavipham.Text.Trim() + "', " +
                         "N'" + txttenvipham.Text.Trim() + "')";

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
            if (txtmavipham.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã vi phạm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtmavipham.Focus();
                return false;
            }
            if (txttenvipham.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên vi phạm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txttenvipham.Focus();
                return false;
            }
            return true;
        }
        private bool checkKey(string key)
        {
            string sql = "SELECT COUNT(*) FROM ViPham WHERE MaViPham = '" + key + "'";
            SqlCommand sqlCommand = new SqlCommand(sql, DAO.con);
            int count = (int)sqlCommand.ExecuteScalar();
            return count > 0;
        }

        private void btsua_Click(object sender, EventArgs e)
        {
            string sql = "UPDATE ViPham SET TenViPham = N'" + txttenvipham.Text.Trim() +
                         "' WHERE MaViPham = '" + txtmavipham.Text.Trim() + "'";
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
            string sql = "DELETE FROM ViPham WHERE MaViPham = '" + txtmavipham.Text.Trim() + "'";
            if (datavipham.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xóa");
                return;
            }
            if (txtmavipham.Text.Trim() == "")
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

        private void btthoat_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();
            sachtruyen formSachtruyen = new sachtruyen();
            formSachtruyen.ShowDialog();
            this.Close();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();
            MuonSach formMuonsach = new MuonSach();
            formMuonsach.ShowDialog();
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();
            khachhang formKH = new khachhang();
            formKH.ShowDialog();
            this.Close();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();
            vipham formVipham = new vipham();
            formVipham.ShowDialog();
            this.Close();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();
            thongke formThongke = new thongke();
            formThongke.ShowDialog();
            this.Close();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            DAO.Close();
            this.Hide();
            NhanVien formNhanvien = new NhanVien();
            formNhanvien.ShowDialog();
            this.Close();
        }
    }
}
