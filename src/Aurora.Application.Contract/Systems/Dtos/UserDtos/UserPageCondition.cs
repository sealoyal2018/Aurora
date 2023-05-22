using Aurora.Domain.Shared.Pages;

namespace Aurora.Application.Contract.Systems.Dtos.UserDtos; 

public class UserPageInput : PageInputBase {
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }
    /// <summary>
    /// 号码
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 组织名称
    /// </summary>
    public string OrganizeId { get; set; }
}