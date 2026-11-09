using AutoMapper;
using Blogy.Business.Dtos.SocialDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings;

public class SocialMappings : Profile
{
    public SocialMappings()
    {
        CreateMap<Social, ResultSocialDto>().ReverseMap();
        CreateMap<Social, UpdateSocialDto>().ReverseMap();
        CreateMap<Social, CreateSocialDto>().ReverseMap();
    }
}
