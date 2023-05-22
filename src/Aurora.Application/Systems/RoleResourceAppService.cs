using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.RoleResourceDtos;
using Aurora.Application.Contract.Systems.Dtos.UserRoleDtos;
using Aurora.Domain;
using Aurora.Domain.Shared;
using Aurora.Domain.Shared.Core;
using Aurora.Domain.Shared.DependencyInjections;
using Aurora.Domain.Shared.Exceptions;
using Aurora.Domain.Shared.Extensions;
using Aurora.Domain.System;
using Microsoft.EntityFrameworkCore;

namespace Aurora.Application.Systems; 

public class RoleResourceAppService : AppServiceBase<RoleResource>, IRoleResourceAppService,ITransientDependency {
    private readonly ICurrentUser _currentUser;
    
    public RoleResourceAppService(IRepository repository, ICurrentUser currentUser) : base(repository) {
        this._currentUser = currentUser;
    }

    
    [UnitOfWork]
    public async Task SaveAsync(RoleResourceSaveDto model) {
        var now = DateTime.Now;
        if (model.ResourceIds.IsNullOrEmpty())
            return;

        var role = await this.Repository.FindAsync<Role>(t => t.Id == model.RoleId);
        if (role.IsNullOrEmpty()) {
            throw new BusException(false, "未找到信息数据", BusCodeType.NotFound);
        }
        
        await this.DeleteAsync(t => t.RoleId == model.RoleId);
        
        var newData = model.ResourceIds.Select(t => new RoleResource {
            Id = IdHelper.Get(),
            RoleId = model.RoleId,
            IsDeleted = false,
            CreatedTime = now,
            CreatorId = this._currentUser.UserId,
            ResourceId = t,
        });
        await this.InsertAsync(newData);
    }

    public async Task<RoleResourceDto> GetData(string roleId) {
        var queryable = from rr in this.GetQueryable()
            join r1 in this.Repository.GetQueryable<Resource>() on rr.ResourceId equals r1.Id
            join r2 in this.Repository.GetQueryable<Role>() on rr.RoleId equals r2.Id
            where rr.RoleId == roleId
            select new {
                RoleName = r2.Name,
                RoleId = r2.Id,
                Resource = r1
            };

        return await queryable.GroupBy(t => new { t.RoleId, t.RoleName })
            .Select(t => new RoleResourceDto {
                RoleName = t.Key.RoleName,
                RoleId = t.Key.RoleId,
                Resources = t.Select(v => v.Resource).ToList()
            })
            .FirstOrDefaultAsync();
        
    }
}