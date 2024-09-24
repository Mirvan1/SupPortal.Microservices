using System.Reflection;
using System.Text;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SupPortal.UserService.API.Data.Context;
using SupPortal.UserService.API.Data.Repository.Abstract;
using SupPortal.UserService.API.Data.Repository.Concrete;
using SupPortal.UserService.API.Data.Service;
using SupPortal.UserService.API.Extension;
using SupPortal.UserService.API.Models.Dto;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var logger = new LoggerConfiguration()
   .ReadFrom.Configuration(
    new ConfigurationBuilder().AddJsonFile("serilog.json").Build()
    )
   .Enrich.FromLogContext()
   .CreateLogger();

builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorize header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
     c.OperationFilter<SecurityRequirementsOperationFilter>();
});


builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<userDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ));

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddFluentValidation(fv => {
    fv.RegisterValidatorsFromAssemblyContaining<Program>();
 
    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {

        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["secret-key"]))

    };

});
builder.Services.AddAuthorization();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));


builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((ct, cf) =>
    {
        cf.Host(builder.Configuration["RabbitMQConfig:Host"], x =>
        {
            x.Username(builder.Configuration["RabbitMQConfig:Username"]);
            x.Password(builder.Configuration["RabbitMQConfig:Password"]);
        });

    });

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1"));
}
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllers();

app.Run();
