using SupPortal.UserService.API.Models.Dto;
using SupPortal.UserService.API.Models.Entities;

namespace SupPortal.UserService.API.Data.Service;

public interface IUserService
{
    Task<LoginUserResponseDto> Login(LoginUserRequestDto request);
    Task<BaseResponse> Register(RegisterUserRequestDto requestDto);

    Task<BaseResponse> UpdateUser(UpdateUserRequestDto requestDto);

    Task<User> GetUser(string? Username = null);
}