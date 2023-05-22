using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System;

[Table("Sys_Tenant"), Comment("租户表"), Index(nameof(Tenant.Sort))]
public class Tenant : EntityBase {

    [Required, StringLength(50), Comment("名称")]
    public string Name { get; set; }

    public string Describe { get; set; }

    [Required, Comment("排序")]
    public int Sort { get; set; }

    [StringLength(19), Comment("上级租户")]
    public string? ParentId { get; set; }

    [Required, StringLength(1000), Comment("租户连接字符")]
    public string ParentNum { get; set; }
}
