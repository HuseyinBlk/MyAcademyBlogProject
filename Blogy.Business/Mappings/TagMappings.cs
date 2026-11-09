using AutoMapper;
using Blogy.Business.Dtos.CommentDtos;
using Blogy.Business.Dtos.TagDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings;

public class TagMappings : Profile
{
    public TagMappings()
    {
        CreateMap<Tag, CreateTagDto>().ReverseMap();
        CreateMap<Tag, UpdateTagDto>().ReverseMap();
        CreateMap<Tag, ResultTagDto>().ReverseMap();

    }
}
