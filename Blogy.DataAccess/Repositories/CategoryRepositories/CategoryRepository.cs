using Blogy.DataAccess.Context;
using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;

namespace Blogy.DataAccess.Repositories.CategoryRepositories;

public class CategoryRepository : GenericRepository<Category> , ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}