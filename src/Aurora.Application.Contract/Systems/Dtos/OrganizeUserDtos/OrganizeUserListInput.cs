using Aurora.Domain.Shared.Pages;
using System.ComponentModel.DataAnnotations;

namespace Aurora.Application.Contract.Systems.Dtos.OrganizeUserDtos; 

public class OrganizeUserListInput : PageInput {
    
    [Required]
    public string OrganizeId { get; set; }
    public string Name { get; set; }

    public int Status { get; set; } = 0;
}