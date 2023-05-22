using Aurora.Domain.Shared.Pages;

namespace Aurora.Application.Contract.Systems.Dtos.TenantDtos; 

public class TenantPageInput : PageInput {
    public string Name { get; set; }
}