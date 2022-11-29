using Microsoft.EntityFrameworkCore;
using PokemonWebApi.Data;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataDbContext _db;
        private readonly DbSet<Category> _table;
        public CategoryRepository(DataDbContext db)
        {
            _db = db;
            _table= db.Set<Category>();
        }
        public bool CategoryExists(int categoryId)
        {
            return _table.Any(c => c.Id == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _db.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _db.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _table.ToList();
        }

        public Category GetCategory(int id)
        {
            return _table.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
        {
            return _db.PekomonCategories.Where(c => c.CategoryId == categoryId).Select(p=>p.Pokemon).ToList();
        }

        public bool Save()
        {
            var saved=_db.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _db.Update(category);
            return Save();
        }
    }
}
