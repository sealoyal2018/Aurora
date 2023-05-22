using Aurora.Application.Contract.Systems.Dtos.RoleResourceDtos;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems; 

public interface IRoleResourceAppService:  IAppService<RoleResource> {
    
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task SaveAsync(RoleResourceSaveDto model);

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<RoleResourceDto> GetData(string roleId);
    
}