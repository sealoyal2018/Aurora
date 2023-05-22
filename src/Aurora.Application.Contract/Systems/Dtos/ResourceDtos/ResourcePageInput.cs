using Aurora.Domain.Shared.Pages;

namespace Aurora.Application.Contract.Systems.Dtos.ResourceDtos;
public class ResourcePageInput : PageInputBase {
    public string Name { get; set; }
}
