using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace back_end.DataModels
{
	public class Room
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomID { get; set; }

        [Required]
		[Range(1, int.MaxValue, ErrorMessage = "Il numero della stanza deve " +
			"essere un numero positivo")]
		public int RoomNumber { get; set; }

        [Required]
		[MaxLength(50, ErrorMessage = "Il tipo di stanza non può superare i " +
			"50 caratteri")]
		public string RoomType { get; set; }

        [Required]
		[MaxLength(10, ErrorMessage = "La capacità non può superare i 10 caratteri")]
		public string Capacity { get; set; }

        [Required]
		[Range(0, double.MaxValue, ErrorMessage = "Il prezzo deve essere un " +
			"numero positivo")]
		public decimal Price { get; set; }

        [Required]
		[MaxLength(500, ErrorMessage = "La descrizione non deve superare i 500 " +
			"caratteri")]
		public string Description { get; set; }

        public bool IsAvailable { get; set; }

        [Required]
		[MaxLength(50, ErrorMessage = "La dimensione non può superare i 50 caratteri")]
		public string size { get; set; }

        // propietà di navigazione
        public List<RoomImage> RoomImages { get; set; } = new List<RoomImage>();
		public List<Amenitie> Amenities { get; set; } = new List<Amenitie>();
	}
}

