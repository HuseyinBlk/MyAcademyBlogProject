using AutoMapper;
using Blogy.Business.Dtos.CategoryDtos;
using Blogy.DataAccess.Context;
using Blogy.DataAccess.Repositories.CategoryRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings;

public class CategoryMappings : Profile
{
    public CategoryMappings()
    {
        CreateMap<Category, ResultCategoryDto>().ForMember(dst => dst.Name, o => o.MapFrom(category => category.Name));
        CreateMap<Category, CreateCategoryDto>().ReverseMap();
        CreateMap<Category, UpdateCategoryDto>().ReverseMap();
    }

}
