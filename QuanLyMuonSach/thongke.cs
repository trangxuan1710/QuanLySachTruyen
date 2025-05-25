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
    public partial class thongke : Form
    {
        public thongke()
        {
            InitializeComponent();
        }

        private void thongke_Load(object sender, EventArgs e)
        {
            try
            {
                DAO.Connect();
                bthuy.Enabled = false;
                bttao.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void bttao_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn tháng chưa
            if (cbthang.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một tháng để thống kê.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int thangDuocChon = Convert.ToInt32(cbthang.SelectedItem);
            int namHienTai = DateTime.Now.Year; // Giả định thống kê cho năm hiện tại

            try
            {
                // Truy vấn SQL để lấy dữ liệu thống kê
                string sql = @"
                       SELECT
                        MONTH(ts.NgayThue) AS ThangThue,
                        YEAR(ts.NgayThue) AS NamThue,
                        COUNT(cts.MaSach) AS TongSoLuongSachThue,
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

                SqlCommand sqlCommand = new SqlCommand(sql, DAO.con);
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

                            dtThongKe.Rows.Add(
                                Convert.ToInt32(reader["ThangMuon"]),
                                Convert.ToInt32(reader["NamMuon"]),
                                reader["TongDoanhThu"] != DBNull.Value ? (decimal)reader["TongDoanhThu"] : 0,
                                reader["TheLoaiMuonNhieuNhat"] != DBNull.Value ? reader["TheLoaiMuonNhieuNhat"].ToString() : ""
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
            bthuy.Enabled = false;
            bttao.Enabled = false;
        }
        private void clear()
        {
            cbthang.SelectedIndex = -1; // Đặt lại giá trị của ComboBox
            lbldoanhthu.Text = "";
            lblTheLoaiNhieuNhat.Text = "";
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
    }
}
