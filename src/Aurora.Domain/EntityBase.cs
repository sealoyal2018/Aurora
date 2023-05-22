using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Aurora.Domain;

/// <summary>
/// 基础实体
/// </summary>
[Index(nameof(Id), nameof(CreatedTime))]
public abstract class EntityBase : IEntity {
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
    [Required, StringLength(19), Comment("创建人id")]
    public string CreatorId { get; set; }

    /// <summary>
    /// 软删除
    /// </summary>
    [Required, Comment("软删除")]
    public bool IsDeleted { get; set; } = false;
}
