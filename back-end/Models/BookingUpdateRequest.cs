using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
    public class BookingUpdateRequest
    {
        [Required(ErrorMessage = "La data di check-in è obbligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La data di check-in deve essere nel formato corretto.")]
        public DateOnly CheckInDate { get; set; }

        [Required(ErrorMessage = "La data di check-out è obbligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La data di check-out deve essere nel formato corretto.")]
        public DateOnly CheckOutDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Il numero di ospiti deve essere almeno 1.")]
        public int NumberOfGuests { get; set; }

        [MaxLength(500, ErrorMessage = "Le richieste speciali non possono superare i 500 caratteri.")]
        public string? SpecialRequests { get; set; }

        [Required(ErrorMessage = "Lo stato della prenotazione è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Lo stato della prenotazione non può superare i 50 caratteri.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Lo stato del pagamento è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Lo stato del pagamento non può superare i 50 caratteri.")]
        public string PaymentStatus { get; set; }

        public List<int> RoomIds { get; set; } = new List<int>();
        public List<int> ServiceIds { get; set; } = new List<int>();
    }
}


