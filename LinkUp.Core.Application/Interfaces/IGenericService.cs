using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IGenericService<Entity, DtoModel>
        where Entity : class
        where DtoModel : class
    {
        Task<DtoModel?> AddAsync(DtoModel dto);
        Task UpdateAsync(DtoModel dto, int id);
        Task DeleteAsync(int id);
        Task<List<DtoModel>> GetAllAsync();
        Task<DtoModel?> GetByIdAsync(int id);
    }
}