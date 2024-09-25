using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace back_end.DataModels
{
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [MaxLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria")]
		[EmailAddress(ErrorMessage = "L'email non è valida")]
		public string Email { get; set; }

        [Required(ErrorMessage = "PasswordHash obbligatoria")]
		public string PasswordHash { get; set; }

        [Required(ErrorMessage = "PasswordSalt obbligatoria")]
        public string PasswordSalt { get; set; }

        [Phone(ErrorMessage = "Il numero di telefono non è valido")]
		public string? Cell { get; set; }

        [DataType(DataType.Date)]
		public DateTime? DateOfBirth { get; set; }

        public string? Nationally { get; set; }

        public string? Gender { get; set; }

        public string? Country { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        [MaxLength(10, ErrorMessage = "Il CAP non può superare i 10 caratteri")]
		public string? CAP { get; set; }

        // propietà di navigazione
        public List<Review> Reviews { get; set; } = new List<Review>();
		public List<Role> Roles { get; set; } = new List<Role>();

        [JsonIgnore]
		public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

