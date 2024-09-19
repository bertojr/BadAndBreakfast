using back_end.DataModels;

namespace back_end.Models
{
	public class AuthResult
	{
		public string Token { get; set; }
        public User User { get; set; }
    }
}

