using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System;


[Table("Sys_RoleResource"), Comment("角色与权限关联表"), Index(nameof(RoleId), nameof(ResourceId))]
public class RoleResource : EntityBase {
    [Required, StringLength(19), Comment("关联角色")]
    public string RoleId { get; set; }

    [Required, StringLength(19), Comment("关联资源")]
    public string ResourceId { get; set; }
}
