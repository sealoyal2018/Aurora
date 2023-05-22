using Aurora.Application.Contract.Systems.Dtos.OrganizeDtos;
using Aurora.Application.Contract.Systems.Dtos.OrganizeUserDtos;
using Aurora.Application.Contract.Systems.Dtos.ResourceDtos;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems; 

public interface IOrganizeAppService : IAppService<Organize> {
    Task<Organize> GetData(string id);
    Task<PageResult<Organize>> GetPageList(OrganizePageInput input);
    Task<IEnumerable<OrganizeTreeDto>> GetTreeAsync();
    
    Task RemoveAsync(params string[] ids);
    
    Task SaveOrUpdateAsync(Organize input);
}