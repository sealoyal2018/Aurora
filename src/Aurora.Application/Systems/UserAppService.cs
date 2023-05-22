using Aurora.Application.Contract.Systems;
using Aurora.Application.Contract.Systems.Dtos.OrganizeDtos;
using Aurora.Application.Contract.Systems.Dtos.UserDtos;
using Aurora.Domain;
using Aurora.Domain.Shared;
using Aurora.Domain.Shared.Cons;
using Aurora.Domain.Shared.Core;
using Aurora.Domain.Shared.DependencyInjections;
using Aurora.Domain.Shared.Exceptions;
using Aurora.Domain.Shared.Extensions;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;
using Aurora.Domain.System.Shared;
using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace Aurora.Application.Systems;

public class UserAppService : AppServiceBase<User>, IUserAppService, ITransientDependency {
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly IOrganizeUserAppService _organizeUserAppService;

    public UserAppService(IRepository repository, IDistributedCache cache, ICurrentUser currentUser, IOrganizeUserAppService organizeUserAppService) : base(repository) {
        this._cache = cache;
        this._currentUser = currentUser;
        this._organizeUserAppService = organizeUserAppService;
    }

    public async Task<CaptchaDto> GetCaptchaAsync() {
        var random = new Random();
        var width = 130;
        var height = 48;
        var type = random.Next(0, 100);
        byte[] bytes;
        string value;
        var id = IdHelper.Get();
        if (type % 2 == 0) {
            // 文字
            var length = random.Next(0, 50) % 2 == 0 ? 4 : 5;
            if (length == 5)
                width = 130;
            (value, bytes) = await ImageHelper.GeneratorTextCaptchaAsync(length, width, height);
        } else {
            // 数字
            (value, bytes) = await ImageHelper.GeneratorCalcCaptchaAsync(width, height);
        }
        await _cache.SetStringAsync(string.Format(CacheCons.Captchakey, id), value, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });

