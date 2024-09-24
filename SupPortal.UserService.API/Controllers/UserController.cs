using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SupPortal.UserService.API.Data.Repository.Abstract;
using SupPortal.UserService.API.Data.Service;
using SupPortal.UserService.API.Extension;
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
        return  (await _userService.Register(register)).ToActionResult();
    }
 
    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> UserLogin(LoginUserRequestDto request)
    {
        return  (await _userService.Login(request)).ToActionResult();
    }

    [HttpPut("update-logged-user")]
    public async Task<IActionResult> UpdateUser(UpdateUserRequestDto request)
    {
        return  (await _userService.UpdateUser(request)).ToActionResult();

    }
 

    [HttpGet("get-logged-user")]
    public async Task<IActionResult> GetUser()
    {
         return  (await _userService.GetUserInfo()).ToActionResult();
    }

    [HttpGet("user-by-username"),AllowAnonymous]

    public async Task<IActionResult> GetUser([FromQuery]string email)
    {
         return  (await _userService.GetUserInfo(email)).ToActionResult();
    }


    [HttpPost("forgot-password"), AllowAnonymous]

    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
    {
        return (await _userService.ForgotPassword(request)).ToActionResult();
    }



    [HttpPost("reset-password"), AllowAnonymous]

    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
    {
        return (await _userService.ResetPassword(request)).ToActionResult();
    }

}
