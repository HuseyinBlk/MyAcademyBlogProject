using AutoMapper;
using Blogy.Business.Dtos.UserDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings;

public class UserMappings : Profile
{
    public UserMappings()
    {
        CreateMap<AppUser, RegisterDto>()
            .ForMember(
                dst => dst.Username,
                opt => opt.MapFrom(src => src.UserName))
            .ReverseMap();

        CreateMap<AppUser, ResultUserDto>()
            .ForMember(
                dst => dst.FullName,
                opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
            .ReverseMap();

        CreateMap<AppUser, EditProfileDto>()
            .ForMember(
                dst => dst.ImageUrl,
                opt => opt.MapFrom(src => src.ProfilImageUrl))
            .ReverseMap();
    }
}
