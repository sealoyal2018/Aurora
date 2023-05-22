using Aurora.Domain.Shared.Pages;
using System.ComponentModel.DataAnnotations;

namespace Aurora.Application.Contract.Systems.Dtos.UserRoleDtos; 

public class RoleUserListInput: PageInput {
    [Required]
    public string RoleId { get; set; }
    
    public string Name { get; set; }

    public int? Status { get; set; }
}