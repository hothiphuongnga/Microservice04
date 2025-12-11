using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

public class UserLoginDTO
{
    [Required(ErrorMessage = "Username không được để trống")]
    public string Username { get; set; } = null!;
    [Required(ErrorMessage = "Password không được để trống")]
    public string Password { get; set; } = null!;
}

public class UserRegisterDTO
{
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;
}