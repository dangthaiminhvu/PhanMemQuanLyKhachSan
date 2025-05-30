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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();

            _connString = ConfigurationManager
            .ConnectionStrings["HotelDb"]
            .ConnectionString;

            txtRoomId.Leave += TxtRoomId_Leave;
            cbbRating.Leave += CbbRating_Leave;
        }

        private readonly string _connString;

        private void TxtRoomId_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomId.Text))
            {
                MessageBox.Show("Bạn phải nhập mã phòng", "Cảnh báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoomId.Focus();
            }
        }
        private void CbbRating_Leave(object sender, EventArgs e)
        {
            if (cbbRating.SelectedIndex < 0)
            {
                MessageBox.Show("Bạn cần chọn đánh giá", "Cảnh báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbRating.DroppedDown = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSendFb_Click(object sender, EventArgs e)
        {
            string roomNo = txtRoomId.Text.Trim();
            if (string.IsNullOrEmpty(roomNo))
            {
                MessageBox.Show("Bạn phải nhập mã phòng.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbbRating.SelectedIndex < 0)
            {
                MessageBox.Show("Bạn cần chọn đánh giá.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string comment = txtComment.Text.Trim();
            if (string.IsNullOrEmpty(comment))
            {
                MessageBox.Show("Bạn cần nhập nội dung phản hồi.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Lấy MaKhachHang và MaKhachSan hiện tại
            int? maKhachHang = null;
            int? maKhachSan = null;
            using (var conn = new MySqlConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT 
                        HD.MaKhachHang,
                        P.MaKhachSan
                    FROM HoaDon HD
                    JOIN Phong P 
                      ON HD.MaPhong = P.MaPhong
                    WHERE P.SoPhong = @roomNo
                      AND P.TrangThaiPhong = 'Đã Sử Dụng'
                    ORDER BY HD.NgayNhanPhong DESC
                    LIMIT 1;
                ";
                cmd.Parameters.AddWithValue("@roomNo", roomNo);

                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        maKhachHang = rdr.GetInt32("MaKhachHang");
                        maKhachSan = rdr.GetInt32("MaKhachSan");
                    }
                }
            }

            if (maKhachHang == null)
            {
                MessageBox.Show(
                    $"Không tìm thấy khách nào hiện tại đang thuê phòng {roomNo}.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Xác nhận gửi
            var dr = MessageBox.Show("Xác nhận gửi phản hồi?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes) return;

            // 3. Insert vào PhanHoi
            using (var conn = new MySqlConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
            INSERT INTO PhanHoi
              (MaKhachHang, MaKhachSan, NoiDung, MucDoHaiLong)
            VALUES
              (@mkh, @mks, @noiDung, @rating);
        ";
                cmd.Parameters.AddWithValue("@mkh", maKhachHang.Value);
                cmd.Parameters.AddWithValue("@mks", maKhachSan.Value);
                cmd.Parameters.AddWithValue("@noiDung", comment);
                cmd.Parameters.AddWithValue("@rating", cbbRating.SelectedIndex + 1);

                conn.Open();
                int n = cmd.ExecuteNonQuery();
                if (n > 0)
                    MessageBox.Show("Gửi phản hồi thành công", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Gửi thất bại. Vui lòng thử lại.", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResetFb_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Xác nhận nhập lại?", "Confirm",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr != DialogResult.Yes) return;

            txtRoomId.Clear();
            cbbRating.SelectedIndex = -1;
            txtComment.Clear();
        }

        private void txtRoomId_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbbRating_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtComment_TextChanged(object sender, EventArgs e)
        {

        }
    }
}