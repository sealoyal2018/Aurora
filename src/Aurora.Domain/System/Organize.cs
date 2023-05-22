using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System; 

[Table("Sys_Organize"), Comment("组织表")]
[Index(nameof(Name), nameof(ParentId))]
public class Organize: TenantEntityBase {
    /// <summary>
    /// 组织名称
    /// </summary>
    [Comment("组织名称"), Required, StringLength(20)]
    public string Name { get; set; }

    /// <summary>
    /// 上级组织
    /// </summary>
    [Comment("上级组织"), StringLength(19)]
    public string? ParentId { get; set; }

    /// <summary>
    /// 组织id连接字符串
    /// </summary>
    [Comment("组织id连接字符串"), StringLength(200), Required]
    public string ParentNum { get; set; }

    [Required, Comment("排序"), DefaultValue(0)]
    public int Sort { get; set; }
}