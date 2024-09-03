using System;
using back_end.DataModels;
using back_end.Interfaces;
using back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class RoomService : IRoomService
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<RoomService> _logger;

        public RoomService(ApplicationDbContext dbContext,
            ILogger<RoomService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Room> Create(Room newRoom, List<int> amenitiesIds)
        {
            try
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
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Errore durante la creazione della camera");
                throw new InvalidOperationException($"Non è stato possibile creare la camera, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore generico durante la creazione della camera");
                throw;
            }
        }

        public async Task<List<Room>> GetAll()
        {
            try
            {
                return await _dbContext.Rooms
                    .Include(r => r.Amenities)
                    .Include(r => r.RoomImages)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante il recupero di tutte le camere");
                throw new InvalidOperationException($"Non è stato possibile recuperare le camere, riprovare più tardi.", ex);
            }
        }

        public async Task<Room> Update(int id, Room updateRoom, List<int> amenitiesIds)
        {
            try
            {
                var existingRoom = await _dbContext.Rooms
                    .Include(r => r.Amenities)
                    .Include(r => r.RoomImages)
                    .FirstOrDefaultAsync(r => r.RoomID == id);

                if (existingRoom == null)
                {
                    throw new KeyNotFoundException($"Camera con ID {id} non trovata");
                }

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

                existingRoom.Amenities = amenities;
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
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Errore durante l'aggiornamento della camera");
                throw new InvalidOperationException($"Non è stato possibile aggiornare la camera, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore generico durante la camera della prenotazione");
                throw;
            }
        }
    }
}

