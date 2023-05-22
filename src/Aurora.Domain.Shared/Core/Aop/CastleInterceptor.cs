using Castle.DynamicProxy;

namespace Aurora.Domain.Shared.Core.Aop;

public class CastleInterceptor : AsyncInterceptorBase {
    private readonly IServiceProvider _serviceProvider;
    private IAOPContext _aopContext;
    private List<AOPAttributeBase> _aops = new List<AOPAttributeBase>();

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="serviceProvider"></param>
    public CastleInterceptor(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 进入时
    /// </summary>
    /// <returns></returns>
    private async Task OnEntryAsync() {
        foreach (AOPAttributeBase aAop in _aops) {
            await aAop.OnEntryAsync(_aopContext);
        }
    }

    /// <summary>
    /// 退出时
    /// </summary>
    /// <returns></returns>
    public async Task OnExitAsync() {
        foreach (AOPAttributeBase aAop in _aops) {
            await aAop.OnExitAsync(_aopContext);
        }
    }

    /// <summary>
    /// 执行成功
    /// </summary>
    /// <returns></returns>
    public async Task OnSuccessAsync() {
        foreach (AOPAttributeBase aAop in _aops) {
            await aAop.OnSuccessAsync(_aopContext);
        }
    }

    /// <summary>
    /// 执行异常
    /// </summary>
    public async Task OnExceptionAsync(Exception ex) {
        foreach (AOPAttributeBase aAop in _aops) {
            await aAop.OnExceptionAsync(_aopContext, ex);
        }
    }

    private void Init(IInvocation invocation) {
        _aopContext = new AOPContext(invocation, _serviceProvider);

        _aops = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(AOPAttributeBase), true)
            .Concat(invocation.InvocationTarget.GetType().GetCustomAttributes(typeof(AOPAttributeBase), true))
            .Select(x => (AOPAttributeBase)x)
            .ToList();
    }

    protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed) {

        try {
            Init(invocation);
            await OnEntryAsync();
            await proceed(invocation, proceedInfo);
            await OnSuccessAsync();
        } catch (Exception ex) {
            await OnExceptionAsync(ex);
            throw;
        } finally {
            await OnExitAsync();
        }

        //await Befor();
        //await proceed(invocation, proceedInfo);
        //await After();
    }

    protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed) {
        //Init(invocation);
        //await Befor();
        //TResult result = await proceed(invocation, proceedInfo);
        //invocation.ReturnValue = result;//设置返回值
        //await After();
        //return result;
        try {
            Init(invocation);
            await OnEntryAsync();
            var result = await proceed(invocation, proceedInfo);
            invocation.ReturnValue = result;//设置返回值
            await OnSuccessAsync();
            return result;
        } catch (Exception ex) {
            await OnExceptionAsync(ex);
            return default;
        } finally {
            await OnExitAsync();
        }
    }
}