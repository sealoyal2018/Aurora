using Aurora.Domain;
using Aurora.Domain.Shared.DependencyInjections;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Aurora.EntityFramework;

/// <summary>
/// 基础仓储
/// </summary>
public class AppRepository : IRepository, IScopedDependency {
    /// <summary>
    /// 数据库上下文
    /// </summary>
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// 构建一个基础仓储
    /// </summary>
    /// <param name="dbContext">数据库</param>
    public AppRepository(AppDbContext dbContext) {
        this._dbContext = dbContext;
    }

    /// <inheritdoc />
    public async Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class, IEntity {
        _dbContext.Set<TEntity>().RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity {
        var data = _dbContext.Set<TEntity>().Where(predicate);
        _dbContext.Set<TEntity>().RemoveRange(data);
        await _dbContext.SaveChangesAsync();
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, bool tracking = false)
        where TEntity : class, IEntity {
        var dataSet = _dbContext.Set<TEntity>();
        if (tracking)
            dataSet.AsTracking();
        else
            dataSet.AsNoTracking();
        return await dataSet.FirstOrDefaultAsync(predicate);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, bool tracking = false)
        where TEntity : class, IEntity {
        var dataSet = _dbContext.Set<TEntity>();
        if (tracking)
            dataSet.AsTracking();
        else
            dataSet.AsNoTracking();
        return await dataSet.Where(predicate).ToListAsync();
    }

    /// <inheritdoc />
    public IQueryable<TEntity> GetQueryable<TEntity>()
        where TEntity : class, IEntity {
        return _dbContext.Set<TEntity>();
    }

    /// <inheritdoc />
    public async Task InsertAsync<TEntity>(TEntity entity)
        where TEntity : class, IEntity {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task InsertAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class, IEntity {
        await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class, IEntity {
        _dbContext.Set<TEntity>().UpdateRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity {
        var dataSet = _dbContext.Set<TEntity>();
        var data = dataSet.Where(predicate);
        _dbContext.Set<TEntity>().UpdateRange(data);
        await _dbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync<TEntity>(TEntity entity)
        where TEntity : class, IEntity {
        _dbContext.Set<TEntity>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity {
        var dataSet = _dbContext.Set<TEntity>();
        return await dataSet.AnyAsync(predicate);
    }
}