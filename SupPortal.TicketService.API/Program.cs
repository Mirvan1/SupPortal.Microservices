using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Server;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Filters;
using SupPortal.Shared;
using SupPortal.Shared.Events;
using SupPortal.TicketService.API.ApplicationCore.Dtos.Request;
using SupPortal.TicketService.API.ApplicationCore.Interface;
using SupPortal.TicketService.API.ApplicationCore.Jobs;
using SupPortal.TicketService.API.ApplicationCore.Services;
using SupPortal.TicketService.API.Infrastructure.Data;
using SupPortal.TicketService.API.Infrastructure.Extension;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
var logger = new LoggerConfiguration()
   .ReadFrom.Configuration(
    new ConfigurationBuilder().AddJsonFile("serilog.json").Build()
    )
   .Enrich.FromLogContext()
   .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddFluentValidationAutoValidation();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {

        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["secret-key"]))

    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token successfully validated.");
            return Task.CompletedTask;
        }
    };

});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDIRegistration();

builder.Services.AddDbContext<tsContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ticket API", Version = "v1" });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standart Authorize header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

});
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((ct, cf) =>
    {
        cf.Host(builder.Configuration["RabbitMQConfig:Host"], x =>
        {
            x.Username(builder.Configuration["RabbitMQConfig:Username"]);
            x.Password(builder.Configuration["RabbitMQConfig:Password"]);
        });

        //cf.Message<CreateTicketEvent>(x =>
        //{
        //    x.SetEntityName(MQSettings.CreateTicketEvent);
        //});




    });

});


builder.Services.AddHangfire(configuration => configuration
 .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfireServer();



builder.Services.AddTransient<OutboxPublishJob>();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    //.AddSqlServer(builder.Configuration.GetConnectionString("HangfireConnection"));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var jobScheduler = scope.ServiceProvider.GetRequiredService<OutboxPublishJob>();
    jobScheduler.TicketOutboxJob();
}


//
// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHangfireDashboard();
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
