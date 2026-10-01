namespace Blogy.Business.Services.Common;

public interface IGenericService<TResult,TUpdate,TCreate>
{
    Task<List<TResult>> GetAllAsync();
    Task<TUpdate> GetByIdAsync(int id);
    Task CreateAsync(TCreate entityDto);
    Task UpdateAsync(TUpdate entityDto);
    Task DeleteAsync(int id);
}