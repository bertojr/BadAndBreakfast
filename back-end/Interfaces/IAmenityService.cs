using System;
using back_end.DataModels;

namespace back_end.Interfaces
{
	public interface IAmenityService
	{
        public Task<Amenity> Create(Amenity newAmenity);
        public Task<List<Amenity>> GetAll();
        public Task<Amenity> Edit(int id, Amenity updateAmenity);
        public Task Delete(int id);
    }
}

