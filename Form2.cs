using QLKS__Luxury.Presenter;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLKS__Luxury.View;

namespace QLKS__Luxury
{
    public partial class Form2 : Form, IBookingView
    {
        public Form2()
        {
            InitializeComponent();
            new BookingPresenter(this);
        }

        public string BookingCode => txtBookingCode.Text.Trim();
        public string IDNumber => txtIDNumber.Text.Trim();

        public void SetCustomerName(string name)
            => lblCustomerName.Text = name;
        public void SetRoomID(string roomId)
            => lblRoomID.Text = roomId;
        public void SetPhone(string phone)
            => lblPhone.Text = phone;
        public void SetRoomType(string type)
            => lblRoomType.Text = type;
        public void SetEmail(string email)
            => lblEmail.Text = email;
        public void SetCheckDates(string dates)
            => lblCheckDates.Text = dates;
        public void SetSpecialRequest(string req)
            => lblYCDACBIET.Text = req;
        public void SetPrice(string price)
            => lblPrice.Text = price;

        public event EventHandler SearchClicked
        {
            add { btnSearch.Click += value; }
            remove { btnSearch.Click -= value; }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnConfirmCheckIn_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show(
                     "Xác nhận giao phòng?",
                     "Xác nhận",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
            );

            // 2. Nếu chọn Yes thì báo thành công
            if (dr == DialogResult.Yes)
            {
                MessageBox.Show(
                    "Giao phòng thành công",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {

        }

        private void lblCustomerName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblRoomID_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblRoomType_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblCheckDates_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblYCDACBIET_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBookingCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtIDNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
