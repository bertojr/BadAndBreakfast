using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back_end.DataModels
{
	public class AdditionalService
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "Il nome del servizio è obbligatorio")]
        [MaxLength(50, ErrorMessage = "Il nome del servizio non può superare i " +
            "50 caratteri")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "La descrizione del servizio non può " +
            "superare i 500 caratteri")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Il prezzo è obbligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "Il prezzo totale deve essere " +
            "maggiore o uguale a zero.")]
        public decimal UnitPrice { get; set; }

        // propietà di navigazione
        [JsonIgnore]
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

