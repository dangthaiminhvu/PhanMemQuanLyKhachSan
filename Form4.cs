using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            txtRoomId.Leave += TxtRoomId_Leave;
            cbbRating.Leave += CbbRating_Leave;
        }
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

            var dr = MessageBox.Show(
                     "Xác nhận gửi?",       // Nội dung
                     "Confirm",             // Tiêu đề
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
            );

            // 2. Nếu chọn Yes thì báo thành công
            if (dr == DialogResult.Yes)
            {
                MessageBox.Show(
                    "Gửi phản hồi thành công",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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


    }
}