using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.OrganizeDtos;
using Aurora.Application.Contract.Systems.Dtos.OrganizeUserDtos;
using Aurora.Application.Contract.Systems.Dtos.ResourceDtos;
using Aurora.Domain;
using Aurora.Domain.Shared;
using Aurora.Domain.Shared.Core;
using Aurora.Domain.Shared.Core.Tree;
using Aurora.Domain.Shared.DependencyInjections;
using Aurora.Domain.Shared.Exceptions;
using Aurora.Domain.Shared.Extensions;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Aurora.Application.Systems; 

public class OrganizeAppService : AppServiceBase<Organize>, IOrganizeAppService, ITransientDependency {
    private readonly ICurrentUser _currentUser;
    public OrganizeAppService(IRepository repository, ICurrentUser currentUser) : base(repository) {
        this._currentUser = currentUser;
    }

    public async Task<Organize> GetData(string id) {
        return await this.FindAsync(t=> t.Id == id);
    }

    public async Task<PageResult<Organize>> GetPageList(OrganizePageInput input) {
        var whereExpr = LinqHelper.True<Organize>()
            .AndIf(input.Name.IsNotNullOrEmpty(), p => p.Name.Contains(input.Name));

        return await GetQueryable()
            .Where(whereExpr)
            .ToPageAsync(input);
    }

    public async Task<IEnumerable<OrganizeTreeDto>> GetTreeAsync() {
        Expression<Func<Organize, OrganizeTreeDto>> selectExpr = o => new OrganizeTreeDto { };
        selectExpr = selectExpr.BuildExtendSelectExpre();
        var data = await GetQueryable().Select(selectExpr).ToListAsync();
        return data.Build<string, OrganizeTreeDto>();
    }

    [UnitOfWork]
    public async Task RemoveAsync(params string[] ids) {

        var list = await this.GetQueryable().Select(p => new { Id = p.Id, p.ParentNum }).ToListAsync();
        var deletingIds = new List<string>();
        deletingIds.AddRange(ids);
        foreach (var item in list) {
            var arr = item.ParentNum.Split(',');
            var ret = arr.Any(ids.Contains);
            if (ret) {
                deletingIds.Add(item.Id);
            }
        }

        await Repository.DeleteAsync<OrganizeUser>(t => deletingIds.Contains(t.OrganizeId));
        await this.DeleteAsync(t => deletingIds.Contains(t.Id));
    }

    public async Task SaveOrUpdateAsync(Organize input) {
        if (input.Name.IsNullOrEmpty()) {
            throw new BusException(false, "请填写组织名称", BusCodeType.NotAcceptable);
        }

        Organize parentOrg = null;
        if (input.ParentId.IsNotNullOrEmpty()) {
            parentOrg = await this.GetQueryable().FirstOrDefaultAsync(t => t.Id == input.ParentId);
        }
        
        if (input.Id.IsNullOrEmpty()) {
            // 添加
            input.Id = IdHelper.Get();
            input.CreatedTime = DateTime.Now;
            input.CreatorId = this._currentUser.UserId;
            input.IsDeleted = false;
            input.ParentNum = input.Id;
            input.TenantId = this._currentUser.UserId;
            if (parentOrg != null) {
                input.ParentNum = parentOrg.Id + "," + input.Id;
            }

            await this.InsertAsync(input);
            return;
        }

        // 编辑
        var currentOrg = await this.FindAsync(t => t.Id == input.Id);
        if (currentOrg.IsNullOrEmpty()) {
            throw new BusException(false, "未找到数据", BusCodeType.Forbidden);
        }
        
        if (parentOrg?.ParentId != currentOrg.ParentId) {
            // 更换父组织
            var childrenOrg = await this.GetListAsync(t => t.ParentNum.Contains(input.Id));
            if (childrenOrg.IsNotNullOrEmpty()) {
                currentOrg.ParentId = input.ParentId;
                currentOrg.ParentNum = parentOrg.ParentNum + "," + currentOrg.Id;

                foreach (var child in childrenOrg) {
                    var arr = child.ParentNum.Split(',');
                    var index = Array.FindIndex(arr, t => t == input.Id);
                    child.ParentNum = currentOrg.ParentNum + "," + string.Join(',', arr[(index)..]);
                }

                await this.UpdateAsync(childrenOrg);
            }
        }

        currentOrg.Name = input.Name;
        currentOrg.TenantId = input.TenantId;
        await this.UpdateAsync(currentOrg);
    }
}