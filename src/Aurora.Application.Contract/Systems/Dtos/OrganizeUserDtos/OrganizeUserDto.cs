using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.OrganizeUserDtos; 

public class OrganizeUserDto : User {
    
    public bool IsJoin { get; set; }
}