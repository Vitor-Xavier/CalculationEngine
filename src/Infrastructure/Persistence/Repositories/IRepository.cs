using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task Add(TEntity entity);

        Task AddRange(IEnumerable<TEntity> entities);

        Task Delete(TEntity Entity);

        Task DeleteRange(IEnumerable<TEntity> entities);

        Task Edit(TEntity entity);
        
        Task EditRange(IEnumerable<TEntity> entities);
    }
}
