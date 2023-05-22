using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Aurora.Domain;
public abstract class TenantEntityBase : EntityBase, ITenantEntity {

    [Required, StringLength(19), Comment("关联租户")]
    public string TenantId { get; set; }
}
