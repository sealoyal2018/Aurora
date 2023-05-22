using Aurora.Application.Contract.Systems.Dtos.TenantDtos;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems; 

public interface ITenantAppService : IAppService<Tenant> {
    Task<IEnumerable<Tenant>> GetAccessTenants(string userId);
    Task<IEnumerable<TenantTreeDto>> GetTenants();
    Task<PageResult<Tenant>> GetPages(TenantPageInput pageInput);
    Task CreateAsync(CreateTenantDto model);
    Task EditAsync(Tenant tenant);
    Task RemoveAsync(params string[] ids);
}