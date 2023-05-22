using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.DepartmentUserDtos;
using Aurora.Application.Contract.Systems.Dtos.OrganizeUserDtos;
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

public class OrganizeUserAppService : AppServiceBase<OrganizeUser>, IOrganizeUserAppService, ITransientDependency {
    private readonly ICurrentUser _currentUser;
    public OrganizeUserAppService(IRepository repository, ICurrentUser currentUser) : base(repository) {
        this._currentUser = currentUser;
    }

    public async Task SaveAsync(OrganizeUserSaveDto model) {
        var now = DateTime.Now;
        if (model.UserId.IsNullOrEmpty()) {
            throw new BusException(false, "未找到相关信息", BusCodeType.NotFound);
        }
        var user = await this.Repository.FindAsync<User>(t => t.Id == model.UserId);
        if (user.IsNullOrEmpty()) {
            throw new BusException(false, "未找到相关信息", BusCodeType.NotFound);
        }
        await this.DeleteAsync(t => t.UserId == model.UserId);
        if (model.OrganizeIds.IsNullOrEmpty()) {
            return;
        }

        var newData = model.OrganizeIds.Select(t => new OrganizeUser {
            Id = IdHelper.Get(),
            UserId = model.UserId,
            IsDeleted = false,
            CreatedTime = now,
            CreatorId = this._currentUser.UserId,
            OrganizeId = t,
            TenantId = user.TenantId,
        });
        await this.InsertAsync(newData);
    }

    public async Task<UserOrganizeDto> GetData(string userId) {
        var queryable = from ur in this.GetQueryable()
            join u in this.Repository.GetQueryable<User>() on ur.UserId equals u.Id
            join r in this.Repository.GetQueryable<Organize>() on ur.OrganizeId equals r.Id
            where ur.UserId == userId
            select new {
                UserName = u.UserName,
                UserId = u.Id,
                Organize = r
            };

        return await queryable.GroupBy(t => new { t.UserId, t.UserName })
            .Select(t => new UserOrganizeDto {
                UserName = t.Key.UserName,
                UserId = t.Key.UserId,
                Organizes = t.Select(v => v.Organize).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PageResult<OrganizeUserDto>> GetRoleUserListAsync(OrganizeUserListInput input) {
        var roleQueryable = this.GetQueryable().Where(t=> t.OrganizeId == input.OrganizeId);
        Expression<Func<User, OrganizeUserDto>> selectExpr = (u) => new OrganizeUserDto {
            IsJoin = roleQueryable.Any(t => t.UserId == u.Id)
        };

        var whereExpr = LinqHelper.True<OrganizeUserDto>()
            .AndIf(input.Name.IsNotNullOrEmpty(), t => t.UserName.Contains(input.Name))
            .AndIf(input.Status == 0, t => t.IsJoin)
            .AndIf(input.Status != 0, t => !t.IsJoin);


        var queryable = Repository.GetQueryable<User>()
            .Select(selectExpr)
            .Where(whereExpr)
            .ToPageAsync(input);

        return await queryable;
    }
}