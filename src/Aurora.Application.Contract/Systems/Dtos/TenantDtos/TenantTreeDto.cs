using Aurora.Domain.Shared.Core.Tree;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.TenantDtos; 

public class TenantTreeDto: Tenant, ITreeModel<string, TenantTreeDto> {
    public IEnumerable<TenantTreeDto> Children { get; set; } = new List<TenantTreeDto>();
    public string CheckArr { get; set; } = "0";
}