using System.ComponentModel.DataAnnotations;

namespace Aurora.Application.Contract.Systems.Dtos.UserRoleDtos; 

public class UserRoleSaveDto {
    [Required]
    public string UserId { get; set; }
    
    [Required]
    public List<string> RoleIds { get; set; }
}