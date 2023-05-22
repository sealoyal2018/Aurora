using Aurora.Domain.Shared.Core.Aop;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Diagnostics;

namespace Aurora.Domain;

public sealed class UnitOfWorkAttribute : AOPAttributeBase {
    private readonly IsolationLevel _isolationLevel;
    private IUnitOfWork? unitOfWork;

    public UnitOfWorkAttribute(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) {
        this._isolationLevel = isolationLevel;
    }

    public override async Task OnEntryAsync(IAOPContext context) {
        this.unitOfWork = context.ServiceProvider?.GetService<IUnitOfWork>();
        Debug.Assert(this.unitOfWork != null);
        await this.unitOfWork.BeginTransactionAsync(this._isolationLevel);
        await base.OnEntryAsync(context);
    }

    public override async Task OnSuccessAsync(IAOPContext context) {
        Debug.Assert(this.unitOfWork != null);
        await this.unitOfWork.CommitTransactionAsync();
        await base.OnSuccessAsync(context);
    }

    public override async Task OnExceptionAsync(IAOPContext context, Exception ex) {
        Debug.Assert(this.unitOfWork != null);
        await this.unitOfWork.RollbackTransactionAsync();
        await base.OnExceptionAsync(context, ex);
    }

}
