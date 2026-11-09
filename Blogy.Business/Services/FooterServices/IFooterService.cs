using Blogy.Business.Dtos.FooterDtos;
using Blogy.Business.Services.Common;

namespace Blogy.Business.Services.FooterServices;

public interface IFooterService : IGenericService<ResultFooterDto,UpdateFooterDto,CreateFooterDto>
{
    Task<string> GetFooterSummaryAsync();
}
