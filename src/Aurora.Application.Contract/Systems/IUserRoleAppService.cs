using Aurora.Application.Contract;
using Aurora.Application.Contract.Systems.Dtos.UserRoleDtos;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;
using System.Linq.Dynamic.Core;

namespace Aurora.Application.Contract.Systems; 

public interface IUserRoleAppService: IAppService<UserRole> {

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task SaveAsync(UserRoleSaveDto model);

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<UserRoleDto> GetData(string userId);

    Task<PageResult<RoleUserDto>> GetRoleUserListAsync(RoleUserListInput input);
    Task UpdateRoleUser(UpdateRoleUserDto input);
}