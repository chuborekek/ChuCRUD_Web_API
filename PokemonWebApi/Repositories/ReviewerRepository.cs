using Microsoft.EntityFrameworkCore;
using PokemonWebApi.Data;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Repositories
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataDbContext _db;
        private readonly DbSet<Reviewer> _table;

        public ReviewerRepository(DataDbContext db)
        {
            _db = db;
            _table = db.Set<Reviewer>();
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
           _db.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _db.Remove(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _table.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _table.ToList();
        }

        public ICollection<Review> GetReviewsbyReviewer(int reviewerId)
        {
            return _db.Reviews.Where(r=>r.Reviewer.Id==reviewerId).ToList();
        }

        public bool ReviewerExists(int reviewerId)
        {
            return _table.Any(r=>r.Id==reviewerId);
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _db.Update(reviewer);
            return Save();
        }
    }
}
