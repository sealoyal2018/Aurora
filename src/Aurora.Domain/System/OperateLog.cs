using Aurora.Domain.System.Shared;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System;

[Table("Sys_OperateLog"), Comment("操作日志")]
[Index(nameof(Type))]
public class OperateLog : IEntity {
    
    /// <summary>
    /// id
    /// </summary>
    [Key, StringLength(19), Comment("id主键")]
    public string Id { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Required, Comment("创建时间")]
    public DateTime CreatedTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 创建人
    /// </summary>
    [StringLength(19), Comment("创建人id")]
    public string? CreatorId { get; set; }
    
    [Required, Comment("操作地址"), StringLength(200)]
    public string Url { get; set; }

    [Required, Comment("操作地址源"), StringLength(50)]
    public string IPAddress { get; set; }

    [Required, Comment("操作耗时")] 
    public int ElapsedTime { get; set; }

    /// <summary>
    /// 附加数据
    /// </summary>
    [Comment("附加数据"), StringLength(500)]
    public string Tag { get; set; }

    /// <summary>
    /// 日志类型,0:操作日志, 1:异常日志
    /// </summary>
    [Required, Comment("日志类型,0:操作日志, 1:异常日志")]
    public SystemLogType Type { get; set; }
}