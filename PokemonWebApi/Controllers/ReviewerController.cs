using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApi.Dto;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepo;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepo, IMapper mapper)
        {
            _reviewerRepo = reviewerRepo;
            _mapper = mapper;
        }


        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewerRepo.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            var reviewer = _reviewerRepo.GetReviewer(reviewerId);
            if (reviewer == null)
            {
                return NotFound();
            }
            if (!_reviewerRepo.DeleteReviewer(reviewer))
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
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewerDto)
        {
            if (updatedReviewerDto == null)
            {
                return BadRequest(ModelState);
            }
            if (reviewerId != updatedReviewerDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewerRepo.ReviewerExists(reviewerId))
            {
                ModelState.AddModelError("", "Pokemon Not Found");
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_reviewerRepo.UpdateReviewer(_mapper.Map<Reviewer>(updatedReviewerDto)))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer(ReviewerDto reviewerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewers = _reviewerRepo.GetReviewers()
                .Where(r => (r.LastName.Trim().ToUpper() == reviewerDto.LastName.ToUpper())
                         && (r.FirstName.Trim().ToUpper() == reviewerDto.FirstName.ToUpper()))
                .FirstOrDefault();

            if (reviewers != null)
            {
                ModelState.AddModelError("", "Reviewer already Exists");
                return StatusCode(422, ModelState);
            }
            if (!_reviewerRepo.CreateReviewer(_mapper.Map<Reviewer>(reviewerDto)))
            {
                ModelState.AddModelError("", "Something went wrong saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Created Successfully");
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public IActionResult GetReviewers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepo.GetReviewers());
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepo.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepo.GetReviewer(reviewerId));
            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsbyReviewer(int reviewerId)
        {
            if (!_reviewerRepo.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepo.GetReviewsbyReviewer(reviewerId));
            return Ok(reviews);
        }


    }
}
