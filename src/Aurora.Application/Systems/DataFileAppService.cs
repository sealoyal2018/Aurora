using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.DataFileDtos;
using Aurora.Domain;
using Aurora.Domain.Shared.Core;
using Aurora.Domain.Shared.DependencyInjections;
using Aurora.Domain.Shared.Extensions;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;
using System.Linq.Dynamic.Core;
using LinqKit;

namespace Aurora.Application.Systems; 

public class DataFileAppService: AppServiceBase<DataFile>, IDataFileAppService, ITransientDependency {
    public DataFileAppService(IRepository repository) : base(repository)
    {
    }

    public async Task<PageResult<DataFile>> GetPage(DataFilePageInput input) {
        var expr = input.BuildCondition<DataFile>();
        var whereExpr = LinqHelper.True<DataFile>()
            .AndIf(input.Types.HasValue, t => (int)t.Type == input.Types.Value)
            .AndIf(input.StartTime.HasValue, t => t.CreatedTime > input.StartTime.Value)
            .AndIf(input.EndTime.HasValue, t => t.CreatedTime < input.EndTime.Value);
        if (expr != null) {
            whereExpr = whereExpr.And(expr);
        }
        return await GetQueryable().Where(whereExpr).ToPageAsync(input);
        
    }
}