using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApi.Dto;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IMapper _mapper;
        private readonly IReviewerRepository _reviewerRepo;
        private readonly IPokemonRepository _pokemonRepo;

        public ReviewController(IReviewRepository reviewRepo,IMapper mapper,
            IReviewerRepository reviewerRepo,
            IPokemonRepository pokemonRepo)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
            _reviewerRepo = reviewerRepo;
            _pokemonRepo = pokemonRepo;
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewRepo.ReviewExists(reviewId))
            {
                return NotFound();
            }
            var review = _reviewRepo.GetReview(reviewId);
            if (review == null)
            {
                return NotFound();
            }
            if (!_reviewRepo.DeleteReview(review))
            {
                ModelState.AddModelError("", "Somewthing went wrong deleting");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }



        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReviewDto)
        {
            if (updatedReviewDto == null)
            {
                return BadRequest(ModelState);
            }
            if (reviewId != updatedReviewDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewRepo.ReviewExists(reviewId))
            {
                ModelState.AddModelError("", "Pokemon Not Found");
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewRepo.UpdateReview(_mapper.Map<Review>(updatedReviewDto)))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return BadRequest(ModelState);
            }
            return NoContent();
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, int pokemonId, [FromBody] ReviewDto reviewDto)
        {
            var reviewer = _reviewerRepo.GetReviewer(reviewerId);
            if (reviewer == null)
            {
                ModelState.AddModelError("", "Reviewer doesn't Exist");
                return StatusCode(422,ModelState);
            }
            var pokemon = _pokemonRepo.GetPokemon(pokemonId);
            if (pokemon == null)
            {
                ModelState.AddModelError("", "Pokemon doesn't Exist");
                return StatusCode(422, ModelState);
            }

            var reviewMap = _mapper.Map<Review>(reviewDto);
            reviewMap.Reviewer = reviewer;
            reviewMap.Pokemon = pokemon;
            if(!_reviewRepo.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong saving.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");


        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepo.GetReviews());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepo.ReviewExists(reviewId))
            {
                return NotFound();
            }
            var review = _mapper.Map<ReviewDto>(_reviewRepo.GetReview(reviewId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(review);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsofAPokemon(int pokeId)
        {
          
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepo.GetReviewsofAPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }



    }
}
