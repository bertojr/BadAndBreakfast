using back_end.DataModels;

namespace back_end.Interfaces
{
	public interface IRoomService
	{
        public Task<Room> Create(Room newRoom, List<int> amenitiesIds);
        public Task<Room> Update(int id, Room updateRoom);
        public Task<List<Room>> GetAll();
        public Task<Amenity> AddAmenityToRoom(int roomId, int amenityId);
        public Task RemoveAmenityFromRoom(int roomId, int amenityId);
        public Task<Room> GetById(int id);
    }
}

