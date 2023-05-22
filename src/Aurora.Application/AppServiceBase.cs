using Aurora.Application.Contract;
using Aurora.Domain;
using Aurora.Domain.Shared;
using Aurora.Domain.Shared.Core;
using Aurora.Domain.Shared.Extensions;
using System.Linq.Expressions;

namespace Aurora.Application;

/// <summary>
/// 通用应用程序服务实现
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class AppServiceBase<TEntity> : IAppService<TEntity>
    where TEntity : class, IEntity {
    /// <summary>
    /// 仓储
    /// </summary>
    public IRepository Repository { get; }

    /// <summary>
    /// 当前查询数据
    /// </summary>
    /// <returns></returns>
    public IQueryable<TEntity> GetQueryable() => Repository.GetQueryable<TEntity>();

    /// <summary>
    /// 构建通用应用程序服务
    /// </summary>
    /// <param name="repository">仓储</param>
    public AppServiceBase(IRepository repository) {
        this.Repository = repository;
    }

    /// <inheritdoc />
    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate) {
        return await Repository.FindAsync(predicate);
    }

    /// <inheritdoc />
    public virtual async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate) {
        return await Repository.GetListAsync(predicate);
    }

    /// <inheritdoc />
    public virtual async Task UpdateAsync(IEnumerable<TEntity> entities) {
        await Repository.UpdateAsync(entities);
    }

    /// <inheritdoc />
    public virtual async Task UpdateAsync(TEntity entity) {
        await Repository.UpdateAsync(entity);
    }

    /// <inheritdoc />
    public virtual async Task UpdateAsync(Expression<Func<TEntity, bool>> predicate) {
        await Repository.UpdateAsync(predicate);
    }

    /// <inheritdoc />
    public virtual async Task DeleteAsync(IEnumerable<TEntity> entities) {
        await Repository.DeleteAsync(entities);
    }

    /// <inheritdoc />
    public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate) {
        await Repository.DeleteAsync(predicate);
    }

    /// <inheritdoc />
    public virtual async Task InsertAsync(TEntity entity) {
        await Repository.InsertAsync(entity);
    }

    /// <inheritdoc />
    public virtual async Task InsertAsync(IEnumerable<TEntity> entities) {
        await Repository.InsertAsync(entities);
    }

    protected void SetDataEditable<T>(ICurrentUser currentUser, WebConfig webConfig, params T[] data)
        where T: EntityBase, IDataEditable {
        var primaryRoleId = webConfig.PrimaryRoleId;
        var isPrimeryRole =  primaryRoleId.IsIn(currentUser.RoleIds);

        foreach(var item in data) {

            if (isPrimeryRole) {
                item.IsEditable = true;
                continue;
            }

            item.IsEditable = currentUser.UserId == item.CreatorId;
        }
    }

}