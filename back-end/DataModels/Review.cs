using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace back_end.DataModels
{
	public class Review
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ReviewID { get; set; }

        [Required(ErrorMessage = "Il titolo è obbligatorio")]
		[MaxLength(100, ErrorMessage = "Il titolo non può superare i 100 caratteri")]
		public string title { get; set; }

        [Required(ErrorMessage = "La descrizione è obbligatorio")]
        [MaxLength(1000, ErrorMessage = "La descrizione non può superare i 1000 caratteri")]
        public string Description { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime UpdatedDate { get; set; }

        [Required(ErrorMessage = "Il raiting è obbligatorio")]
		[Range(1, 5, ErrorMessage = "Il raiting deve essere un valore tra 1 e 5")]
		public int raiting { get; set; }

        [ForeignKey("UserID")]
		public User? User { get; set; }

		public Review()
		{
			CreatedDate = DateTime.Now;
			UpdatedDate = DateTime.Now;
		}
    }
}

