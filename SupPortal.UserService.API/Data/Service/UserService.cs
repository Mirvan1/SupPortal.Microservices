using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SupPortal.UserService.API.Data.Repository.Abstract;
using SupPortal.UserService.API.Models.Entities;
 using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using SupPortal.UserService.API.Models.Dto;

namespace SupPortal.UserService.API.Data.Service;

public class UserService(IHttpContextAccessor _httpContextAccessor, IMapper _mapper, IUserRepository _userRepository,ILogger<UserService> _logger) :IUserService
{

    public async Task<LoginUserResponseDto> Login(LoginUserRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Password))
            return BaseResponse.Response<LoginUserResponseDto>(false);

        var getUser = await _userRepository.GetUserByUsernameAsync(request.Username,true);

        if (getUser is null) return BaseResponse.Response<LoginUserResponseDto>(false);

        if (!getUser.IsActive) return BaseResponse.Response<LoginUserResponseDto>(false);
        if (!getUser.Username.Equals(request.Username)) return BaseResponse.Response<LoginUserResponseDto>(false);

        bool passwordMatching = BCrypt.Net.BCrypt.Verify(request.Password, getUser.Password);
        if (!passwordMatching) return BaseResponse.Response<LoginUserResponseDto>(false);

        getUser.Profile.LastLogin = DateTime.Now;
        _userRepository.Update(getUser);
       await _userRepository.SaveChangesAsync();

        _logger.LogInformation("");

        return new LoginUserResponseDto() { IsSuccess = true, Token = CreateToken(getUser) };
    }


    public async Task<BaseResponse> Register(RegisterUserRequestDto requestDto)
    {
        var checkUserExist = await _userRepository.GetUserByEmailAsync(requestDto.Email);
        if (checkUserExist is not null) return BaseResponse.Response(false);


        var user = new User()
        {
            Email = requestDto.Email,
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            Password = BCrypt.Net.BCrypt.HashPassword(requestDto.Password),
            IsActive = true,
            Username = requestDto.UserName,
            Profile = new UserProfile()
            {
                DateOfBirth = requestDto.BirthDate,
                CreatedAt = DateTime.Now,
            },
           Role=await _userRepository.GetStandUserRole()
        };

        await _userRepository.AddAsync(user);
        int res = await _userRepository.SaveChangesAsync();

        return BaseResponse.Response(res > 0);
    }



    public async Task<BaseResponse> UpdateUser(UpdateUserRequestDto requestDto)
    {
        User getUser = await GetUser();


        if (getUser is null) return BaseResponse.Response(false) ;

        if (!string.IsNullOrWhiteSpace(requestDto?.Email) && !getUser.Email.Equals(requestDto.Email)) getUser.Email = requestDto.Email;
        if (!string.IsNullOrWhiteSpace(requestDto?.FirstName) && !getUser.FirstName.Equals(requestDto.FirstName)) getUser.FirstName = requestDto.FirstName;
        if (!string.IsNullOrWhiteSpace(requestDto?.LastName) && !getUser.LastName.Equals(requestDto.LastName)) getUser.LastName = requestDto.LastName;
        if (!string.IsNullOrWhiteSpace(requestDto?.UserName) && !getUser.Username.Equals(requestDto.UserName)) getUser.Username = requestDto.UserName;
        if (!string.IsNullOrWhiteSpace(requestDto?.Password)) getUser.Password = BCrypt.Net.BCrypt.HashPassword(requestDto.Password);
 
        if (requestDto?.IsActive != null) getUser.IsActive = (bool)requestDto.IsActive;


        _userRepository.Update(getUser);
        int res = await _userRepository.SaveChangesAsync();
        return BaseResponse.Response(res > 0);
    }


    public async Task<User> GetUser(string? Username=null)
    {
        if (string.IsNullOrEmpty(Username))
        {
            Username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        }
        return await _userRepository.GetUserByUsernameAsync(Username);

    }




    private static string CreateToken(User user)
    {
        var builder = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfigurationRoot _configuration = builder.Build();
        string role =user.Role.Name.ToString("D");
        List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.Role,role) };
        string secretKey = _configuration["secret-key"];
                                                        

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(3), signingCredentials: cred);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }


    
}
