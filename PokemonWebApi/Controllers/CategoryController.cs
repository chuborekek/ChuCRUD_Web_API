using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApi.Dto;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if(!ModelState.IsValid) 
            {
            return BadRequest(ModelState);
            }
            if(!_categoryRepo.CategoryExists(categoryId)) 
            {
                return NotFound();
            }
            var category = _categoryRepo.GetCategory(categoryId);
            if(category == null) 
            {
                return NotFound();
            }
            if (!_categoryRepo.DeleteCategory(category))
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
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategoryDto) 
        { 
            if(updatedCategoryDto == null)
            {
                return BadRequest(ModelState);
            }
            if (categoryId != updatedCategoryDto.Id)
            {
                return BadRequest(ModelState);
            }
            if(!_categoryRepo.CategoryExists(categoryId))
            {
                ModelState.AddModelError("","Category Not Found");
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_categoryRepo.UpdateCategory(_mapper.Map<Category>(updatedCategoryDto)))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return BadRequest(ModelState);
            }
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if(categoryDto == null)
                return BadRequest(ModelState);
            var category = _categoryRepo.GetCategories()
                .Where(c=>c.Name.Trim().ToUpper() == categoryDto.Name.TrimEnd().ToUpper())
                .FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", "Category Already Exists.");
                return StatusCode(422, ModelState);
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryMap= _mapper.Map<Category>(categoryDto);
            if (!_categoryRepo.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("","Something went wrong saving.");
                return StatusCode(500,ModelState);
            }
            return Ok("Successfully created");

        }


        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepo.GetCategories());
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200,Type=typeof(CategoryDto))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            var category = _mapper.Map<CategoryDto>(_categoryRepo.GetCategory(categoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(category);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200,Type=typeof(IEnumerable<PokemonDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByCategory(int categoryId)
        {
            var pokemonsByCategory = _mapper.Map<List<PokemonDto>>(_categoryRepo.GetPokemonsByCategory(categoryId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemonsByCategory);
        }

    }
}
