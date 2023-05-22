using Aurora.Domain.Shared.Core.Tree;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.ResourceDtos;

public class MenuItem: Resource, ITreeModel<string, MenuItem> {
    
    public string Title => this.Name;

    public string OpenType => Type switch {
        ResourceType.Menu => "_iframe",
        _ => "",
    };

    public string Href => this.Url;
    public IEnumerable<MenuItem> Children { get; set; } = new List<MenuItem>();
    public string CheckArr { get; set; } = "0";
}