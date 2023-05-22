using Aurora.Domain.Shared.Core.Tree;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.ResourceDtos;
public class ResourceTreeDto : Resource, ITreeModel<string, ResourceTreeDto> {
    public IEnumerable<ResourceTreeDto> Children { get; set; } = new List<ResourceTreeDto>();
    public string CheckArr { get; set; } = "0";
}
