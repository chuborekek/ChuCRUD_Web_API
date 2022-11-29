using Microsoft.EntityFrameworkCore;
using PokemonWebApi.Data;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataDbContext _db;
        private readonly DbSet<Review> _table;

        public ReviewRepository(DataDbContext db)
        {
            _db = db;
            _table = db.Set<Review>();
        }

        public bool CreateReview(Review review)
        {
            _db.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _db.Remove(review);
            return Save();
        }

        public Review GetReview(int reviewId)
        {
            return _table.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _table.ToList();
        }

        public ICollection<Review> GetReviewsofAPokemon(int pokeId)
        {
            return _table.Where(r=>r.Pokemon.Id== pokeId).ToList();
        }

        public bool ReviewExists(int reviewId)
        {
           return _table.Any(r=>r.Id==reviewId);
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _db.Update(review);
            return Save();
        }
    }
}
