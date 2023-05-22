using System.ComponentModel;

namespace Aurora.Domain.System.Shared; 

/// <summary>
/// 系统日志类型
/// </summary>
public enum SystemLogType {
    /// <summary>
    /// 操作日志
    /// </summary>
    [Description("操作日志")]
    Operator = 0,
    /// <summary>
    /// 异常日志
    /// </summary>
    [Description("异常日志")]
    Exception = 1,
}