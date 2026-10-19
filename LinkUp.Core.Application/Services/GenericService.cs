using AutoMapper;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkUp.Core.Application.Services
{
    public class GenericService<Entity, DtoModel> : IGenericService<Entity, DtoModel>
        where Entity : class
        where DtoModel : class
    {
        private readonly IGenericRepository<Entity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<DtoModel?> AddAsync(DtoModel dto)
        {
            Entity entity = _mapper.Map<Entity>(dto);
            await _repository.AddAsync(entity);
            return _mapper.Map<DtoModel>(entity);
        }

        public virtual async Task UpdateAsync(DtoModel dto, int id)
        {
            Entity entity = _mapper.Map<Entity>(dto);
            await _repository.UpdateAsync(entity, id);
        }

        public virtual async Task DeleteAsync(int id)
        {
            Entity entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                await _repository.DeleteAsync(entity);
            }
        }

        public virtual async Task<List<DtoModel>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<DtoModel>>(entities);
        }

        public virtual async Task<DtoModel?> GetByIdAsync(int id)
        {
            Entity entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<DtoModel>(entity);
        }
    }
}