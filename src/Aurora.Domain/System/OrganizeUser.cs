using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System;

[Table("Sys_OrganizeUser"), Comment("组织与用户关联表")]
[Index(nameof(OrganizeId), nameof(UserId))]
public class OrganizeUser : TenantEntityBase {
    [Comment("关联组织"), Required, StringLength(19)]
    public string OrganizeId { get; set; }

    [Comment("关联用户"), Required, StringLength(19)]
    public string UserId { get; set; }
}