using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.UserDtos; 

public class UserDto :User {

    /// <summary>
    /// 所属部门列表
    /// </summary>
    public List<string> OrganizeIds { get; set; } = new List<string>();
    
}