namespace Aurora.Domain.Shared.Core.Aop;

/// <summary>
/// aop 基础类
/// </summary>
public abstract class AOPAttributeBase : Attribute {
    /// <summary>
    /// 进入时
    /// </summary>
    /// <returns></returns>
    public virtual async Task OnEntryAsync(IAOPContext context) {
        await Task.CompletedTask;
    }

    /// <summary>
    /// 退出时
    /// </summary>
    /// <returns></returns>
    public virtual async Task OnExitAsync(IAOPContext context) {
        await Task.CompletedTask;
    }

    /// <summary>
    /// 执行成功
    /// </summary>
    /// <returns></returns>
    public virtual async Task OnSuccessAsync(IAOPContext context) {
        await Task.CompletedTask;
    }

    /// <summary>
    /// 执行异常
    /// </summary>
    public virtual async Task OnExceptionAsync(IAOPContext context, Exception ex) {
        await Task.CompletedTask;
    }
}