using System;
using System.Collections.Generic;

namespace UserService.Dtos;

public partial class UserDto
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();
}
