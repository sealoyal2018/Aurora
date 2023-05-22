namespace Aurora.Domain.Shared.Core; 

public class WebConfig {

    /// <summary>
    /// 网站ip地址
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// 超级管理员
    /// </summary>
    public string PrimaryRoleId { get; set; }
    
    /// <summary>
    /// 区级管理员
    /// </summary>
    public string SecondaryRoleId { get; set; }
}