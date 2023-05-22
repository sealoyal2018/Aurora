namespace Aurora.Application.Contract.Systems.Dtos.RoleResourceDtos; 

public class RoleResourceSaveDto {
    /// <summary>
    /// 角色
    /// </summary>
    public string RoleId { get; set; }
    
    /// <summary>
    /// 资源ids
    /// </summary>
    public List<string> ResourceIds { get; set; }
}