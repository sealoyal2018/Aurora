using Aurora.Domain.System.Shared;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System; 

[Table("Sys_DataFile"), Comment("文件表")]
[Index(nameof(ParentId), nameof(ParentType), nameof(Sort))]
public class DataFile : TenantEntityBase {
    /// <summary>
    /// 关联id
    /// </summary>
    [Comment("关联id"), StringLength(19)]
    public String? ParentId { get; set; }

    /// <summary>
    /// 关联数据类型
    /// </summary>
    [Comment("关联数据类型")]
    public TableType? ParentType { get; set; }

    /// <summary>
    /// 文件类型
    /// </summary>
    [Comment("文件类型"), Required]
    public DataFileType Type { get; set; }

    /// <summary>
    /// 文件地址
    /// </summary>
    [Comment("文件地址"), Required, StringLength(100)]
    public String Uri { get; set; }
    
    /// <summary>
    /// 文件名称(不带拓展名)
    /// </summary>
    [Comment("文件名称(不带拓展名)"), Required, StringLength(50)]
    public String Name { get; set; }

    /// <summary>
    /// 文件拓展名
    /// </summary>
    [Comment("文件拓展名"), Required, StringLength(20)]
    public String Extensions { get; set; }

    /// <summary>
    /// 文件大小(字节为单位)
    /// </summary>
    [Comment("文件大小(字节为单位)"), Required]
    public long Size { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Comment("排序"), Required, DefaultValue(0)]
    public int Sort { get; set; } = 0;
}