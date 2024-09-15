using Application.Extensions;
using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbSet<TEntity> _dbSet;
        private readonly IClaimService _claimService;
        public GenericRepository(AppDbContext context, IClaimService claimService)
            => (_dbSet, _claimService) = (context.Set<TEntity>(), claimService);

        public async Task<Pagination<TEntity>> GetAllAsync(int pageIndex = 1, int pageSize = 10, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {

            var query = _dbSet.AsQueryable();

            if (include is not null)
            {
                query = include(query);
            }

            var result = new Pagination<TEntity>();

            var list = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            if (list is not null)
            {
                result = new Pagination<TEntity>
                {
                    Items = list,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalItemsCount = list.Count
                };
            }

            return result;
        }

        public async Task<TEntity?> GetByIdAsync(object Id) => await _dbSet.FindAsync(Id);

        public async Task AddEntityAsync(TEntity entity)
        {
            entity.CreationDate = DateTime.UtcNow;
            var createdBy = string.IsNullOrWhiteSpace(_claimService.GetCurrentUserName) ? null : _claimService.GetCurrentUserName;
            entity.CreatedBy = createdBy;
            await _dbSet.AddAsync(entity);
        }

        public async void AddRangeAsync(List<TEntity> entities)
        {
            entities.ForEach(x =>
            {
                x.CreationDate = DateTime.UtcNow;
                x.CreatedBy = _claimService.GetCurrentUserName;
            });
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            entity.ModificationDate = DateTime.UtcNow;
            entity.ModificatedBy = _claimService.GetCurrentUserName;
            _dbSet.Update(entity);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            entities.ForEach(x =>
            {
                x.ModificationDate = DateTime.UtcNow;
                x.ModificatedBy = _claimService.GetCurrentUserName;
            });
            _dbSet.UpdateRange(entities);
        }

        public void Remove(TEntity entity) => _dbSet.Remove(entity);

        public void RemoveRange(List<TEntity> entities) => _dbSet.RemoveRange(entities);

        public void SoftRemove(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletionDate = DateTime.UtcNow;
            entity.DeletedBy = _claimService.GetCurrentUserName;
            _dbSet.Update(entity);
        }

        public void SoftRemoveRange(List<TEntity> entities)
        {
            entities.ForEach(x =>
            {
                x.DeletionDate = DateTime.UtcNow;
                x.DeletedBy = _claimService.GetCurrentUserName;
            });
            _dbSet.RemoveRange(entities);
        }

        public async Task<TEntity?> GetEntityByCondition(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
        {
            var query = _dbSet.AsQueryable();

            if (include is not null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task<Pagination<TEntity>> Filter(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null, int pageIndex = 1, int pageSize = 10, Expression<Func<TEntity, object>>? sortColumn = null, SortDirectionEnum sortDirection = SortDirectionEnum.Descending)
        {
            var query = _dbSet.AsQueryable();

            if (include is not null)
            {
                query = include(query);
            }

            if (expression is not null)
            {
                query = query.Where(expression);
            }

            if (sortColumn is not null)
            {
                query = sortDirection == SortDirectionEnum.Ascending
                    ? query.OrderBy(sortColumn)
                    : query.OrderByDescending(sortColumn);
            }

            var list = await query
                              .Skip((pageIndex - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

            var result = new Pagination<TEntity>
            {
                Items = list,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemsCount = list.Count
            };
            return result;
        }
        public IQueryable<TEntity> GetQueryable() => _dbSet.AsQueryable();

    }
}
