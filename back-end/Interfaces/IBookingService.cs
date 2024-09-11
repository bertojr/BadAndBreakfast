using back_end.DataModels;
using back_end.Models;

namespace back_end.Interfaces
{
	public interface IBookingService
	{
        public Task Create(BookingRequest newBooking, List<int> roomIds, List<int> serviceIds);
        public Task<Booking> Update(int id, BookingUpdateRequest updateRequest, List<int> roomIds, List<int> serviceIds);
        public Task<List<Booking>> GetAll();
        public Task<List<Room>> GetAvailableRooms(DateOnly checkInDate, DateOnly checkOutDate);
    }
}

