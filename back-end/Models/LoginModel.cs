using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
	public class LoginModel
	{
        [Required(ErrorMessage = "Email obbligatoria")]
        [EmailAddress(ErrorMessage = "Email non valida")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password obbligatoria")]
        public string password { get; set; }
    }
}

