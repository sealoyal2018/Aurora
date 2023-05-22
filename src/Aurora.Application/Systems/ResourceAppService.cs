using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.ResourceDtos;
using Aurora.Domain;
using Aurora.Domain.Shared;
using Aurora.Domain.Shared.Cons;
using Aurora.Domain.Shared.Core;
using Aurora.Domain.Shared.Core.Tree;
using Aurora.Domain.Shared.DependencyInjections;
using Aurora.Domain.Shared.Exceptions;
using Aurora.Domain.Shared.Extensions;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;
using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NPOI.SS.Formula.Functions;
using System.Linq.Expressions;
using System.Text;

namespace Aurora.Application.Systems;
public class ResourceAppService : AppServiceBase<Resource>, IResourceAppService, ITransientDependency {

    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    public ResourceAppService(IRepository repository, ICurrentUser currentUser, IMapper mapper, IDistributedCache cache) : base(repository) {
        this._currentUser = currentUser;
        this._mapper = mapper;
        this._cache = cache;
    }

    public async Task CreateOrUpdateAsync(CreateOrUpdateResourceDto model) {
        if (model.IsNullOrEmpty()) {
            throw new BusException(false, "数据不能为空", BusCodeType.Forbidden);
        }

        if (model.Name.IsNullOrEmpty()) {
            throw new BusException(false, "资源名称不能为空", BusCodeType.Forbidden);
        }

        var type = model.Type;
        if (!Enum.IsDefined(type)) {
            throw new BusException(false, "请正确选择资源类型", BusCodeType.Forbidden);
        }

        if (type == ResourceType.Menu) {
            if (model.Url.IsNullOrEmpty()) {
                throw new BusException(false, "菜单类型资源的地址不能为空", BusCodeType.Forbidden);
            }
            if (model.ParentId.IsNullOrEmpty()) {
                throw new BusException(false, "菜单类型资源的所属目录不能为空", BusCodeType.Forbidden);
            }
        }

        if (type == ResourceType.Directory && model.Url.IsNotNullOrEmpty()) {
            model.Url = string.Empty;
        }


        if (model.IsOutside && model.Type != ResourceType.Menu) {
            throw new BusException(false, "外链只能是菜单类型的资源", BusCodeType.Forbidden);
        }


        Resource parentResource = null;
        if (model.ParentId.IsNotNullOrEmpty()) {
            parentResource = await this.FindAsync(t => t.Id == model.ParentId);
        }

        if (model.Id.IsNullOrEmpty()) {
            var id = IdHelper.Get();
            await this.InsertAsync(new Resource {
                Id = id,
                CreatedTime = DateTime.Now,
                CreatorId = _currentUser.UserId,
                IsDeleted = false,
                Name = model.Name,
                Url = model.Url,
                Icon = model.Icon,
                ParentId = model.ParentId,
                Type = model.Type,
                IsOutside = model.IsOutside,
                ParentNum = id,
                PermissionCode = model.PermissionCode,
                Sort = model.Sort
            });
            return;
        }

        var currentResource = await this.FindAsync(t => t.Id == model.Id);
        if (currentResource.IsNullOrEmpty()) {
            throw new BusException(false, "未找到数据", BusCodeType.Forbidden);
        }

        if (parentResource?.Id != currentResource.ParentId) {
            // 更换父组织
            var childrenDict = await this.GetListAsync(t => t.ParentNum.Contains(model.Id));
            if (childrenDict.IsNotNullOrEmpty()) {
                currentResource.ParentId = model.ParentId;
                currentResource.ParentNum = parentResource.ParentNum + "," + currentResource.Id;

                foreach (var child in childrenDict) {
                    var arr = child.ParentNum.Split(',');
                    var index = Array.FindIndex(arr, t => t == model.Id);
                    child.ParentNum = currentResource.ParentNum + "," + string.Join(',', arr[(index)..]);
                }

                await this.UpdateAsync(childrenDict);
            }
        }

        currentResource.Name = model.Name;
        currentResource.ParentId = model.ParentId;
        currentResource.Url = model.Url;
        currentResource.Icon = model.Icon;
        currentResource.Type = model.Type;
        currentResource.IsOutside = model.IsOutside;
        currentResource.PermissionCode = model.PermissionCode;
        currentResource.Sort = model.Sort;
        await this.UpdateAsync(currentResource);

    }

