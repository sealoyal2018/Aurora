using System.ComponentModel;

namespace Aurora.Domain.System.Shared;
public enum DataAccessType {

    /// <summary>
    /// 仅管理自己的数据
    /// </summary>
    [Description("仅管理自己的数据")]
    Self = 0,

    /// <summary>
    /// 管理所属区域的数据
    /// </summary>
    [Description("管理所属区域的数据")]
    Private = 1,

    /// <summary>
    /// 管理所属区域及其子域数据
    /// </summary>
    [Description("管理所属区域及其子域数据")]
    Protected = 2,
}
