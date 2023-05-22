using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.UserRoleDtos;
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

public class UserRoleAppService : AppServiceBase<UserRole>, IUserRoleAppService, ITransientDependency {

    private readonly ICurrentUser _currentUser;

    public UserRoleAppService(IRepository repository, ICurrentUser currentUser) : base(repository) {
        this._currentUser = currentUser;
    }

    [UnitOfWork]
    public async Task SaveAsync(UserRoleSaveDto model) {
        var now = DateTime.Now;
        if (model.RoleIds.IsNullOrEmpty()) {
            return;
        }
        var user = await this.Repository.FindAsync<User>(t => t.Id == model.UserId);
        if (user.IsNullOrEmpty()) {
            throw new BusException(false, "未找到相关信息", BusCodeType.NotFound);
        }
        await this.DeleteAsync(t => t.UserId == model.UserId);

        var newData = model.RoleIds.Select(t => new UserRole {
            Id = IdHelper.Get(),
            UserId = model.UserId,
            IsDeleted = false,
            CreatedTime = now,
            CreatorId = this._currentUser.UserId,
            RoleId = t,
            TenantId = user.TenantId,
        });
        await this.InsertAsync(newData);
    }

    public async Task<UserRoleDto> GetData(string userId) {

        var queryable = from ur in this.GetQueryable()
                        join u in this.Repository.GetQueryable<User>() on ur.UserId equals u.Id
                        join r in this.Repository.GetQueryable<Role>() on ur.RoleId equals r.Id
                        where ur.UserId == userId
                        select new {
                            UserName = u.UserName,
                            UserId = u.Id,
                            Role = r
                        };

        return await queryable.GroupBy(t => new { t.UserId, t.UserName })
            .Select(t => new UserRoleDto {
                UserName = t.Key.UserName,
                UserId = t.Key.UserId,
                Roles = t.Select(v => v.Role).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PageResult<RoleUserDto>> GetRoleUserListAsync(RoleUserListInput input) {

        var roleQueryable = this.GetQueryable().Where(t => t.RoleId == input.RoleId);

        Expression<Func<User, RoleUserDto>> selectExpr = (u) => new RoleUserDto {
            IsJoin = roleQueryable.Any(t => t.UserId == u.Id)
        };

        selectExpr = selectExpr.BuildExtendSelectExpre();

        var whereExpr = LinqHelper.True<RoleUserDto>()
            .AndIf(input.Name.IsNotNullOrEmpty(), t => t.UserName.Contains(input.Name))
            .AndIf(input.Status.HasValue && input.Status == 0, t => t.IsJoin)
            .AndIf(input.Status.HasValue && input.Status != 0, t => !t.IsJoin);

        var queryable = Repository.GetQueryable<User>()
            .Select(selectExpr)
            .Where(whereExpr)
            .ToPageAsync(input);

        var ret = await queryable;
        return ret;
    }

    public async Task UpdateRoleUser(UpdateRoleUserDto input) {
        if (input.UserId.IsNullOrEmpty() || input.RoleId.IsNullOrEmpty()) {
            throw new BusException(false, "非法操作", BusCodeType.BadRequest);
        }
        var user = await this.Repository.FindAsync<User>(t => t.Id == input.UserId);
        if (user.IsNullOrEmpty()) {
            throw new BusException(false, "未找到相关数据", BusCodeType.NotFound);
        }
        
        
        var data = await this.GetQueryable().FirstOrDefaultAsync(t=> t.RoleId == input.RoleId && t.UserId == input.UserId);
        if(input.IsRemove) {
            if (data.IsNullOrEmpty()) {
                return;
            }

            await this.DeleteAsync(t => t.Id == data.Id);
            return;
        }

        // add
        if (data.IsNotNullOrEmpty()) {
            return;
        }

        await this.InsertAsync(new UserRole {
            Id = IdHelper.Get(),
            UserId = input.UserId,
            IsDeleted = false,
            CreatedTime = DateTime.Now,
            CreatorId = this._currentUser.UserId,
            RoleId = input.RoleId,
            TenantId = user.TenantId,
        });

    }
}