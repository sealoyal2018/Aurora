using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System;

[Table("Sys_Role"), Comment("角色表"), Index(nameof(Name))]
public class Role : TenantEntityBase {
    [Required, Comment("角色名"), StringLength(50)]
    public string Name { get; set; }

    [Comment("角色描述"), StringLength(100)]
    public string? Description { get; set; }

    [Comment("排序"), Required, DefaultValue(1)]
    public int Sort { get; set; }
}
