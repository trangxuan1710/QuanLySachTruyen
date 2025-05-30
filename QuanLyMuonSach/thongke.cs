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

namespace QuanLyMuonSach
{
    public partial class thongke : Form
    {
        public thongke()
        {
            InitializeComponent();
            // Đảm bảo cbthang được điền các giá trị số nguyên (từ 1 đến 12)
            // Điều này rất quan trọng để Convert.ToInt32 hoạt động.
            for (int i = 1; i <= 12; i++)
            {
                cbthang.Items.Add(i);
            }
        }

        private void thongke_Load(object sender, EventArgs e)
        {
            try
            {
                DAO.Connect();
                bthuy.Enabled = true;
                bttao.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void bttao_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem một mục có được chọn không
            if (cbthang.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một tháng để thống kê.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Thoát khỏi phương thức nếu không có tháng nào được chọn
            }

            int thangDuocChon;

            // Cố gắng phân tích cú pháp mục đã chọn. Cách này an toàn hơn Convert.ToInt32 trực tiếp.
            if (!int.TryParse(cbthang.SelectedItem.ToString(), out thangDuocChon))
            {
                MessageBox.Show("Giá trị tháng không hợp lệ. Vui lòng chọn lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Thoát nếu phân tích cú pháp thất bại
            }

            int namHienTai = DateTime.Now.Year; // Giả định thống kê cho năm hiện tại

            try
            {
                // Truy vấn SQL để lấy dữ liệu thống kê
                string sql = @"
                        SELECT
                            MONTH(ts.NgayThue) AS ThangThue,
                            YEAR(ts.NgayThue) AS NamThue,
                            COUNT(cts.MaSach) AS TongSoLuongSachThue,
                            SUM(st.DonGiaThue) AS TongDoanhThu, -- Tính tổng doanh thu
                            (
                                SELECT TOP 1 ls.TenLoaiSach
                                FROM ChiTietThueSach ct2
                                INNER JOIN ThueSach ts2 ON ct2.MaThue = ts2.MaThue
                                INNER JOIN SachTruyen st2 ON ct2.MaSach = st2.MaSach
                                INNER JOIN LoaiSach ls ON st2.MaLoaiSach = ls.MaLoaiSach
                                WHERE MONTH(ts2.NgayThue) = @Thang AND YEAR(ts2.NgayThue) = @Nam
                                GROUP BY ls.TenLoaiSach
                                ORDER BY COUNT(*) DESC
                            ) AS TheLoaiMuonNhieuNhat
                        FROM ThueSach ts
                        INNER JOIN ChiTietThueSach cts ON ts.MaThue = cts.MaThue
                        INNER JOIN SachTruyen st ON cts.MaSach = st.MaSach
                        WHERE MONTH(ts.NgayThue) = @Thang AND YEAR(ts.NgayThue) = @Nam
                        GROUP BY MONTH(ts.NgayThue), YEAR(ts.NgayThue);
                ";

                using (SqlCommand sqlCommand = new SqlCommand(sql, DAO.con))
                {
                    sqlCommand.Parameters.AddWithValue("@Thang", thangDuocChon);
                    sqlCommand.Parameters.AddWithValue("@Nam", namHienTai);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Hiển thị doanh thu
                            lbldoanhthu.Text = reader["TongDoanhThu"] != DBNull.Value ? ((decimal)reader["TongDoanhThu"]).ToString("N0") + " VNĐ" : "0 VNĐ";

                            // Hiển thị thể loại mượn nhiều nhất
                            lblTheLoaiNhieuNhat.Text = reader["TheLoaiMuonNhieuNhat"] != DBNull.Value ? reader["TheLoaiMuonNhieuNhat"].ToString() : "Không có dữ liệu";

                            // Tạo DataTable để hiển thị trên DataGridView
                            DataTable dtThongKe = new DataTable();
                            dtThongKe.Columns.Add("Tháng", typeof(int));
                            dtThongKe.Columns.Add("Năm", typeof(int));
                            dtThongKe.Columns.Add("Tổng Doanh Thu", typeof(decimal));
                            dtThongKe.Columns.Add("Thể Loại Mượn Nhiều Nhất", typeof(string));
                            dtThongKe.Columns.Add("Tổng Số Lượng Sách Thuê", typeof(int));

                            dtThongKe.Rows.Add(
                                Convert.ToInt32(reader["ThangThue"]),
                                Convert.ToInt32(reader["NamThue"]),
                                reader["TongDoanhThu"] != DBNull.Value ? (decimal)reader["TongDoanhThu"] : 0,
                                reader["TheLoaiMuonNhieuNhat"] != DBNull.Value ? reader["TheLoaiMuonNhieuNhat"].ToString() : "",
                                reader["TongSoLuongSachThue"] != DBNull.Value ? (int)reader["TongSoLuongSachThue"] : 0
                            );

                            // Gán DataTable cho DataGridView
                            datathongke.DataSource = dtThongKe;
                        }
                        else
                        {
                            lbldoanhthu.Text = "0 VNĐ";
                            lblTheLoaiNhieuNhat.Text = "Không có dữ liệu trong tháng này.";
                            datathongke.DataSource = null; // Xóa dữ liệu cũ trên DataGridView
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi truy vấn dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bthuy_Click(object sender, EventArgs e)
        {
            clear();
           
        }
        private void clear()
        {
            cbthang.SelectedIndex = -1; // Đặt lại giá trị của ComboBox
            lbldoanhthu.Text = "";
            lblTheLoaiNhieuNhat.Text = "";
            datathongke.DataSource = null; // Xóa dữ liệu cũ trên DataGridView
        }
        private void cbthang_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kích hoạt nút "Tạo thống kê" khi người dùng chọn tháng
            if (cbthang.SelectedItem != null)
            {
                bttao.Enabled = true;
                bthuy.Enabled = true;
            }
            else
            {
                bttao.Enabled = false;
                bthuy.Enabled = false;
            }
        }

        private void btthoat_Click(object sender, EventArgs e)
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