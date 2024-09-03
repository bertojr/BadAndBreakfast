using back_end.DataModels;

namespace back_end.Interfaces
{
	public interface IReviewService
	{
        public Task<Review> Create(int? userId, Review newReview);
        public Task<Review> Update(int id, Review updateReview);
        public Task<List<Review>> GetAll();
    }
}

