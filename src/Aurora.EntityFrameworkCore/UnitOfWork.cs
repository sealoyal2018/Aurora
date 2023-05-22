using Aurora.Domain;
using Aurora.Domain.Shared.DependencyInjections;

using Microsoft.EntityFrameworkCore;

namespace Aurora.EntityFramework;

/// <summary>
/// 工作单元
/// </summary>
public class UnitOfWork : IUnitOfWork, IScopedDependency {
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// 构建一个工作单元
    /// </summary>
    /// <param name="dbContext"></param>
    public UnitOfWork(AppDbContext dbContext) {
        this._dbContext = dbContext;
        IsOpenedTransaction = false;
    }

    public bool IsOpenedTransaction { get; private set; }

    /// <inheritdoc />
    public void BeginTransaction(System.Data.IsolationLevel isolationLevel) {
        _dbContext.Database.BeginTransaction(isolationLevel);
        IsOpenedTransaction = true;
    }

    /// <inheritdoc />
    public async Task BeginTransactionAsync(System.Data.IsolationLevel isolationLevel) {
        await _dbContext.Database.BeginTransactionAsync(isolationLevel);
        IsOpenedTransaction = true;
    }

    /// <inheritdoc />
    public void CommitTransaction() {
        _dbContext.Database.CommitTransaction();
        IsOpenedTransaction = false;
    }

    /// <inheritdoc />
    public async Task CommitTransactionAsync() {
        await _dbContext.Database.CommitTransactionAsync();
        IsOpenedTransaction = false;
    }

    /// <inheritdoc />
    public void RollbackTransaction() {
        if (IsOpenedTransaction)
            _dbContext.Database.RollbackTransaction();
        IsOpenedTransaction = false;
    }

    /// <inheritdoc />
    public async Task RollbackTransactionAsync() {
        if (IsOpenedTransaction)
            await _dbContext.Database.RollbackTransactionAsync();
        IsOpenedTransaction = false;
    }

}
