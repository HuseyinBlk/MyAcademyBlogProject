using Blogy.DataAccess.Context;
using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blogy.DataAccess.Repositories.CommentRepositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<Comment>> GetCommentsWithBlogAndUserAsync()
    {
        var comments = await _table.Include(u => u.User).Include(b => b.Blog).ToListAsync();
        return comments;
    }
}