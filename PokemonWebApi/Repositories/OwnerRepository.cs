using Microsoft.EntityFrameworkCore;
using PokemonWebApi.Data;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataDbContext _db;
        private readonly DbSet<Owner> _table;

        public OwnerRepository(DataDbContext db)
        {
            _db = db;
            _table = db.Set<Owner>();
        }

        public bool CreateOwner(Owner owner)
        {
            _db.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _db.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _table.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {

            return _table.Include(c=>c.Country).ToList();
        }

        public ICollection<Owner> GetOwnersOfPokemon(int pokeId)
        {
            return _db.PokemonOwners.Where(p=>p.PokemonId==pokeId).Select(o => o.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _db.PokemonOwners.Where(o=>o.OwnerId==ownerId).Select(p=>p.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _table.Any(o => o.Id == ownerId);
        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _db.Update(owner);
            return Save();
        }
    }
}
