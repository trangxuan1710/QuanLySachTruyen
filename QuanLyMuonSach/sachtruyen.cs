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
            EnableInput(true);
            ClearInput();
        }

        private void btsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txttensach.Text))
            {
                MessageBox.Show("Vui lòng chọn sách cần sửa.");
                return;
            }

            thaoTac = "sua";
            EnableInput(true);
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

            if (thaoTac == "them")
            {
                cmd.CommandText = "INSERT INTO SachTruyen (TenSach, MaLoaiSach, MaLinhVuc, MaTG, MaNXB, MaNgonNgu, SoTrang, GiaSach, DonGiaThue, SoLuong, Anh, GhiChu) " +
                                  "VALUES (@TenSach, @MaLoaiSach, @MaLinhVuc, @MaTG, @MaNXB, @MaNgonNgu, @SoTrang, @GiaSach, @DonGiaThue, @SoLuong, @Anh, @GhiChu)";
            }
            else if (thaoTac == "sua")
            {
                cmd.CommandText = "UPDATE SachTruyen SET MaLoaiSach = @MaLoaiSach, MaLinhVuc = @MaLinhVuc, MaTG = @MaTG, MaNXB = @MaNXB, MaNgonNgu = @MaNgonNgu, " +
                                  "SoTrang = @SoTrang, GiaSach = @GiaSach, DonGiaThue = @DonGiaThue, SoLuong = @SoLuong, Anh = @Anh, GhiChu = @GhiChu " +
                                  "WHERE TenSach = @TenSach";
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thao tác Thêm hoặc Sửa.");
                DAO.Close();
                return;
            }

            cmd.Parameters.AddWithValue("@TenSach", txttensach.Text.Trim());
            cmd.Parameters.AddWithValue("@MaLoaiSach", cbtheloai.Text.Trim());
            cmd.Parameters.AddWithValue("@MaLinhVuc", cblinhvuc.Text.Trim());
            cmd.Parameters.AddWithValue("@MaTG", txtmatacgia.Text.Trim());
            cmd.Parameters.AddWithValue("@MaNXB", txtmanxb.Text.Trim());
            cmd.Parameters.AddWithValue("@MaNgonNgu", txtmangonngu.Text.Trim());
            cmd.Parameters.Add("@SoTrang", SqlDbType.Int).Value = soTrang;
            cmd.Parameters.Add("@GiaSach", SqlDbType.Decimal).Value = giaSach;
            cmd.Parameters.Add("@DonGiaThue", SqlDbType.Decimal).Value = donGiaThue;
            cmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = soLuong;

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

            cmd.Parameters.AddWithValue("@GhiChu", string.IsNullOrEmpty(txtghichu.Text) ? DBNull.Value : (object)txtghichu.Text.Trim());

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show(thaoTac == "them" ? "Đã thêm sách." : "Đã cập nhật sách.");
                LoadSach();
                ClearInput();
                EnableInput(false);
                thaoTac = "";
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

        private void bthuy_Click(object sender, EventArgs e)
        {
            ClearInput();
            EnableInput(false);
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

        private void sachtruyen_Load(object sender, EventArgs e)
        {
            LoadSach();

            cbtheloai.Items.Clear();
            cbtheloai.Items.AddRange(new string[]
            {
                "Tâm lí", "Ngôn Tình", "Trinh thám", "Khác"
            });

            cblinhvuc.Items.Clear();
            cblinhvuc.Items.AddRange(new string[]
            {
                "Tâm lí xa hội", "Kinh dị", "Y tế", "Tài chính", "Khác"
            });

            cbtheloai.SelectedIndex = 0;
            cblinhvuc.SelectedIndex = 0;

            EnableInput(false);
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
    }
}
