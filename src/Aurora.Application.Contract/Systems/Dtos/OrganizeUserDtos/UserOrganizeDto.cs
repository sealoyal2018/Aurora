using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.OrganizeUserDtos; 

public class UserOrganizeDto {
    public string UserId { get; set; }
    
    public string UserName { get; set; }

    public List<Organize> Organizes = new List<Organize>();
}