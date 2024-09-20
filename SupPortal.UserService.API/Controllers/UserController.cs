using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SupPortal.UserService.API.Data.Repository.Abstract;
using SupPortal.UserService.API.Data.Service;
using SupPortal.UserService.API.Models.Dto;

namespace SupPortal.UserService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserService _userService) : ControllerBase
{

    [HttpPost("register"),AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto register)
    {
        return Ok(await _userService.Register(register));
    }
 
    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> UserLogin(LoginUserRequestDto request)
    {
        return Ok(await _userService.Login(request));
    }

    [HttpPut("update-logged-user")]
    public async Task<IActionResult> UpdateUser(UpdateUserRequestDto request)
    {
        return Ok(await _userService.UpdateUser(request));

    }
 

    [HttpGet("get-logged-user")]
    public async Task<IActionResult> GetUser()
    {
        var res = await _userService.GetUser();
        return Ok(res);
    }

    [HttpGet("user-by-email"),AllowAnonymous]

    public async Task<IActionResult> GetUser([FromQuery]string email)
    {
        var res = await _userService.GetUser(email);
        return Ok(res);
    }

}
