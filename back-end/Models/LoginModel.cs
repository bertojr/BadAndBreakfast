using System;
using System.ComponentModel.DataAnnotations;

namespace back_end.Models
{
	public class LoginModel
	{
        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}