        return new CaptchaDto {
            ImageId = id,
            Image = Convert.ToBase64String(bytes)
        };
    }

    public async Task<PageResult<UserDto>> GetUsers(UserPageInput pageInput) {

        var userIds = new List<string>();
        if(pageInput.OrganizeId.IsNotNullOrEmpty()) {
            userIds = await Repository.GetQueryable<OrganizeUser>()
                .Where(p=> p.OrganizeId == pageInput.OrganizeId)
                .Select(t=> t.UserId)
                .ToListAsync();

            if (userIds.IsNullOrEmpty()) {
                userIds = new List<string>();
            }
        }

        Expression<Func<User, UserDto>> selectExpr = u => new UserDto {
            OrganizeIds = Repository.GetQueryable<OrganizeUser>().Where(p => p.UserId == u.Id).Select(t => t.OrganizeId).ToList(),
        };
        selectExpr = selectExpr.BuildExtendSelectExpre();

        var whereExpr = LinqHelper.True<User>()
            .AndIf(pageInput.UserName.IsNotNullOrEmpty(), p => p.UserName.Contains(pageInput.UserName))
            .AndIf(pageInput.Phone.IsNotNullOrEmpty(), p => p.Phone == pageInput.Phone)
            .AndIf(pageInput.OrganizeId.IsNotNullOrEmpty(), p => userIds.Contains(p.Id));

        return await GetQueryable()
            .Where(whereExpr)
            .Select(selectExpr)
            .ToPageAsync(pageInput);
    }

    [UnitOfWork]
    public async Task CreateUpdateUserAsync(CreateUpdateUserDto model) {
        if (model.UserName.IsNullOrEmpty())
            throw new BusException(false, "请填写用户名", BusCodeType.NotAcceptable);
        if (model.Phone.IsNullOrEmpty())
            throw new BusException(false, "请填写手机号码", BusCodeType.NotAcceptable);
        if (model.Email.IsNullOrEmpty())
            throw new BusException(false, "请填写邮箱", BusCodeType.NotAcceptable);
        if (model.NickName.IsNullOrEmpty())
            throw new BusException(false, "请填写昵称", BusCodeType.NotAcceptable);
        var type = (int)model.Gender;
        if (!Enum.IsDefined(typeof(GenderType), type)) {
            throw new BusException(false, "正确选择用户性别", BusCodeType.NotAcceptable);
        }

        if (model.Id.IsNullOrEmpty()) {

            var d = await FindAsync(t=> t.UserName == model.UserName);
            if (d.IsNotNullOrEmpty()) {
                throw new BusException(false, "该登录账号已被占用,请换个重试!", BusCodeType.Forbidden);
            }

            if (model.Password.IsNullOrEmpty())
                throw new BusException(false, "请填写密码", BusCodeType.NotAcceptable);
            if (model.Password != model.RePassword)
                throw new BusException(false, "两次密码不相同", BusCodeType.NotAcceptable);
            // var md5 = MD5.Create();
            var newUser = new User {
                Id = IdHelper.Get(),
                CreatedTime = DateTime.Now,
                CreatorId = this._currentUser.UserId,
                IsDeleted = false,
                Email = model.Email,
                Gender = model.Gender,
                NickName = model.NickName,
                Password = model.Password, //Encoding.UTF8.GetString(md5.ComputeHash(Encoding.UTF8.GetBytes(model.Password))),
                Phone = model.Phone,
                Status = DataStatus.Normal,
                UserName = model.UserName,
                TenantId = this._currentUser.TenantId,
            };
            await this.InsertAsync(newUser);
            await _organizeUserAppService.SaveAsync(new Contract.Systems.Dtos.DepartmentUserDtos.OrganizeUserSaveDto {
                OrganizeIds = model.OrganizeIds,
                UserId = newUser.Id
            });
            return;
        }

        var user = await this.FindAsync(p => p.Id == model.Id);
        if (user.IsNullOrEmpty()) {
            throw new BusException(false, "找不到相关数据", BusCodeType.NotFound);
        }
        var ret = user.UserName != model.UserName;
        user.UserName = model.UserName;
        user.Phone = model.Phone;
        user.NickName = model.NickName;
        user.Email = model.Email;
        user.Gender = model.Gender;
        user.AccessType = model.AccessType;
        if (ret) {
            var d = await FindAsync(t => t.UserName == model.UserName);
            if (d.IsNotNullOrEmpty()) {
                throw new BusException(false, "该登录账号已被占用,请换个重试!", BusCodeType.Forbidden);
            }
        }

        await _organizeUserAppService.SaveAsync(new Contract.Systems.Dtos.DepartmentUserDtos.OrganizeUserSaveDto {
            OrganizeIds = model.OrganizeIds,
            UserId = model.Id
        });
        await this.UpdateAsync(user);
    }

    public async Task<UserDto> GetUser([NotNull] string id) {

        Expression<Func<User, UserDto>> selectExpr = u => new UserDto {
            OrganizeIds = Repository.GetQueryable<OrganizeUser>().Where(p => p.UserId == u.Id).Select(t => t.OrganizeId).ToList(),
        };
        selectExpr = selectExpr.BuildExtendSelectExpre();

        var whereExpr = LinqHelper.True<User>();
        whereExpr = whereExpr.And(p => p.Id == id);
        return await GetQueryable()
            .Where(whereExpr)
            .Select(selectExpr)
            .FirstOrDefaultAsync();
    }

    [UnitOfWork]
    public async Task RemoveAsync(params string[] ids) {
        
        // 删除用户角色信息
        await this.Repository.DeleteAsync<UserRole>(t => ids.Contains(t.UserId));
        
        // 删除自身数据
        await this.DeleteAsync(t=> ids.Contains(t.Id));
    }

    public async Task ResetAsync(ResetDto input) {
        if (input.UserId.IsNullOrEmpty()) {
            input.UserId = _currentUser.UserId;
        }

        if (input.Password != input.RePassword) {
            throw new BusException(false, "两次输入的密码不一样", BusCodeType.Forbidden);
        }

        var u = await this.FindAsync(t=>t.Id == input.UserId);
        if (u.IsNullOrEmpty()) {
            throw new BusException(false, "未找到相关用户", BusCodeType.NotFound);
        }

        u.Password = input.Password;
        await UpdateAsync(u);
    }
}