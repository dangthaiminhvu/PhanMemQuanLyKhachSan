using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QLKS__Luxury.View;

namespace QLKS__Luxury.Presenter
{
    public class BookingPresenter
    {
        private readonly IBookingView _view;
        private readonly string _connString;

        public BookingPresenter(IBookingView view)
        {
            _view = view;
            _view.SearchClicked += OnSearchClicked;

            _connString = ConfigurationManager
                .ConnectionStrings["HotelDb"]
                .ConnectionString;
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            using (var conn = new MySqlConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT KH.HoVaTen,
                           P.SoPhong,
                           KH.SoDienThoai,
                           P.LoaiPhong,
                           KH.Email,
                           HD.NgayNhanPhong,
                           GROUP_CONCAT(YCB.NoiDungYeuCau SEPARATOR ', ') AS TatCaYeuCau,
                           HD.TongTien
                    FROM HoaDon HD
                    JOIN KhachHang KH ON HD.MaKhachHang = KH.MaKhachHang
                    JOIN Phong P ON HD.MaPhong = P.MaPhong
                    LEFT JOIN YeuCauDacBiet YCB ON YCB.MaKhachHang = KH.MaKhachHang
                    WHERE KH.MaXacNhan = @bookingCode
                      AND KH.CCCD = @idNumber
                    GROUP BY KH.HoVaTen, P.SoPhong, KH.SoDienThoai, P.LoaiPhong, KH.Email, HD.NgayNhanPhong, HD.TongTien;
                ";
                cmd.Parameters.AddWithValue("@bookingCode", _view.BookingCode.Trim());
                cmd.Parameters.AddWithValue("@idNumber", _view.IDNumber.Trim());

                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        _view.SetCustomerName(rdr.GetString("HoVaTen"));
                        _view.SetRoomID(rdr.GetString("SoPhong"));
                        _view.SetPhone(rdr.GetString("SoDienThoai"));
                        _view.SetRoomType(rdr.GetString("LoaiPhong"));
                        _view.SetEmail(rdr.GetString("Email"));
                        _view.SetCheckDates(rdr.GetDateTime("NgayNhanPhong")
                                                     .ToString("yyyy-MM-dd"));
                        _view.SetSpecialRequest(
                            rdr.IsDBNull(rdr.GetOrdinal("TatCaYeuCau"))
                              ? ""
                              : rdr.GetString("TatCaYeuCau"));
                        _view.SetPrice(
                            rdr.GetDecimal("TongTien")
                               .ToString("N2"));
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Không tìm thấy đặt phòng với mã xác nhận là [{_view.BookingCode}] của khách có CCCD là [{_view.IDNumber}].",
                            "Kết quả tìm kiếm",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
            }
        }
    }
}
