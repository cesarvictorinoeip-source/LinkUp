namespace LinkUp.Core.Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> AddAsync(TEntity entity);
        Task<List<TEntity>?> AddRangeAsync(List<TEntity> entities);
        Task DeleteAsync(int id);
        Task DeleteAsync<Entity>(Entity entity) where Entity : class;
        Task<List<TEntity>> GetAllList();
        Task<List<TEntity>> GetAllListWithInclude(List<string> properties);
        IQueryable<TEntity> GetAllQuery();
        IQueryable<TEntity> GetAllQueryWithInclude(List<string> properties);
        Task<TEntity?> GetById(int id);
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity?> UpdateAsync(int id, TEntity entity);
        Task UpdateAsync<Entity>(Entity entity, int id) where Entity : class;
    }
}