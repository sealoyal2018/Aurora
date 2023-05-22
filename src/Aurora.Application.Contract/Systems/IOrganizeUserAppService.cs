using Aurora.Application.Contract.Systems.Dtos.DepartmentUserDtos;
using Aurora.Application.Contract.Systems.Dtos.OrganizeUserDtos;
using Aurora.Application.Contract.Systems.Dtos.UserRoleDtos;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems; 

public interface IOrganizeUserAppService : IAppService<OrganizeUser> {
    
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task SaveAsync(OrganizeUserSaveDto model);

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<UserOrganizeDto> GetData(string userId);

    Task<PageResult<OrganizeUserDto>> GetRoleUserListAsync(OrganizeUserListInput input);
    
    
}