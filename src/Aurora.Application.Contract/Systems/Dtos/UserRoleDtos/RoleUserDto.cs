using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.UserRoleDtos; 

public class RoleUserDto: User {
    public bool IsJoin { get; set; }
}