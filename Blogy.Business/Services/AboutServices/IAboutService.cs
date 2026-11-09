using Blogy.Business.Dtos.AboutDtos;
using Blogy.Business.Dtos.UserDtos;
using Blogy.Business.Services.Common;
using Blogy.DataAccess.Repositories.GenericRepositories;
using Blogy.Entity.Entities;

namespace Blogy.Business.Services.AboutServices;

public interface IAboutService : IGenericService<ResultAboutDto,UpdateAboutDto,CreateAboutDto>
{
    Task<List<TopWriterDto>> GetTop6WritersAsync();
}
