using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace back_end.DataModels
{
	public class StripeWebHook
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int WebHookID { get; set; }

        [Required(ErrorMessage = "L'EventID è obbligatorio.")]
        public int EventID { get; set; }

        [Required(ErrorMessage = "Il tipo di evento è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il tipo di evento non può superare i 50 " +
			"caratteri.")]
        public string EventType { get; set; }

        [Required(ErrorMessage = "I dati dell'evento sono obbligatori.")]
        public string EventData { get; set; }

        [Required(ErrorMessage = "La data di ricezione è obbligatoria.")]
        [DataType(DataType.DateTime, ErrorMessage = "La data di ricezione deve " +
            "essere nel formato corretto.")]
        public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;
    }
}

