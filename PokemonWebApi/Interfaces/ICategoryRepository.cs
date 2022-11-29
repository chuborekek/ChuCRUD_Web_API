using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
    public interface ICategoryRepository
    {
        public ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
        bool CategoryExists(int categoryId);
        bool CreateCategory(Category category);
        bool Save();
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
    }
}
