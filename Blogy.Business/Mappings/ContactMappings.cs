using AutoMapper;
using Blogy.Business.Dtos.AboutDtos;
using Blogy.Business.Dtos.ContactDtos;
using Blogy.Entity.Entities;

namespace Blogy.Business.Mappings;

public class ContactMappings :  Profile
{
    public ContactMappings()
    {
        CreateMap<Contact, ResultContactDto>().ReverseMap();
        CreateMap<Contact, CreateContactDto>().ReverseMap();
    }
}
