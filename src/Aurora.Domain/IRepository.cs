using System.Linq.Expressions;

namespace Aurora.Domain;

/// <summary>
/// 仓储
/// </summary>
public interface IRepository {
    /// <summary>
    /// 获取上下文
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> GetQueryable<TEntity>()
        where TEntity : class, IEntity;

    /// <summary>
    /// 获取满足条件的单个数据
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="tracking">是否追踪数据变化</param>
    /// <returns></returns>
    Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, bool tracking = false)
        where TEntity : class, IEntity;

    /// <summary>
    /// 获取满足条件的列表数据 
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="tracking">是否追踪数据变化</param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetListAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, bool tracking = false)
        where TEntity : class, IEntity;

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="entities">数据</param>
    /// <returns></returns>
    Task UpdateAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class, IEntity;

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="entity">数据</param>
    /// <returns></returns>
    Task UpdateAsync<TEntity>(TEntity entity)
        where TEntity : class, IEntity;

    /// <summary>
    /// 更新满足条件的数据
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <returns></returns>
    Task UpdateAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity;

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="entities">需要删除的数据</param>
    /// <returns></returns>
    Task DeleteAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class, IEntity;

    /// <summary>
    /// 删除满足条件的数据
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <returns></returns>
    Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity;

    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="entity">数据</param>
    /// <returns></returns>
    Task InsertAsync<TEntity>(TEntity entity)
        where TEntity : class, IEntity;

    /// <summary>
    /// 批量添加数据
    /// </summary>
    /// <param name="entities">数据</param>
    /// <returns></returns>
    Task InsertAsync<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class, IEntity;

    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class, IEntity;
}
