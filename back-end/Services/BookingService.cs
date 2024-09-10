using System;
using back_end.DataModels;
using back_end.Interfaces;
using back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end.Services
{
	public class BookingService : IBookingService
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<BookingService> _logger;

        public BookingService(ApplicationDbContext dbContext,
            ILogger<BookingService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Booking> Create(BookingRequest bookingRequest,
            int userId, List<int> roomIds, List<int> serviceIds)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // recupero l'utente
                var user = await _dbContext.Users.FindAsync(userId);
                if(user == null)
                {
                    throw new ArgumentException($"Utente con ID {userId} non trovato");
                }

                // recupero le camere
                var rooms = await _dbContext.Rooms
                    .Where(r => roomIds.Contains(r.RoomID))
                    .ToListAsync();
                if (rooms.Count != roomIds.Count)
                {
                    throw new ArgumentException($"Una o più camere non trovate");
                }

                // recupero i servizi
                var services = await _dbContext.AdditionalServices
                    .Where(s => serviceIds.Contains(s.ServiceID))
                    .ToListAsync();
                if (services.Count != serviceIds.Count)
                {
                    throw new ArgumentException($"Uno o più servizi non trovati");
                }

                var checkInDate = bookingRequest.CheckInDate;
                var checkOutDate = bookingRequest.CheckOutDate;

                if(checkOutDate <= checkInDate)
                {
                    throw new ArgumentException("La data di check-out deve " +
                        "essere successiva alla data di check-in");
                }

                // calcolo di quanti notti è la prenotazione
                var numberOfNights = (checkOutDate.DayNumber - checkInDate.DayNumber);

                // calcolo il prezzo totale delle camere
                var totalRoomPrice = rooms.Sum(r => r.Price * numberOfNights);
                // calcolo il prezzo totale dei servizi aggiuntivi
                var totalServicePrice = services.Sum(s => s.UnitPrice);

                // totale della prenotazione
                var totalPrice = totalRoomPrice + totalServicePrice;

                var newBooking = new Booking
                {
                    CheckInDate = bookingRequest.CheckInDate,
                    CheckOutDate = bookingRequest.CheckOutDate,
                    TotalPrice = totalPrice,
                    Status = "Confermata",
                    PaymentStatus = "Da pagare",
                    NumberOfGuests = bookingRequest.NumberOfGuests,
                    SpecialRequests = bookingRequest.SpecialRequests,
                    User = user,
                    Rooms = rooms,
                    AdditionalServices = services
                };

                await _dbContext.Bookings.AddAsync(newBooking);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return newBooking;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Errore durante la creazione della prenotazione");
                throw new InvalidOperationException($"Non è stato possibile creare la prenotazione, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore generico durante la creazione della prenotazione");
                throw;
            }
        }

        public async Task<Booking> Update(int bookingId, BookingUpdateRequest updateRequest,
            List<int> roomIds, List<int> serviceIds)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var existingBooking = await _dbContext.Bookings
                    .Include(b => b.User)
                    .Include(b => b.Rooms)
                    .Include(b => b.AdditionalServices)
                    .FirstOrDefaultAsync(b => b.BookingID == bookingId);

                if(existingBooking == null)
                {
                    throw new KeyNotFoundException($"Prenotazione con ID {bookingId} non trovata");
                }

                // recupero delle camere
                var rooms = await _dbContext.Rooms
                    .Where(r => roomIds.Contains(r.RoomID))
                    .ToListAsync();
                if (rooms.Count != roomIds.Count)
                {
                    throw new ArgumentException($"Una o più camere non trovate");
                }

                // recupero dei servizi aggiuntivi
                var services = await _dbContext.AdditionalServices
                    .Where(s => serviceIds.Contains(s.ServiceID))
                    .ToListAsync();
                if (services.Count != serviceIds.Count)
                {
                    throw new ArgumentException($"Uno o più servizi non trovati");
                }

                var checkInDate = updateRequest.CheckInDate;
                var checkOutDate = updateRequest.CheckOutDate;

                if (checkOutDate <= checkInDate)
                {
                    throw new ArgumentException("La data di check-out deve " +
                        "essere successiva alla data di check-in");
                }

                // calcolo di quanti notti è la prenotazione
                var numberOfNights = (checkOutDate.DayNumber - checkInDate.DayNumber);

                // calcolo il prezzo totale delle camere
                var totalRoomPrice = rooms.Sum(r => r.Price * numberOfNights);
                // calcolo il prezzo totale dei servizi aggiuntivi
                var totalServicePrice = services.Sum(s => s.UnitPrice);

                // totale della prenotazione
                var totalPrice = totalRoomPrice + totalServicePrice;

                existingBooking.AdditionalServices = services;
                existingBooking.Rooms = rooms;
                existingBooking.CheckInDate = updateRequest.CheckInDate;
                existingBooking.CheckOutDate = updateRequest.CheckOutDate;
                existingBooking.NumberOfGuests = updateRequest.NumberOfGuests;
                existingBooking.SpecialRequests = updateRequest.SpecialRequests;
                existingBooking.PaymentStatus = updateRequest.PaymentStatus;
                existingBooking.Status = updateRequest.Status;
                existingBooking.TotalPrice = totalPrice;

                _dbContext.Bookings.Update(existingBooking);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
                return existingBooking;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Errore durante l'aggiornamento della prenotazione");
                throw new InvalidOperationException($"Non è stato possibile aggiornare la prenotazione, riprovare più tardi.", dbEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore generico durante la prenotazione della prenotazione");
                throw;
            }
        }

        public async Task<List<Booking>> GetAll()
        {
            try
            {
                return await _dbContext.Bookings
                    .Include(b => b.Rooms)
                    .Include(b => b.AdditionalServices)
                    .Include(b => b.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante il recupero di tutte le prenotazioni");
                throw new InvalidOperationException($"Non è stato possibile recuperare le prenotazioni, riprovare più tardi.", ex);
            }
        }

        public async Task<List<Room>> GetAvailableRooms (DateOnly checkInDate, DateOnly checkOutDate)
        {
            try
            {
                var availableRooms = await _dbContext.Rooms
                    .Where(r => !r.Bookings
                    .Any(b => b.CheckInDate < checkOutDate && b.CheckOutDate > checkInDate))
                    .Where(r => r.IsAvailable == true)
                    .Include(r => r.RoomImages)
                    .Include(r => r.Amenities)
                    .ToListAsync();

                if(availableRooms.Count == 0)
                {
                    throw new KeyNotFoundException("Nessuna camera disponibile per la data selezionata");
                }

                return availableRooms;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Errore durante il recupero delle camere disponibili");
                throw;
            }
        }
    }
}

