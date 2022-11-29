using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApi.Dto;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepo, ICountryRepository countryRepo,IMapper mapper)
        {
            _ownerRepo = ownerRepo;
            _countryRepo = countryRepo;
            _mapper = mapper;
        }


        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_ownerRepo.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var owner = _ownerRepo.GetOwner(ownerId);
            if (owner == null)
            {
                return NotFound();
            }
            if (!_ownerRepo.DeleteOwner(owner))
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
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto updatedOwnerDto)
        {
            if (updatedOwnerDto == null)
            {
                return BadRequest(ModelState);
            }
            if (ownerId != updatedOwnerDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_ownerRepo.OwnerExists(ownerId))
            {
                ModelState.AddModelError("", "Category Not Found");
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_ownerRepo.UpdateOwner(_mapper.Map<Owner>(updatedOwnerDto)))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery]int countryId,[FromBody]OwnerDto ownerDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var owner = _ownerRepo.GetOwners()
                .Where(o=>o.Name.Trim().ToUpper()==ownerDto.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if(owner != null)
            {
                ModelState.AddModelError("", "Name already Exist");
                return StatusCode(422, ModelState);
            }
            var ownerMap = _mapper.Map<Owner>(ownerDto);
            ownerMap.Country = _countryRepo.GetCountry(countryId);
            if (!_ownerRepo.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("","Something went wrong saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepo.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(OwnerDto))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepo.OwnerExists(ownerId))
                return NotFound();

            var owner = _mapper.Map<OwnerDto>(_ownerRepo.GetOwner(ownerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersOfPokemon(int pokeId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepo.GetOwnersOfPokemon(pokeId));
           
            return Ok(owners);
        }

        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepo.GetPokemonByOwner(ownerId));

            return Ok(pokemons);
        }

    }
}
