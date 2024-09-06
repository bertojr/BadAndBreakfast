using System;
using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
	public class RegisterModel
	{
        [MaxLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria")]
        [EmailAddress(ErrorMessage = "L'email non è valida")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password obbligatoria")]
        public string Password { get; set; }

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
    }
}

