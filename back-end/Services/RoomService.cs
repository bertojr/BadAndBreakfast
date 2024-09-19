
using back_end.DataModels;
using back_end.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class RoomService : IRoomService
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly IServiceExceptionHandler _exceptionHandler;

        public RoomService(ApplicationDbContext dbContext, IServiceExceptionHandler exceptionHandler)
        {
            _dbContext = dbContext;
            _exceptionHandler = exceptionHandler;
        }

        // creazione nuova camera con le comodità associate
        public async Task<Room> Create(Room newRoom, List<int> amenitiesIds)
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                List<Amenity> amenities = new List<Amenity>();
                if (amenitiesIds != null && amenitiesIds.Any())
                {
                    amenities = await _dbContext.Amenities
                        .Where(a => amenitiesIds.Contains(a.AmenityID))
                        .ToListAsync();

                    if (amenities.Count != amenitiesIds.Count)
                    {
                        throw new ArgumentException("Una o più comodità non trovate.");
                    }
                }

                newRoom.Amenities = amenities;

                await _dbContext.Rooms.AddAsync(newRoom);
                await _dbContext.SaveChangesAsync();

                return newRoom;
            });
        }


        // recupero tutte le camere
        public async Task<List<Room>> GetAll()
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                return await _dbContext.Rooms
                    .Include(r => r.Amenities)
                    .Include(r => r.RoomImages)
                    .ToListAsync();
            });
        }

        public async Task<Room> GetById(int id)
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                var room = await _dbContext.Rooms
                    .Include(r => r.RoomImages)
                    .Include(r => r.Amenities)
                    .FirstOrDefaultAsync(r => r.RoomID == id);
                if (room == null)
                {
                    throw new KeyNotFoundException($"Camera con ID {id} non trovata.");
                }

                return room;
            });
        }

        public async Task<Amenity> AddAmenityToRoom(int roomId, int amenityId)
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                // recupero la camera
                var room = await GetById(roomId);

                // recupero la comodità da aggiungere
                var amenity = await _dbContext.Amenities
                    .FirstOrDefaultAsync(a => a.AmenityID == amenityId);

                // controllo che la comodità con id sia presente
                if (amenity == null)
                {
                    throw new KeyNotFoundException($"Comodità con ID {amenityId} non trovata.");
                }

                // controllo che non sia gia presente una comodità
                if (room.Amenities.Any(a => a.AmenityID == amenityId))
                {
                    throw new InvalidOperationException($"Comodità: {amenity.Name} già presenta nella camera {room.RoomNumber}");
                }

                room.Amenities.Add(amenity);
                _dbContext.Rooms.Update(room);
                await _dbContext.SaveChangesAsync();

                return amenity;
            });

        }

        public async Task RemoveAmenityFromRoom(int roomId, int amenityId)
        {
            await _exceptionHandler.ExecuteAsync(async () =>
            {
                // recupero la camera
                var room = await GetById(roomId);

                // recupero la comodità da dissociare
                var amenity = await _dbContext.Amenities
                    .FirstOrDefaultAsync(a => a.AmenityID == amenityId);

                // controllo che la comodità con quell'id esista
                if (amenity == null)
                {
                    throw new KeyNotFoundException($"Comodità con ID {amenityId} non trovata.");
                }

                room.Amenities.Remove(amenity);
                _dbContext.Rooms.Update(room);
                await _dbContext.SaveChangesAsync();
            });
        }

        public async Task<Room> Update(int id, Room updateRoom)
        {
            return await _exceptionHandler.ExecuteAsync(async () =>
            {
                // recupero la camera che voglio modificare
                var existingRoom = await GetById(id);

                // aggiorno ogni propietà
                existingRoom.Capacity = updateRoom.Capacity;
                existingRoom.Description = updateRoom.Description;
                existingRoom.IsAvailable = updateRoom.IsAvailable;
                existingRoom.Price = updateRoom.Price;
                existingRoom.RoomNumber = updateRoom.RoomNumber;
                existingRoom.RoomType = updateRoom.RoomType;
                existingRoom.size = updateRoom.size;

                _dbContext.Rooms.Update(existingRoom);
                await _dbContext.SaveChangesAsync();

                return existingRoom;
            });
        }

    }
}

