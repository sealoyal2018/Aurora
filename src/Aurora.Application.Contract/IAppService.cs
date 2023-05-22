using Aurora.Domain;
using System.Linq.Expressions;

namespace Aurora.Application.Contract;

/// <summary>
/// 应用类
/// </summary>
public interface IAppService<TEntity>
    where TEntity : class, IEntity {
    /// <summary>
    /// 当前领域内的仓储对象
    /// </summary>
    IRepository Repository { get; }

    /// <summary>
    /// 获取满足条件的单个数据
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <returns></returns>
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 获取满足条件的列表数据 
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="entities">数据</param>
    /// <returns></returns>
    Task UpdateAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="entity">数据</param>
    /// <returns></returns>
    public virtual async Task UpdateAsync(TEntity entity) {
        await Repository.UpdateAsync(entity);
    }

    /// <summary>
    /// 更新满足条件的数据
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <returns></returns>
    Task UpdateAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="entities">需要删除的数据</param>
    /// <returns></returns>
    Task DeleteAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// 删除满足条件的数据
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <returns></returns>
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="entity">数据</param>
    /// <returns></returns>
    Task InsertAsync(TEntity entity);

    /// <summary>
    /// 批量添加数据
    /// </summary>
    /// <param name="entities">数据</param>
    /// <returns></returns>
    Task InsertAsync(IEnumerable<TEntity> entities);
}