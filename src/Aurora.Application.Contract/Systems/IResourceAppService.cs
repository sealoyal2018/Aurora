using Aurora.Application.Contract.Systems.Dtos.ResourceDtos;
using Aurora.Application.Contract.Systems.Dtos.RoleDtos;
using Aurora.Application.Contract.Systems.Dtos.UserDtos;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Application.Contract.Systems;

public interface IResourceAppService : IAppService<Resource> {

    Task<PageResult<ResourceDto>> GetPageList(ResourcePageInput pageInput);

    Task CreateOrUpdateAsync(CreateOrUpdateResourceDto model);
    Task<ResourceDto> GetData(string id);

    Task RemoveAsync(params string[] ids);


    Task<IEnumerable<ResourceDto>> GetListAsync();
    
    Task<IEnumerable<ResourceTreeDto>> GetTreeListAsync(string name, bool filterAction);
    
    Task<IEnumerable<MenuItem>> GetMenuTreeAsync();

    Task<IEnumerable<string>> GetPermissionCodes(string userId);

    Task<IEnumerable<string>> GetPermissionIds(string roldId);

    Task<bool> HavePermissionAsync(string code);
}
