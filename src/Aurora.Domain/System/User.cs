using Aurora.Domain.System.Shared;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aurora.Domain.System;

[Table("Sys_User"), Comment("用户表"), Index(nameof(UserName), nameof(Sort))]
public class User : TenantEntityBase {
    [StringLength(20), Required, Comment("用户名")]
    public string UserName { get; set; }

    [Required, StringLength(20), Comment("昵称")]
    public string? NickName { get; set; }

    [StringLength(50), Required, Comment("密码")]
    public string Password { get; set; }

    [StringLength(50), Comment("邮箱")]
    public string? Email { get; set; }

    [StringLength(11), Comment("电话号码")]
    public string? Phone { get; set; }

    [Required, Comment("性别"), DefaultValue(GenderType.Male)]
    public GenderType Gender { get; set; }
    
    [Required, Comment("数据状态"), DefaultValue(DataStatus.Normal)]
    public DataStatus Status { get; set; }

    [Required, Comment("排序"), DefaultValue(0)]
    public int Sort { get; set; }

    [Required, Comment("数据权限类型")]
    public DataAccessType AccessType { get; set; }
}
