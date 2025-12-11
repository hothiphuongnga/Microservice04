using System;
using System.Collections.Generic;

namespace UserService.Dtos;

public class UserRoleDto
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual RoleDto Role { get; set; } = null!;
}
