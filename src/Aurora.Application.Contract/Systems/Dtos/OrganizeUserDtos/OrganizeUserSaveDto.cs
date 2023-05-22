using System.ComponentModel.DataAnnotations;

namespace Aurora.Application.Contract.Systems.Dtos.DepartmentUserDtos; 

public class OrganizeUserSaveDto {
    [Required]
    public string UserId { get; set; }

    [Required]
    public List<string> OrganizeIds { get; set; } = new List<string>();
}