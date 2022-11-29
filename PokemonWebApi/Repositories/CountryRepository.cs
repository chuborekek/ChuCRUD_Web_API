using Microsoft.EntityFrameworkCore;
using PokemonWebApi.Data;
using PokemonWebApi.Dto;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataDbContext _db;
        private readonly DbSet<Country> _table;

        public CountryRepository(DataDbContext db)
        {
            _db = db;
            _table = db.Set<Country>();
        }
        public bool CountryExists(int countryId)
        {
            return _table.Any(c=>c.Id== countryId);
        }

        public bool CreateCountry(Country country)
        {
            _db.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _db.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _table.AsNoTracking().ToList();
        }

        public Country GetCountry(int countryId)
        {
            return _table.Where(c => c.Id == countryId).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {

            return _db.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromACountry(int countryId)
        {
            return _db.Owners.Where(c => c.Country.Id == countryId).ToList(); 

        }

        public bool Save()
        {
            var saved = _db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _db.Update(country);
            return Save();
        }
    }
}
