using System;
namespace back_end.Interfaces
{
	public interface IServiceExceptionHandler
	{
		public Task<T> ExecuteAsync<T>(Func<Task<T>> operation);
		public Task ExecuteAsync(Func<Task> operation);
	}
}

