using back_end.DataModels;

namespace back_end.Interfaces
{
	public interface IBookingService
	{
        public Task<Booking> Create(Booking newBooking, int userId, List<int> roomIds, List<int> serviceIds);
        public Task<Booking> Update(int id, Booking updateBooking, List<int> roomIds, List<int> serviceIds);
        public Task<List<Booking>> GetAll();
    }
}

