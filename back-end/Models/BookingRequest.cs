using System;
using back_end.DataModels;

namespace back_end.Models
{
	public class BookingRequest
	{
        public Booking Booking { get; set; }
        public int UserId { get; set; }
        public List<int> RoomIds { get; set; }
        public List<int> ServiceIds { get; set; }
    }
}

