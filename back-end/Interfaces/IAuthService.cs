using System;
using back_end.DataModels;

namespace back_end.Interfaces
{
	public interface IAuthService
	{
		public Task<(string token, User user)> Login(string email, string password);
		public Task<User> Register(User user);
	}
}

