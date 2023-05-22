using Aurora.Domain.Shared.Pages;

namespace Aurora.Application.Contract.Systems.Dtos.RoleDtos; 

public class RolePageInput: PageInputBase {
    public string Name { get; set; }
}