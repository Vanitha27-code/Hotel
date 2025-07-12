using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHotelBookingSystem.Models
{
    public class Booking
    {
        public int BookingID { get; set; }          // Primary key
        public int UserID { get; set; }            // Foreign key referencing Users
        public int RoomID { get; set; }            // Foreign key referencing Rooms
        public DateTime CheckInDate { get; set; }  // Date and time of check-in
        public DateTime CheckOutDate { get; set; } // Date and time of check-out
                                                   //public string Status { get; set; }          // Status of the booking (e.g., Confirmed, Pending)
        public string Status { get; set; } = "Active";  // Nullable property

        public int PaymentID { get; set; }

    }
}
