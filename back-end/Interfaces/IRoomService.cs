using back_end.DataModels;

namespace back_end.Interfaces
{
	public interface IRoomService
	{
        public Task<Room> Create(Room newRoom, List<int> amenitiesIds);
        public Task<Room> Update(int id, Room updateRoom, List<int> amenitiesIds);
        public Task<List<Room>> GetAll();
    }
}

