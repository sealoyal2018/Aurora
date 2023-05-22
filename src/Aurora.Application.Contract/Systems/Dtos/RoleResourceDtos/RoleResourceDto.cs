using Aurora.Application.Contract.Systems.Dtos.ResourceDtos;
using Aurora.Application.Contract.Systems.Dtos.RoleDtos;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.RoleResourceDtos; 

public class RoleResourceDto {
    /// <summary>
    /// 角色
    /// </summary>
    public string RoleId { get; set; }
    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName { get; set; }
    /// <summary>
    /// 角色资源
    /// </summary>
    public List<Resource> Resources { get; set; }
}