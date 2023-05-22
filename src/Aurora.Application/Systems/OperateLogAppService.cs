using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.OperateLogDtos;
using Aurora.Domain;
using Aurora.Domain.Shared;
using Aurora.Domain.Shared.Core;
using Aurora.Domain.Shared.DependencyInjections;
using Aurora.Domain.Shared.Extensions;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;
using System.Linq.Dynamic.Core;
using LinqKit;

namespace Aurora.Application.Systems; 

public class OperateLogAppService : AppServiceBase<OperateLog>, IOperateLogAppService, ITransientDependency {
    private readonly ICurrentUser _currentUser; 
    
    public OperateLogAppService(IRepository repository, ICurrentUser currentUser) : base(repository) {
        this._currentUser = currentUser;
    }

    public async Task SaveAsync(OperateLog input) {
        input.Id = IdHelper.Get();
        input.CreatedTime = DateTime.Now;
        input.CreatorId = _currentUser?.UserId?? "admin";

        //input.IPAddress = "127.0.0.1";
        await this.InsertAsync(input);
    }

    public async Task<PageResult<OperateLog>> GetPages(OperateLogPageInput input) {
        var whereExpr = LinqHelper.True<OperateLog>()
            .AndIf(input.StartTime.HasValue, t => t.CreatedTime > input.StartTime.Value)
            .AndIf(input.EndTime.HasValue, t => t.CreatedTime < input.EndTime.Value);
        
        if (!input.Keyword.IsNullOrEmpty() && !input.KeywordValue.IsNullOrEmpty()) {
            var expression = input.ConditionType switch {
                ConditionType.Equal => "",
                _ => $@"{input.Keyword}.Contains(@0)",
            };
            
            var newWhere = DynamicExpressionParser.ParseLambda<OperateLog, bool>(
                ParsingConfig.Default, false, expression, input.KeywordValue);
            whereExpr = whereExpr.And(newWhere);
        }
        //input.Sort = new Dictionary<string, PageSortType> {
        //    {nameof(OperateLog.CreatedTime), PageSortType.Desc }
        //};
        return await GetQueryable().Where(whereExpr).ToPageAsync(input);
    }
}