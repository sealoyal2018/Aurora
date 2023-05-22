using Aurora.Application.Contract.Systems.Dtos.RoleDtos;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems; 

public interface IRoleAppService: IAppService<Role> {
    
    Task<PageResult<RoleDto>> GetUsers(RolePageInput pageInput);

    Task CreateOrUpdateUserAsync(CreateOrUpdateRoleDto model);
    
    Task<RoleDto> GetRoleById(string id);

    Task RemoveAsync(params string[] ids);
}