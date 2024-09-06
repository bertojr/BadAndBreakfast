using back_end.DataModels;

namespace back_end.Interfaces
{
	public interface IUserService
	{
        public Task<List<User>> GetAll();
        public Task AddRoleToUser(int userId, int roleId);
        public Task<User> GetById(int id);
        //public Task<User> Update(int id, User updateUser);
    }
}

