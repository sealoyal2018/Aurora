using Aurora.Domain.Shared.Pages;

namespace Aurora.Application.Contract.Systems.Dtos.OrganizeUserDtos;

public class OrganizePageInput : PageInput {

    public string Name { get; set; }

    public int? Status { get; set; } 
}