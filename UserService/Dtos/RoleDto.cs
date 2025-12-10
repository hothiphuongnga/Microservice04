using System;
using System.Collections.Generic;

namespace UserService.Dtos;

public partial class RoleDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
}
