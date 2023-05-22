using Aurora.Application.Contract.Systems.Dtos.OperateLogDtos;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems; 

public interface IOperateLogAppService : IAppService<OperateLog> {

    Task SaveAsync(OperateLog input);

    Task<PageResult<OperateLog>> GetPages(OperateLogPageInput input);
}