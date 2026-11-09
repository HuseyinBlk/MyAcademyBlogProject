using AutoMapper;
using Blogy.Business.Dtos.FooterDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings;

public class FooterMappings : Profile
{
    public FooterMappings()
    {
        CreateMap<Footer,CreateFooterDto>().ReverseMap();
        CreateMap<Footer,UpdateFooterDto>().ReverseMap();
        CreateMap<Footer,ResultFooterDto>().ReverseMap();
    }
}
