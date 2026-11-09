using Blogy.DataAccess.Context;
using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;

namespace Blogy.DataAccess.Repositories.FooterRepositories;

public class FooterRepository : GenericRepository<Footer> ,IFooterRepository
{
    public FooterRepository(AppDbContext context) : base(context)
    {
    }
}