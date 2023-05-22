namespace Aurora.Domain.Shared.Cons;

public static class CacheCons {
    /// <summary>
    /// 图形验证码key, 需要配置对应的id
    /// </summary>
    public const string Captchakey = "cache_captcha_{0}";

    /// <summary>
    /// 密码md5加密key
    /// </summary>
    public const string PasswordMd5Key = "sealoyal@hotmail.com";
    
    /// <summary>
    /// 权限缓存key
    /// </summary>
    public const string PermissionKey = "AttractSystem_Permission_{0}";

    /// <summary>
    /// 租户id
    /// </summary>
    public const string TenantKey = "AttractSystem_Tenant_{0}";
}