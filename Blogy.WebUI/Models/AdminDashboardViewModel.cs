using Blogy.Business.Dtos.BlogDtos;
using Blogy.Business.Dtos.CommentDtos;

namespace Blogy.WebUI.Models;

public class AdminDashboardViewModel
{
    public int TotalBlogCount { get; set; }
    public int TotalCategoryCount { get; set; }
    public int TotalCommentCount { get; set; }
    public int ToxicCommentCount { get; set; }
    public int CleanCommentCount { get; set; }
    public int TotalWriterCount { get; set; }
    public int TotalUserCount { get; set; }

    public List<string> CategoryLabels { get; set; } = [];
    public List<int> CategoryBlogCounts { get; set; } = [];

    public List<ResultBlogDto> RecentBlogs { get; set; } = [];
    public List<ResultCommentDto> RecentComments { get; set; } = [];
}
