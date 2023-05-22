using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System;

[Table("Sys_Resource"), Comment("权限资源表"), Index(nameof(Name), nameof(ParentId))]
public class Resource : EntityBase {
    [Comment("名称"), Required, StringLength(50)]
    public string Name { get; set; }

    [Comment("地址"), StringLength(50)]
    public string? Url { get; set; }

    [Comment("图标"), StringLength(100)]
    public string? Icon { get; set; }

    [Comment("上级 id"), StringLength(19)]
    public string? ParentId { get; set; }

    [Comment("资源类型"), Required, DefaultValue(ResourceType.Directory)]
    public ResourceType Type { get; set; }

    [Required, Comment("是否为外链"), DefaultValue(false)]
    public bool IsOutside { get; set; }

    [Comment("权限码"), StringLength(50)]
    public string? PermissionCode { get; set; }

    [Required, StringLength(200), Comment("父级id连接")]
    public string ParentNum { get; set; }

    [Required, Comment("排序"), DefaultValue(0)]
    public int Sort { get; set; } = 0;

}


/// <summary>
/// 资源类型
/// </summary>
public enum ResourceType {
    [Description("目录")]
    Directory = 0,

    [Description("菜单")]
    Menu = 1,

    [Description("按钮")]
    Action = 2,
}