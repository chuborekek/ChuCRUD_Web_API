using PokemonWebApi.Dto;
using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int countryId);
        Country GetCountryByOwner(int ownerId);
        ICollection<Owner> GetOwnersFromACountry(int countryId);
        bool CountryExists(int countryId);
        bool CreateCountry(Country country);
        bool Save();
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
    }
}
