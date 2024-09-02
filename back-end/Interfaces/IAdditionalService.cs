using back_end.DataModels;

namespace back_end.Interfaces
{
	public interface IAdditionalService
	{
		public Task<AdditionalService> Create(AdditionalService newService);
        public Task<List<AdditionalService>> GetAll();
        public Task<AdditionalService> Edit(int id, AdditionalService updateService);
        public Task Delete(int id);
    }
}

