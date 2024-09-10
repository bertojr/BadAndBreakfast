using System;
namespace back_end.Models
{
	public class SearchAvailableRoomsRequest
	{
		public DateOnly CheckInDate { get; set; }
		public DateOnly CheckOutDate { get; set; }
		public int Guests { get; set; }
	}
}

