using AutoMapper;
using Blogy.Business.Dtos.CommentDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings;

public class CommentMapping : Profile
{
    public CommentMapping()
    {
        CreateMap<Comment,CreateCommentDto>().ReverseMap();
        CreateMap<Comment,UpdateCommentDto>().ReverseMap();
        CreateMap<Comment,ResultCommentDto>().ReverseMap();
    }
}
