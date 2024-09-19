using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
	public class BookingRequest
	{
        public List<int> RoomIds { get; set; }
        public List<int>? ServiceIds { get; set; }
        [Required(ErrorMessage = "La data di check-in è obbligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La data di check-in deve essere" +
            " nel formato corretto.")]
        public DateOnly CheckInDate { get; set; }

        [Required(ErrorMessage = "La data di check-out è obbligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La data di check-out deve " +
            "essere nel formato corretto.")]
        public DateOnly CheckOutDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Il numero di ospiti deve essere " +
            "almeno 1.")]
        public int NumberOfGuests { get; set; }

        [MaxLength(500, ErrorMessage = "Le richieste speciali non possono " +
            "superare i 500 caratteri.")]
        public string? SpecialRequests { get; set; }
    }
}

