using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System;

[Table("Sys_UserRole"), Comment("用户与角色关联表"), Index(nameof(RoleId), nameof(UserId))]
public class UserRole : TenantEntityBase {
    [Required, StringLength(19), Comment("关联角色")]
    public string RoleId { get; set; }
    [Required, StringLength(19), Comment("关联用户")]
    public string UserId { get; set; }

}
