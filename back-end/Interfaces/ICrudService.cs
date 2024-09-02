namespace back_end.Interfaces
{
	public interface ICrudService<T> where T : class
	{
        public Task<T> Create(T newEntity);
        public Task<List<T>> GetAll();
        public Task<T> Edit(int id, T updateEntity);
        public Task Delete(int id);
    }
}

