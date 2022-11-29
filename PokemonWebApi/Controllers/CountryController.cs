using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApi.Dto;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepo;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepo,IMapper mapper)
        {
            _countryRepo = countryRepo;
            _mapper = mapper;
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_countryRepo.CountryExists(countryId))
            {
                return NotFound();
            }
            var category = _countryRepo.GetCountry(countryId);
            if (category == null)
            {
                return NotFound();
            }
            if (!_countryRepo.DeleteCountry(category))
            {
                ModelState.AddModelError("", "Somewthing went wrong deleting");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody]CountryDto updatedCountryDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (countryId != updatedCountryDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_countryRepo.CountryExists(countryId))
            {
                return NotFound();
            }

            var country = _countryRepo.GetCountries()
                .Where(c => c.Name == updatedCountryDto.Name)
                .FirstOrDefault();
          
            if(country!=null && country.Id != countryId)
            {
                ModelState.AddModelError("", "Country name Already Exists");
                return StatusCode(422, ModelState);
            }
            var countryMap = _mapper.Map<Country>(updatedCountryDto);
            if (!_countryRepo.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("","Something went wrong in updating");
                return StatusCode(500,ModelState);
            }
            return StatusCode(204,"successfully updated"); // 204 means NoContent or you can return NoContent();
                

        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody]CountryDto countryDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var country = _countryRepo.GetCountries()
                .Where(c=>c.Name.Trim().ToUpper()==countryDto.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if(country != null) {
                ModelState.AddModelError("", "Country Already Exists.");
                return StatusCode(422, ModelState);
            }
            if (!_countryRepo.CreateCountry(_mapper.Map<Country>(countryDto)))
            {
                ModelState.AddModelError("", "Something went wrong saving");
                return StatusCode(500,ModelState);
            }
            return Ok("Successfully Created.");
        }

       
        [HttpGet]
        [ProducesResponseType(200,Type=typeof(IEnumerable<CountryDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepo.GetCountries());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(countries);
        }
        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_countryRepo.CountryExists(countryId))
            {
                return NotFound();
            }
            var country = _mapper.Map<CountryDto>(_countryRepo.GetCountry(countryId));
            return Ok(country);
            
        }

        [HttpGet("Owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var country = _mapper.Map<CountryDto>(_countryRepo.GetCountryByOwner(ownerId));
            return Ok(country);
        }

        [HttpGet("Owners/{countryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OwnerDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersFromACountry(int countyId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countries = _mapper.Map<List<OwnerDto>>(_countryRepo.GetOwnersFromACountry(countyId));
            return Ok(countries);
        }
    }
}

