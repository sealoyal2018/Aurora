namespace Aurora.Domain.Shared.Core;

/// <summary>
/// 当前在线用户
/// </summary>
public interface IUserProvider {

    /// <summary>
    /// 是否登陆验证
    /// </summary>
    public bool? IsAuthenticated { get; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }


    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 用户id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 租户id
    /// </summary>
    public string TenantId { get; set; }

    /// <summary>
    /// 是否为管理员
    /// </summary>
    public bool IsAdmin { get; }

}