    public async Task<ResourceDto> GetData(string id) {
        Expression<Func<Resource, ResourceDto>> selectExpr = r => new ResourceDto { };
        selectExpr = selectExpr.BuildExtendSelectExpre();

        var whereExpr = LinqHelper.True<Resource>();

        whereExpr = whereExpr.And(p => p.Id == id);

        return await GetQueryable()
            .Where(whereExpr)
            .Select(selectExpr)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ResourceDto>> GetListAsync() {
        Expression<Func<Resource, ResourceDto>> selectExpr = r => new ResourceDto { };
        selectExpr = selectExpr.BuildExtendSelectExpre();
        return await GetQueryable()
            .Select(selectExpr)
            .ToListAsync();
    }

    public async Task<IEnumerable<ResourceTreeDto>> GetTreeListAsync(string name, bool filterAction) {
        Expression<Func<Resource, ResourceTreeDto>> selectExpr = r => new ResourceTreeDto { };
        var whereExpr = LinqHelper.True<ResourceTreeDto>()
            .AndIf(name.IsNotNullOrEmpty(), t => t.Name.Contains(name))
            .AndIf(filterAction, t => t.Type != ResourceType.Action);

        selectExpr = selectExpr.BuildExtendSelectExpre();
        var data = await GetQueryable()
            .Select(selectExpr)
            .Where(whereExpr)
            .OrderBy(t=> t.Sort)
            .ToListAsync();

        if(name.IsNotNullOrEmpty()) {
            data.ForEach(x => x.ParentId = null);
        }


        return data.Build<string, ResourceTreeDto>();
    }

    public async Task<PageResult<ResourceDto>> GetPageList(ResourcePageInput pageInput) {
        Expression<Func<Resource, ResourceDto>> selectExpr = r => new ResourceDto { };
        selectExpr = selectExpr.BuildExtendSelectExpre();

        var whereExpr = LinqHelper.True<Resource>();

        whereExpr = whereExpr.AndIf(pageInput.Name.IsNotNullOrEmpty(), p => p.Name.Contains(pageInput.Name));
        pageInput.Order = $"{nameof(ResourceDto.Sort)}_asc";
        return await GetQueryable()
            .Where(whereExpr)
            .Select(selectExpr)
            .ToPageAsync(pageInput);
    }

    [UnitOfWork]
    public async Task RemoveAsync(params string[] ids) {

        await this.Repository.DeleteAsync<RoleResource>(t => ids.Contains(t.ResourceId));
        
        await this.DeleteAsync(t => ids.Contains(t.Id));
    }

    public async Task<IEnumerable<MenuItem>> GetMenuTreeAsync() {

        var roleIds = await this.Repository.GetQueryable<UserRole>()
            .Where(t => t.UserId == this._currentUser.UserId)
            .Select(t => t.RoleId)
            .ToListAsync();

        var resourceIds = await this.Repository.GetQueryable<RoleResource>()
            .Where(t => roleIds.Contains(t.RoleId))
            .Select(p => p.ResourceId)
            .ToListAsync();
        
        Expression<Func<Resource, MenuItem>> selectExpr = r => new MenuItem { };
        selectExpr = selectExpr.BuildExtendSelectExpre();
        var data = await GetQueryable()
            .Where(p=> resourceIds.Contains(p.Id) && p.Type != ResourceType.Action)
            .Select(selectExpr)
            .OrderBy(t=>t.Sort)
            .ToListAsync();

        return data.Build<string, MenuItem>();
    }

    public async Task<IEnumerable<string>> GetPermissionCodes(string userId) {
        var roleIds = await this.Repository.GetQueryable<UserRole>()
            .Where(t => t.UserId == userId)
            .Select(t => t.RoleId)
            .ToListAsync();

        var resourceIds = await this.Repository.GetQueryable<RoleResource>()
            .Where(t => roleIds.Contains(t.RoleId))
            .Select(p => p.ResourceId)
            .ToListAsync();

        var codes = await GetQueryable()
            .Where(p => resourceIds.Contains(p.Id) && p.Type == ResourceType.Action)
            .Select(t => t.PermissionCode)
            .ToListAsync();
        
        return codes;
    }
    public async Task<IEnumerable<string>> GetPermissionIds(string roleId) {

        var resourceIds = await this.Repository.GetQueryable<RoleResource>()
            .Where(t => roleId == t.RoleId)
            .Select(p => p.ResourceId)
            .ToListAsync();

        return resourceIds;
    }

    public async Task<bool> HavePermissionAsync(string code) {
        if (string.IsNullOrWhiteSpace(code)) {
            return false;
        }
        
        var bytes = await _cache.GetAsync(string.Format(CacheCons.PermissionKey, this._currentUser.UserId));
        if (bytes is null) {
            return false;
        }
        var permissionCodeString = Encoding.UTF8.GetString(bytes);
        var codes = permissionCodeString.Split(',');

        var arr = code.Split(',');
        if (arr.Any(item => item.IsIn(codes)))
        {
            return true;
        }

        return false;
    }
}
