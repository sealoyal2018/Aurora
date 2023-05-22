using Aurora.Application.Contract.Systems.Dtos.UserDtos;
using Aurora.Domain.Shared.Pages;
using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems; 

public interface IUserAppService : IAppService<User> {

    Task<CaptchaDto> GetCaptchaAsync();

    Task<PageResult<UserDto>> GetUsers(UserPageInput pageInput);

    Task CreateUpdateUserAsync(CreateUpdateUserDto model);
    Task<UserDto> GetUser(string id);

    Task RemoveAsync(params string[] ids);
    Task ResetAsync(ResetDto input);
}