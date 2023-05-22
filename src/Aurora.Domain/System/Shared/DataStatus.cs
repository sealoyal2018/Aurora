using System.ComponentModel;

namespace Aurora.Domain.System.Shared; 

public enum DataStatus {
    [Description("正常")]
    Normal = 0,
    [Description("禁用")]
    Disable = 1,
}