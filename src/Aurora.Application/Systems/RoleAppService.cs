using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.RoleDtos;
using Aurora.Domain;
using Aurora.Domain.Shared;
using Aurora.Domain.Shared.Core;
using Aurora.Domain.Shared.DependencyInjections;
using Aurora.Domain.Shared.Exceptions;
using Aurora.Domain.Shared.Extensions;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Aurora.Application.Systems;

public class RoleAppService : AppServiceBase<Role>, IRoleAppService, ITransientDependency {
    private readonly ICurrentUser _currentUser;

    public RoleAppService(IRepository repository, ICurrentUser currentUser) : base(repository) {
        this._currentUser = currentUser;
    }

    public async Task<PageResult<RoleDto>> GetUsers(RolePageInput pageInput) {
        Expression<Func<Role, RoleDto>> selectExpr = r => new RoleDto { };
        selectExpr = selectExpr.BuildExtendSelectExpre();

        var whereExpr = LinqHelper.True<Role>()
            .AndIf(pageInput.Name.IsNotNullOrEmpty(), p => p.Name.Contains(pageInput.Name));

        return await GetQueryable()
            .Where(whereExpr)
            .Select(selectExpr)
            .ToPageAsync(pageInput);
    }

    public async Task CreateOrUpdateUserAsync(CreateOrUpdateRoleDto model) {
        if (model.Name.IsNullOrEmpty())
            throw new BusException(false, "请填写角色名", BusCodeType.NotAcceptable);

        if (model.Id.IsNullOrEmpty()) {
            await this.InsertAsync(new Role {
                Id = IdHelper.Get(),
                CreatedTime = DateTime.Now,
                CreatorId = this._currentUser.UserId,
                IsDeleted = false,
                Name = model.Name,
                Description = model.Description,
                Sort = model.Sort,
                TenantId = this._currentUser.TenantId,
            });
            return;
        }

        var role = await this.FindAsync(p => p.Id == model.Id);
        if (role.IsNullOrEmpty())
            throw new BusException(false, "找不到相关数据", BusCodeType.NotFound);
        role.Name = model.Name;
        role.Description = model.Description;
        role.Sort = model.Sort;
        await this.UpdateAsync(role);
    }

    public async Task<RoleDto> GetRoleById(string id) {
        Expression<Func<Role, RoleDto>> selectExpr = r => new RoleDto { };
        selectExpr = selectExpr.BuildExtendSelectExpre();

        var whereExpr = LinqHelper.True<Role>();

        whereExpr = whereExpr.AndIf(id.IsNotNullOrEmpty(), p => p.Id == id);
        return await GetQueryable()
            .Where(whereExpr)
            .Select(selectExpr)
            .FirstOrDefaultAsync();
    }

    [UnitOfWork]
    public async Task RemoveAsync(params string[] ids) {
        
        // 删除用户角色数据
        await this.Repository.DeleteAsync<UserRole>(t => ids.Contains(t.RoleId));
        
        // 删除资源数据
        await this.Repository.DeleteAsync<RoleResource>(t => ids.Contains(t.RoleId));
        
        // 删除自身数据
        await this.DeleteAsync(t => ids.Contains(t.Id));
    }
}