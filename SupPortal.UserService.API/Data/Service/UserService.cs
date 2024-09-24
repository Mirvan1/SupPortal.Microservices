using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SupPortal.UserService.API.Data.Repository.Abstract;
using SupPortal.UserService.API.Models.Entities;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using SupPortal.UserService.API.Models.Dto;
using SupPortal.UserService.API.Extension;
using MassTransit;
using Newtonsoft.Json.Linq;
using System.Web;
using SupPortal.Shared;
using SupPortal.Shared.Command;

namespace SupPortal.UserService.API.Data.Service;

public class UserService(IHttpContextAccessor _httpContextAccessor, IMapper _mapper, IUserRepository _userRepository, ILogger<UserService> _logger, IBus _bus, IConfiguration _configuration) : IUserService
{

    public async Task<LoginUserResponseDto> Login(LoginUserRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) && string.IsNullOrWhiteSpace(request.Password))
            return BaseResponse.Response<LoginUserResponseDto>(false);

        var getUser = await _userRepository.GetUserByUsernameAsync(request.Username, true);

        if (getUser is null) return BaseResponse.ErrorResponse<LoginUserResponseDto>(ConstantErrorMessages.NotFound);

        if (!getUser.IsActive) return BaseResponse.ErrorResponse<LoginUserResponseDto>(ConstantErrorMessages.BadRequest);
        if (!getUser.Username.Equals(request.Username)) return BaseResponse.ErrorResponse<LoginUserResponseDto>(ConstantErrorMessages.BadRequest);

        bool passwordMatching = BCrypt.Net.BCrypt.Verify(request.Password, getUser.Password);
        if (!passwordMatching) return BaseResponse.ErrorResponse<LoginUserResponseDto>(ConstantErrorMessages.BadRequest);

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
            Role = await _userRepository.GetStandUserRole()
        };

        await _userRepository.AddAsync(user);
        int res = await _userRepository.SaveChangesAsync();

        return BaseResponse.Response(res > 0);
    }



    public async Task<BaseResponse> UpdateUser(UpdateUserRequestDto requestDto)
    {
        User getUser = await GetUser();


        if (getUser is null) return BaseResponse.Response(false);

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

    public async Task<GetUserDto> GetUserInfo(string? Username = null)
    {
        if (string.IsNullOrEmpty(Username))
        {
            Username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }
        var user = await _userRepository.GetUserByUsernameAsync(Username);
        return new GetUserDto(user.Username, user.Email, user.FirstName, user.LastName, user.IsActive) { IsSuccess = true };
    }


    public async Task<BaseResponse> ForgotPassword(ForgotPasswordRequestDto request)
    {
        var getUser = await _userRepository.GetUserByEmailAsync(request.Email, true);

        if (getUser is null) return BaseResponse.ErrorResponse(ConstantErrorMessages.NotFound);

        var getClient = _configuration.GetValue<string>("clientEndpoint");
        var clientUrl = new UriBuilder(getClient);

        var queryParam = HttpUtility.ParseQueryString(clientUrl.Query);
        queryParam.Add("email", getUser.Email);
        queryParam.Add("value", CreateToken(getUser));

        clientUrl.Query = queryParam.ToString();

        var sendMail = new ForgotPasswordCommand()
        {
            Email = getUser.Email,
            Username = getUser.Username,
            ResetPasswordUrl = clientUrl.ToString()
        };

        var bus = BusConfigurator.ConfigureBus();//without outbox pattern
        var sendEndpoint = await bus.GetSendEndpoint
            (new Uri($"{_configuration.GetSection("RabbitMQConfig:Host").Value}/{MQSettings.ForgotPasswordCommand}"));
        await sendEndpoint.Send(sendMail);

        return BaseResponse.Response(true);
    }

    public async Task<BaseResponse> ResetPassword(ResetPasswordRequestDto request)
    {
        if (!request.Password.Equals(request.ConfirmPassword)) return BaseResponse.ErrorResponse(ConstantErrorMessages.BadRequest);

        var getUserFromJwt = ReadToken(request.Token);

        if (string.IsNullOrWhiteSpace(getUserFromJwt)) return BaseResponse.ErrorResponse(ConstantErrorMessages.BadRequest);

        var getUser = await _userRepository.GetUserByUsernameAsync(getUserFromJwt);

        if (getUser is null) return BaseResponse.ErrorResponse(ConstantErrorMessages.NotFound);

        getUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

        _userRepository.Update(getUser);
        int res = await _userRepository.SaveChangesAsync();


        var sendMail = new ResetPasswordCommand()
        {
            Email = getUser.Email,
            Username = getUser.Username,
            IpAddress=_httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString()??""
        };
        var bus = BusConfigurator.ConfigureBus();//without outbox pattern
        var sendEndpoint = await bus.GetSendEndpoint
            (new Uri($"{_configuration.GetSection("RabbitMQConfig:Host").Value}/{MQSettings.ResetPasswordInfoCommand}"));
        await sendEndpoint.Send(sendMail);


        return BaseResponse.Response(res > 0);

    }
    private async Task<User> GetUser(string? Username = null)
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
        string role = user.Role.Name.ToString("D");
        List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.Role, role) };
        string secretKey = _configuration["secret-key"];


        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(3), signingCredentials: cred);
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
 
    private static string? ReadToken(string Token)
    {
        var handler = new JwtSecurityTokenHandler();
        var readToken = handler.ReadJwtToken(Token);
        return readToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
    }

}
