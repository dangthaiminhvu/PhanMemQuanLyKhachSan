using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKS__Luxury.View
{
    public interface IBookingView
    {
        string BookingCode { get; }
        string IDNumber { get; }

        void SetCustomerName(string name);
        void SetRoomID(string roomId);
        void SetPhone(string phone);
        void SetRoomType(string type);
        void SetEmail(string email);
        void SetCheckDates(string dates);
        void SetSpecialRequest(string req);
        void SetPrice(string price);

        event EventHandler SearchClicked;
    }

}
