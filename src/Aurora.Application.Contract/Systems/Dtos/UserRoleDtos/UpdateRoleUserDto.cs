using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Application.Contract.Systems.Dtos.UserRoleDtos;
public class UpdateRoleUserDto {
    [Required]
    public string UserId { get; set; }
    [Required]
    public string RoleId { get; set; }

    public bool IsRemove { get; set; } = false;
}
