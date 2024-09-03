﻿using back_end.DataModels;
using back_end.Models;

namespace back_end.Interfaces
{
	public interface IBookingService
	{
        public Task<Booking> Create(BookingRequest newBooking, int userId, List<int> roomIds, List<int> serviceIds);
        public Task<Booking> Update(int id, BookingUpdateRequest updateRequest, List<int> roomIds, List<int> serviceIds);
        public Task<List<Booking>> GetAll();
    }
}

