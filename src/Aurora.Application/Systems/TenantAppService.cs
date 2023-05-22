using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.TenantDtos;
using Aurora.Domain;
using Aurora.Domain.Shared;
using Aurora.Domain.Shared.Core;
using Aurora.Domain.Shared.Core.Tree;
using Aurora.Domain.Shared.DependencyInjections;
using Aurora.Domain.Shared.Exceptions;
using Aurora.Domain.Shared.Extensions;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;
using Aurora.Domain.System.Shared;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;

namespace Aurora.Application.Systems;

public class TenantAppService : AppServiceBase<Tenant>, ITenantAppService, ITransientDependency {
    private readonly ICurrentUser _currentUser;
    private readonly IDistributedCache _cache;

    public TenantAppService(IRepository repository, ICurrentUser currentUser, IDistributedCache cache) : base(repository) {
        this._currentUser = currentUser;
        this._cache = cache;
    }

    public async Task<IEnumerable<Tenant>> GetAccessTenants(string userId) {
        var user = await Repository.FindAsync<User>(t => t.Id == userId);
        var res = new List<Tenant>();
        if (user.AccessType != DataAccessType.Protected) {
            var t = await this.FindAsync(t => t.Id == user.TenantId);
            if (t is null) {
                throw new BusException(false, "未找到相关数据", BusCodeType.NotFound);
            }

            res.Add(t);
            return res;
        }
        
        return await GetQueryable()
            .Where(t => t.ParentNum.Contains(user.TenantId))
            .OrderBy(t => t.Sort)
            .ToListAsync();
    }

    public async Task<IEnumerable<TenantTreeDto>> GetTenants() {
        var user = await Repository.FindAsync<User>(t => t.Id == this._currentUser.UserId);
        Expression<Func<Tenant, TenantTreeDto>> selectExpr = o => new TenantTreeDto { };
        selectExpr = selectExpr.BuildExtendSelectExpre();

        var whereExpr = LinqHelper.True<TenantTreeDto>()
            .AndIf(user.AccessType == DataAccessType.Self, t => t.Id == user.TenantId)
            .And(t=> t.ParentId != null)
            .AndIf(user.AccessType == DataAccessType.Private, t => t.Id == user.TenantId)
            .AndIf(user.AccessType == DataAccessType.Protected, t => t.ParentNum.Contains(this._currentUser.TenantId));
        
        var list = await GetQueryable()
            .Select(selectExpr)
            .Where(whereExpr)
            .OrderBy(t => t.Sort)
            .ToListAsync();

        return list.Build<string, TenantTreeDto>();
    }

    public async Task<PageResult<Tenant>> GetPages(TenantPageInput pageInput) {
        var whereExpr = LinqHelper.True<Tenant>()
            .AndIf(pageInput.Name.IsNotNullOrEmpty(), t => t.Name.Contains(pageInput.Name))
            .And(t => t.ParentNum.Contains(this._currentUser.TenantId));

        return await GetQueryable()
            .Where(whereExpr)
            .ToPageAsync(pageInput);
    }

    [UnitOfWork]
    public async Task CreateAsync(CreateTenantDto model) {
        var tenant = model.Tenant;
        var user = model.User;
        var parentTenant = await FindAsync(t => t.Id == this._currentUser.TenantId);
        tenant.Id = IdHelper.Get();
        tenant.ParentId = this._currentUser.TenantId;
        tenant.ParentNum = string.Join(',', new[] { parentTenant?.ParentNum , tenant.Id} );
        tenant.IsDeleted = false;
        tenant.CreatedTime = DateTime.Now;
        tenant.CreatorId = _currentUser.UserId;

        user.TenantId = tenant.Id;
        user.Id = IdHelper.Get();
        user.IsDeleted = tenant.IsDeleted;
        user.CreatedTime = tenant.CreatedTime;
        user.CreatorId = _currentUser.UserId;
        user.NickName = tenant.Name;
        user.AccessType = DataAccessType.Protected;
        user.Sort = 0;
        user.Gender = GenderType.Male;
        user.Status = DataStatus.Normal;

        await this.InsertAsync(tenant);
        await this.Repository.InsertAsync(user);
    }

    [UnitOfWork]
    public async Task EditAsync(Tenant tenant) {
        // 编辑
        var currentTenant = await this.FindAsync(t => t.Id == tenant.Id);
        //if (tenant.ParentId.IsNotNullOrEmpty()) {
        //    var parentTenant = await FindAsync(t => t.Id == tenant.ParentId);
        //    if (currentTenant.IsNullOrEmpty()) {
        //        throw new BusException(false, "未找到数据", BusCodeType.Forbidden);
        //    }

        //    if (parentTenant?.Id != currentTenant.ParentId) {
        //        // 更换父组织
        //        var childrenDict = await this.GetListAsync(t => t.ParentNum.Contains(tenant.Id));
        //        if (childrenDict.IsNotNullOrEmpty()) {
        //            currentTenant.ParentId = tenant.ParentId;
        //            currentTenant.ParentNum = parentTenant.ParentNum + "," + currentTenant.Id;

        //            foreach (var child in childrenDict) {
        //                var arr = child.ParentNum.Split(',');
        //                var index = Array.FindIndex(arr, t => t == tenant.Id);
        //                child.ParentNum = currentTenant.ParentNum + "," + string.Join(',', arr[(index)..]);
        //            }

        //            await this.UpdateAsync(childrenDict);
        //        }
        //    }
        //}

        currentTenant.Name = tenant.Name;
        //currentTenant.ParentId = tenant.ParentId;
        currentTenant.Sort = tenant.Sort;
        currentTenant.Describe = tenant.Describe;
        await this.UpdateAsync(currentTenant);
    }

    public async Task RemoveAsync(params string[] ids) {
        foreach (var item in ids) {
            await this.DeleteAsync(t => t.ParentNum.Contains(item));
        }
    }
}