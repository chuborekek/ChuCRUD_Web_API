﻿using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetReviewers();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewsbyReviewer(int reviewerId);
        bool ReviewerExists(int reviewerId);
        bool CreateReviewer(Reviewer reviewer);
        bool Save();
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
    }
}
