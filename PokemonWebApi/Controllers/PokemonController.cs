using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApi.Dto;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokeRepo;
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepo;
        private readonly ICategoryRepository _categoryRepo;

        public PokemonController(
            IPokemonRepository pokeRepo, IMapper mapper,
            IOwnerRepository ownerRepo,
            ICategoryRepository categoryRepo)
        {
            _pokeRepo = pokeRepo;
            _mapper = mapper;
            _ownerRepo = ownerRepo;
            _categoryRepo = categoryRepo;
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_pokeRepo.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var pokemon = _pokeRepo.GetPokemon(pokeId);
            if (pokemon == null)
            {
                return NotFound();
            }
            if (!_pokeRepo.DeletePokemon(pokemon))
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
        public IActionResult UpdatePokemon(int pokeId,int ownerId,int categoryId, [FromBody] PokemonDto updatedPokemonDto)
        {
            if (updatedPokemonDto == null)
            {
                return BadRequest(ModelState);
            }
            if (pokeId != updatedPokemonDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_pokeRepo.PokemonExists(pokeId))
            {
                ModelState.AddModelError("", "Pokemon Not Found");
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_pokeRepo.UpdatePokemon(ownerId, categoryId, _mapper.Map<Pokemon>(updatedPokemonDto)))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return BadRequest(ModelState);
            }
            return NoContent();
        }



        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery]int ownerId,int categoryId, [FromBody]PokemonDto pokemonDto)
        {
            if(pokemonDto== null)
            {
                return BadRequest(ModelState);
            }
            if(!_ownerRepo.OwnerExists(ownerId))
            {
                ModelState.AddModelError("", "owner does not exist");
                return StatusCode(422,ModelState);
            }
            if (!_categoryRepo.CategoryExists(categoryId))
            {
                ModelState.AddModelError("", "category does not exist");
                return StatusCode(422, ModelState);
            }
            var pokem = _pokeRepo.GetPokemons()
                .Where(p=>p.Name.Trim().ToUpper()== pokemonDto.Name.ToUpper())
                .FirstOrDefault();
            if (pokem != null)
            {
                ModelState.AddModelError("", "pokemon already exist");
                return StatusCode(422, ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonDto);
            if (!_pokeRepo.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong in ssaving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Crated");

        }

        [HttpGet]
        [ProducesResponseType(200,Type=typeof(IEnumerable<PokemonDto>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_pokeRepo.GetPokemons());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }
        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type=typeof(PokemonDto))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if(!_pokeRepo.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokeRepo.GetPokemon(pokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200,Type= typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokeRepo.PokemonExists(pokeId))
                return NotFound();

            var rating = _pokeRepo.GetPokemonRating(pokeId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);

            

        }
    }
}
