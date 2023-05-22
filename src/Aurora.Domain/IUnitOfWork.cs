﻿using System.Data;

namespace Aurora.Domain;

/// <summary>
/// 工作单元
/// </summary>
public interface IUnitOfWork {
    /// <summary>
    /// 开始事务
    /// </summary>
    /// <param name="isolationLevel"></param>
    void BeginTransaction(IsolationLevel isolationLevel);

    /// <summary>
    /// 开始事务
    /// </summary>
    /// <param name="isolationLevel"></param>
    /// <returns></returns>
    Task BeginTransactionAsync(IsolationLevel isolationLevel);

    /// <summary>
    /// 提交事务
    /// </summary>
    void CommitTransaction();

    /// <summary>
    /// 提交事务
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// 回滚事务
    /// </summary>
    void RollbackTransaction();

    /// <summary>
    /// 回滚事务
    /// </summary>
    Task RollbackTransactionAsync();
}
