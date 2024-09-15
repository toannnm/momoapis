using Application.Extensions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Interfaces.IRepositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task AddEntityAsync(TEntity entity);
        void AddRangeAsync(List<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(List<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void SoftRemove(TEntity entity);
        void SoftRemoveRange(List<TEntity> entities);
        Task<TEntity?> GetByIdAsync(object Id);
        Task<Pagination<TEntity>> GetAllAsync(int pageIndex = 1, int pageSize = 10, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
        Task<Pagination<TEntity>> Filter(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, int pageIndex = 1, int pageSize = 10, Expression<Func<TEntity, object>>? sortColumn = null, SortDirectionEnum sortDirection = SortDirectionEnum.Descending);
        Task<TEntity?> GetEntityByCondition(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null);
        IQueryable<TEntity> GetQueryable();
    }
}