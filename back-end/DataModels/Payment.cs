using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back_end.DataModels
{
	public class Payment
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }

        [Required(ErrorMessage = "Il StripePaymentID è obbligatorio.")]
        public string StripePaymentID { get; set; }

        [Required(ErrorMessage = "Il StripeChargeID è obbligatorio.")]
        public string StripeChargeID { get; set; }

        [Required(ErrorMessage = "L'importo è obbligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "L'importo deve essere " +
            "maggiore o uguale a zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "La valuta è obbligatoria.")]
        [MaxLength(3, ErrorMessage = "La valuta deve essere composta da tre caratteri.")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Lo stato del pagamento è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Lo stato del pagamento non può superare " +
            "i 50 caratteri.")]
        public string PaymentStatus { get; set; }

        [Required(ErrorMessage = "Il metodo di pagamento è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il metodo di pagamento non può superare " +
            "i 50 caratteri.")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "La data di creazione è obbligatoria.")]
        [DataType(DataType.DateTime, ErrorMessage = "La data di creazione deve " +
            "essere nel formato corretto.")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La data di aggiornamento è obbligatoria.")]
        [DataType(DataType.DateTime, ErrorMessage = "La data di aggiornamento " +
            "deve essere nel formato corretto.")]
        public DateTime UpdateDate { get; set; }

        [ForeignKey("BookingID")]
        [JsonIgnore]
        public Booking Booking { get; set; }
    }
}

