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
    public partial class sachtruyen : Form
    {
        public sachtruyen()
        {
            InitializeComponent();
            EnableInputs(true);
        }
        private void EnableInputs(bool enable)
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
            pickhachhang.Enabled = enable; // nếu đây là TextBox hoặc Button để chọn ảnh
            txtghichu.Enabled = enable;
        }

        private string thaoTac = "";

        private void btthoat_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dashboard_book formTrangchu = new Dashboard_book();
            formTrangchu.ShowDialog();
            this.Close();
        }

        private void btnmuonsach_Click(object sender, EventArgs e)
        {
            this.Hide();
            MuonSach formMuonsach = new MuonSach();
            formMuonsach.ShowDialog();
            this.Close();
        }

        private void btnkhachhang_Click(object sender, EventArgs e)
        {
            this.Hide();
            khachhang formKhachhang = new khachhang();
            formKhachhang.ShowDialog();
            this.Close();
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

        private void btnnhanvien_Click(object sender, EventArgs e)
        {
            this.Hide();
            NhanVien formNhanvien = new NhanVien();
            formNhanvien.ShowDialog();
            this.Close();
        }

        private void btthem_Click(object sender, EventArgs e)
        {
            thaoTac = "them";
            EnableInputs(true);
            ClearInput();
        }

        private void btsua_Click(object sender, EventArgs e)
        {
            if (datakhachhang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một dòng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            thaoTac = "sua"; // Biến trạng thái, bạn có thể dùng nó trong nút Lưu để phân biệt giữa thêm/sửa
            EnableInputs(true); // Cho phép chỉnh sửa các ô nhập
        }

            private void btxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txttensach.Text))
            {
                MessageBox.Show("Vui lòng chọn sách cần xóa.");
                return;
            }

            DAO.Connect();
            string sql = "DELETE FROM SachTruyen WHERE TenSach = @TenSach";
            SqlCommand cmd = new SqlCommand(sql, DAO.con);
            cmd.Parameters.AddWithValue("@TenSach", txttensach.Text);

            try
            {
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Xóa thành công!");
                    LoadSach();
                    ClearInput();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sách để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                DAO.Close();
            }
        }

        private void btluu_Click(object sender, EventArgs e)
        {
            // Kiểm tra mã sách không được để trống
            if (string.IsNullOrWhiteSpace(txtmasach.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sách.");
                return;
            }

            // Kiểm tra các trường số hợp lệ
            if (!int.TryParse(txtsotrang.Text, out int soTrang) ||
                !decimal.TryParse(txtgiasach.Text, out decimal giaSach) ||
                !decimal.TryParse(txtdongiathue.Text, out decimal donGiaThue) ||
                !int.TryParse(txtsoluong.Text, out int soLuong))
            {
                MessageBox.Show("Dữ liệu số không hợp lệ.");
                return;
            }

            DAO.Connect();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = DAO.con;

            // Xác định lệnh SQL tương ứng thao tác
            if (thaoTac == "them")
            {
                cmd.CommandText = @"INSERT INTO SachTruyen 
        (MaSach, TenSach, MaLoaiSach, MaLinhVuc, MaTG, MaNXB, MaNgonNgu, SoTrang, GiaSach, DonGiaThue, SoLuong, Anh, GhiChu)
        VALUES 
        (@MaSach, @TenSach, @MaLoaiSach, @MaLinhVuc, @MaTG, @MaNXB, @MaNgonNgu, @SoTrang, @GiaSach, @DonGiaThue, @SoLuong, @Anh, @GhiChu)";
            }
            else if (thaoTac == "sua")
            {
                cmd.CommandText = @"UPDATE SachTruyen SET 
        TenSach = @TenSach,
        MaLoaiSach = @MaLoaiSach,
        MaLinhVuc = @MaLinhVuc,
        MaTG = @MaTG,
        MaNXB = @MaNXB,
        MaNgonNgu = @MaNgonNgu,
        SoTrang = @SoTrang,
        GiaSach = @GiaSach,
        DonGiaThue = @DonGiaThue,
        SoLuong = @SoLuong,
        Anh = @Anh,
        GhiChu = @GhiChu
        WHERE MaSach = @MaSach";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thao tác Thêm hoặc Sửa.");
                DAO.Close();
                return;
            }

            // Truyền các tham số vào command
            cmd.Parameters.AddWithValue("@MaSach", txtmasach.Text.Trim());
            cmd.Parameters.AddWithValue("@TenSach", txttensach.Text.Trim());
            cmd.Parameters.AddWithValue("@MaLoaiSach", ((ComboboxItem)cbtheloai.SelectedItem).Value);
            cmd.Parameters.AddWithValue("@MaLinhVuc", ((ComboboxItem)cblinhvuc.SelectedItem).Value);
            cmd.Parameters.AddWithValue("@MaTG", txtmatacgia.Text.Trim());
            cmd.Parameters.AddWithValue("@MaNXB", txtmanxb.Text.Trim());
            cmd.Parameters.AddWithValue("@MaNgonNgu", txtmangonngu.Text.Trim());
            cmd.Parameters.Add("@SoTrang", SqlDbType.Int).Value = soTrang;
            cmd.Parameters.Add("@GiaSach", SqlDbType.Decimal).Value = giaSach;
            cmd.Parameters.Add("@DonGiaThue", SqlDbType.Decimal).Value = donGiaThue;
            cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = soLuong;

            // Xử lý ảnh: chuyển ảnh thành mảng byte để lưu
            if (pickhachhang.Image != null)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    pickhachhang.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    cmd.Parameters.Add("@Anh", SqlDbType.VarBinary).Value = ms.ToArray();
                }
            }
            else
            {
                cmd.Parameters.Add("@Anh", SqlDbType.VarBinary).Value = DBNull.Value;
            }

            // Ghi chú (nullable)
            if (string.IsNullOrWhiteSpace(txtghichu.Text))
                cmd.Parameters.AddWithValue("@GhiChu", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@GhiChu", txtghichu.Text.Trim());

            // Thực thi lệnh
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show(thaoTac == "them" ? "Đã thêm sách thành công." : "Đã cập nhật sách thành công.");
                LoadSach();         // Tải lại dữ liệu
                ClearInput();       // Xóa dữ liệu nhập
                EnableInputs(false);// Khóa form
                thaoTac = "";       // Reset thao tác
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
            finally
            {
                DAO.Close();
            }
        }


        private void bthuy_Click(object sender, EventArgs e)
        {
            ClearInput();
            EnableInputs(false);
            thaoTac = "";
        }

        private void ClearInput()
        {
            txttensach.Clear();
            cbtheloai.SelectedIndex = -1;
            cblinhvuc.SelectedIndex = -1;
            txtmatacgia.Clear();
            txtmanxb.Clear();
            txtmangonngu.Clear();
            txtsotrang.Clear();
            txtgiasach.Clear();
            txtdongiathue.Clear();
            txtsoluong.Clear();
            pickhachhang.Image = null;
            txtghichu.Clear();
        }

      

        private void LoadSach()
        {
            DAO.Connect();
            string sql = "SELECT TenSach, MaLoaiSach, MaLinhVuc, MaTG, MaNXB, MaNgonNgu, SoTrang, GiaSach, DonGiaThue, SoLuong, Anh, GhiChu FROM SachTruyen";
            DataTable dt = DAO.LoadDataToTable(sql);

            // Xử lý hiển thị ảnh nếu cần
            if (dt.Columns.Contains("Anh"))
            {
                dt.Columns.Add("HinhAnh", typeof(Image));
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Anh"] != DBNull.Value)
                    {
                        byte[] imgData = (byte[])row["Anh"];
                        using (var ms = new System.IO.MemoryStream(imgData))
                        {
                            row["HinhAnh"] = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        row["HinhAnh"] = null;
                    }
                }
            }

            datakhachhang.DataSource = dt;

            // Ẩn cột ảnh gốc nếu muốn, chỉ hiện "HinhAnh"
            if (datakhachhang.Columns["Anh"] != null)
                datakhachhang.Columns["Anh"].Visible = false;
            if (datakhachhang.Columns["HinhAnh"] != null)
            {
                datakhachhang.Columns["HinhAnh"].HeaderText = "Ảnh";
                datakhachhang.Columns["HinhAnh"].Width = 100;
            }

            DAO.Close();
        }
        public class ComboboxItem
        {
            public string Value { get; set; }
            public string Text { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void sachtruyen_Load(object sender, EventArgs e)
        {
            LoadSach();

            cbtheloai.Items.Clear();
            cbtheloai.Items.Add(new ComboboxItem { Text = "Tâm lí", Value = "TL01" });
            cbtheloai.Items.Add(new ComboboxItem { Text = "Ngôn Tình", Value = "TL02" });
            cbtheloai.Items.Add(new ComboboxItem { Text = "Trinh thám", Value = "TL03" });
            cbtheloai.Items.Add(new ComboboxItem { Text = "Khác", Value = "TL04" });


            cblinhvuc.Items.Clear();
            cblinhvuc.Items.Add(new ComboboxItem { Text = "Tâm lí xã hội", Value = "LV01" });
            cblinhvuc.Items.Add(new ComboboxItem { Text = "Kinh dị", Value = "LV02" });
            cblinhvuc.Items.Add(new ComboboxItem { Text = "Y tế", Value = "LV03" });
            cblinhvuc.Items.Add(new ComboboxItem { Text = "Tài chính", Value = "LV04" });
            cblinhvuc.Items.Add(new ComboboxItem { Text = "Khác", Value = "LV05" });

            cbtheloai.SelectedIndex = 0;
            cblinhvuc.SelectedIndex = 0;

            EnableInputs(false);
        }


        private void datakhachhang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i >= 0 && i < datakhachhang.Rows.Count)
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

                if (datakhachhang.Rows[i].Cells["Anh"].Value != DBNull.Value)
                {
                    byte[] imgData = (byte[])datakhachhang.Rows[i].Cells["Anh"].Value;
                    using (var ms = new System.IO.MemoryStream(imgData))
                    {
                        pickhachhang.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pickhachhang.Image = null;
                }

                txtghichu.Text = datakhachhang.Rows[i].Cells["GhiChu"].Value.ToString();
            }
        }

        private void pickhachhang_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images (*.jpg;*.png;*.gif;*.bmp)|*.jpg;*.png;*.gif;*.bmp|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pickhachhang.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void datakhachhang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtmanxb_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
