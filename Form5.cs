using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKS__Luxury
{
    public partial class Form5 : Form
    {
        private readonly string _connString;

        public Form5()
        {
            InitializeComponent();
            _connString = ConfigurationManager
                .ConnectionStrings["HotelDb"]
                .ConnectionString;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            // Khởi tạo danh sách trạng thái
            cbxTrangThai.Items.Clear();
            cbxTrangThai.Items.AddRange(new[]
            {
                "Đã Nhận Phòng",
                "Sẵn Sàng Dọn Dẹp",
                "Sẵn Sàng Sử Dụng"
            });
            cbxTrangThai.SelectedIndex = 0;
        }

        private void btnFetchInvoice_Click(object sender, EventArgs e)
        {
            var roomNo = txtRoomID.Text.Trim();
            if (string.IsNullOrEmpty(roomNo))
            {
                MessageBox.Show("Vui lòng nhập số phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new MySqlConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT 
                        KH.HoVaTen,
                        P.SoPhong,
                        KH.CCCD,
                        GROUP_CONCAT(PT.ChiTiet SEPARATOR ', ') AS DvsPhatSinh
                    FROM Phong P
                    JOIN HoaDon HD ON HD.MaPhong = P.MaPhong
                    JOIN KhachHang KH ON HD.MaKhachHang = KH.MaKhachHang
                    LEFT JOIN PhuThu PT ON PT.MaPhong = P.MaPhong AND PT.MaKhachHang = KH.MaKhachHang
                    WHERE P.SoPhong = @roomNo
                    GROUP BY KH.HoVaTen, P.SoPhong, KH.CCCD;
                ";
                cmd.Parameters.AddWithValue("@roomNo", roomNo);

                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        lblCustomerName.Text = rdr.GetString("HoVaTen");
                        lblRoom.Text = rdr.GetString("SoPhong");
                        lblIDNumber.Text = rdr.GetString("CCCD");
                        lblServiceCharges.Text = rdr.IsDBNull(rdr.GetOrdinal("DvsPhatSinh"))
                                                  ? "(không có)"
                                                  : rdr.GetString("DvsPhatSinh");
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Không tìm thấy đặt phòng cho số phòng {roomNo}.",
                            "Kết quả",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                        lblCustomerName.Text = lblRoom.Text = lblIDNumber.Text = lblServiceCharges.Text = "";
                    }
                }
            }
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            var roomNo = txtRoomID.Text.Trim();
            var newStatus = cbxTrangThai.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(roomNo) || string.IsNullOrEmpty(newStatus))
            {
                MessageBox.Show("Vui lòng tìm phòng trước và chọn trạng thái.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new MySqlConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    UPDATE Phong
                    SET TrangThaiPhong = @status
                    WHERE SoPhong = @roomNo;
                ";
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@roomNo", roomNo);

                conn.Open();
                var affected = cmd.ExecuteNonQuery();
                if (affected > 0)
                {
                    MessageBox.Show("Cập nhật trạng thái phòng thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại số phòng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtRoomID.Text = "";
            lblCustomerName.Text = "";
            lblRoom.Text = "";
            lblIDNumber.Text = "";
            lblServiceCharges.Text = "";
            cbxTrangThai.SelectedIndex = 0;
        }

        // Nếu bạn chưa gắn event cho nút thoát:
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {

        }
    }
}
