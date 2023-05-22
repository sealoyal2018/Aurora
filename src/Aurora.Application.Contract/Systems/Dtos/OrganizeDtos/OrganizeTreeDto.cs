using Aurora.Domain.Shared.Core.Tree;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.OrganizeDtos;

public class OrganizeTreeDto : Domain.System.Organize, ITreeModel<string, OrganizeTreeDto> {
    public IEnumerable<OrganizeTreeDto> Children { get; set; } = new List<OrganizeTreeDto>();
    public string CheckArr { get; set; } = "0";
}