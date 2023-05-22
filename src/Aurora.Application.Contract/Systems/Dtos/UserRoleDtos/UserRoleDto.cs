using Aurora.Application.Contract.Systems.Dtos.RoleDtos;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.UserRoleDtos; 

public class UserRoleDto {
    public string UserId { get; set; }
    public string UserName { get; set; }
    public List<Role> Roles { get; set; }
}