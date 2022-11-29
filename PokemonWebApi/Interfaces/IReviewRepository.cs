using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsofAPokemon(int pokeId);
        bool ReviewExists(int reviewId);
        bool CreateReview(Review review);
        bool Save();
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);

    }
}
