using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace back_end.DataModels
{
	public class Booking
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingID { get; set; }

        [Required(ErrorMessage = "La data di check-in è obbligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La data di check-in deve essere" +
			" nel formato corretto.")]
        public DateOnly CheckInDate { get; set; }

        [Required(ErrorMessage = "La data di check-out è obbligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "La data di check-out deve " +
            "essere nel formato corretto.")]
        public DateOnly CheckOutDate { get; set; }

        [Required(ErrorMessage = "Il prezzo totale è obbligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "Il prezzo totale deve essere " +
            "maggiore o uguale a zero.")]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Lo stato della prenotazione è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Lo stato della prenotazione non può " +
            "superare i 50 caratteri.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Lo stato del pagamento è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Lo stato del pagamento non può superare " +
            "i 50 caratteri.")]
        public string PaymentStatus { get; set; }

        [Required(ErrorMessage = "La data di prenotazione è obbligatoria")]
		public DateTime BookingDate { get; set; } = DateTime.Now;

        [Range(1, int.MaxValue, ErrorMessage = "Il numero di ospiti deve essere " +
            "almeno 1.")]
        public int NumberOfGuests { get; set; }

        [MaxLength(500, ErrorMessage = "Le richieste speciali non possono " +
            "superare i 500 caratteri.")]
        public string? SpecialRequests { get; set; }

        [Required]
        [ForeignKey("UserID")]
		public User User { get; set; }

        public Payment? Payment { get; set; }

        // propietà di navigazione
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<AdditionalService> AdditionalServices { get; set; } =
            new List<AdditionalService>();
    }
}

