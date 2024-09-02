using System;
using back_end.DataModels;
using back_end.Interfaces;
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

        public async Task<Booking> Create(Booking newBooking, int userId, List<int> roomIds, List<int> serviceIds)
        {
            // Avvia una nuova transazione asincrona per garantire che tutte le
            // operazioni nel blocco try siano eseguite come un'unica unità di lavoro atomica.
            // Questo significa che, se si verifica un errore durante il
            // salvataggio delle modifiche al database,
            // tutte le operazioni verranno annullate (rollback) per mantenere
            // la coerenza dei dati.
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var user = await _dbContext.Users.FindAsync(userId);
                if(user == null)
                {
                    throw new ArgumentException($"Utente con ID {userId} non trovato");
                }
                var rooms = await _dbContext.Rooms
                    .Where(r => roomIds.Contains(r.RoomID))
                    .ToListAsync();
                if (rooms.Count != roomIds.Count)
                {
                    throw new ArgumentException($"Una o più camere non trovate");
                }
                var services = await _dbContext.AdditionalServices
                    .Where(s => serviceIds.Contains(s.ServiceID))
                    .ToListAsync();
                if (services.Count != serviceIds.Count)
                {
                    throw new ArgumentException($"Uno o più servizi non trovati");
                }

                newBooking.User = user;
                newBooking.Rooms = rooms;
                newBooking.AdditionalServices = services;
                newBooking.TotalPrice = rooms.Sum(r => r.Price);
                newBooking.Status = "Confermata";
                newBooking.PaymentStatus = "Da pagare";


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

        public async Task<Booking> Update(int bookingId, Booking updateBooking, List<int> roomIds, List<int> serviceIds)
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

                var rooms = await _dbContext.Rooms
                    .Where(r => roomIds.Contains(r.RoomID))
                    .ToListAsync();
                if (rooms.Count != roomIds.Count)
                {
                    throw new ArgumentException($"Una o più camere non trovate");
                }
                var services = await _dbContext.AdditionalServices
                    .Where(s => serviceIds.Contains(s.ServiceID))
                    .ToListAsync();
                if (services.Count != serviceIds.Count)
                {
                    throw new ArgumentException($"Uno o più servizi non trovati");
                }

                existingBooking.AdditionalServices = services;
                existingBooking.Rooms = rooms;
                existingBooking.CheckInDate = updateBooking.CheckInDate;
                existingBooking.CheckOutDate = updateBooking.CheckOutDate;
                existingBooking.NumberOfGuests = updateBooking.NumberOfGuests;
                existingBooking.PaymentStatus = updateBooking.PaymentStatus;
                existingBooking.SpecialRequests = updateBooking.SpecialRequests;
                existingBooking.Status = updateBooking.Status;
                existingBooking.TotalPrice = rooms.Sum(r => r.Price);

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
    }
}

