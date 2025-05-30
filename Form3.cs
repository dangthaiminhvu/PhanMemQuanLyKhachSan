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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            txtRoomId.Leave += TxtRoomId_Leave;
            cbbServiceType.Leave += CbbServiceType_Leave;
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
        private void CbbServiceType_Leave(object sender, EventArgs e)
        {
            // Với DropDownList, bạn kiểm SelectedIndex
            if (cbbServiceType.SelectedIndex < 0)
            {
                MessageBox.Show("Bạn cần chọn loại dịch vụ", "Cảnh báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbServiceType.DroppedDown = true;   // mở list để chọn luôn
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSend_Click(object sender, EventArgs e)
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
                    "Gửi yêu cầu thành công",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }

        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Xác nhận nhập lại?", "Confirm",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr != DialogResult.Yes) return;

            // Clear các ô và đặt focus về txtRoomId
            txtRoomId.Clear();
            cbbServiceType.SelectedIndex = -1;
            txtNotes.Clear();
            btnReset.Focus();
        }
    }
}