using Microsoft.AspNetCore.Mvc;
using UserService.Dtos;
using UserService.Services;
using UserService.Services.Base;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _userService.GetAllAsync();
        return result;
    }

    // GET: api/users/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _userService.GetByIdAsync(id);
        return result;
    }


    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> CreateUser(UserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return ResponseEntity.Fail("Invalid model state", 400);
        }

        var result = await _userService.AddAsync(userDto);
        return result;
    }

    // PUT: api/users/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
    {
        if (id != userDto.Id)
        {
            return ResponseEntity.Fail("ID mismatch", 400);
        }

        var result = await _userService.UpdateAsync(userDto);
        return result;
    }

    // DELETE: api/users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteAsync(id);
        return result;
    }
}
