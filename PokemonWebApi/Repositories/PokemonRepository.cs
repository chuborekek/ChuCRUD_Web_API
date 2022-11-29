using Microsoft.EntityFrameworkCore;
using PokemonWebApi.Data;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataDbContext _db;
        private readonly DbSet<Pokemon> _table;
        public PokemonRepository(DataDbContext db)
        {
            _db= db;
            _table = db.Set<Pokemon>();
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = _db.Owners.Where(o=>o.Id==ownerId).FirstOrDefault();
            var category = _db.Categories.Where(c=>c.Id==categoryId).FirstOrDefault();


            var pokemonOwner = new PokemonOwner()
            {
                Owner = owner,
                Pokemon = pokemon
            };
            _db.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };
            _db.Add(pokemonCategory);
            _db.Add(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
           _db.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _table.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _table.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var reviews = _db.Reviews.Where(p=>p.Pokemon.Id== pokeId);
            var reviewsCount = reviews.Count();
            if(reviewsCount<=0) 
                return 1;
            return ((decimal)reviews.Sum(p => p.Rating)/reviewsCount);

        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _table.OrderBy(p=>p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            return _table.Any(p=>p.Id==pokeId); 
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _db.Update(pokemon);
            return Save();
        }
    }
}
