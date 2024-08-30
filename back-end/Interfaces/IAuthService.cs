﻿using System;
using back_end.DataModels;
using back_end.Models;

namespace back_end.Interfaces
{
	public interface IAuthService
	{
		public Task<AuthResult> Login(string email, string password);
		public Task<(User user, string errorMessage)> Register(LoginModel model);
	}
}

