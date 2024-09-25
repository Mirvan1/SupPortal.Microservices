using MassTransit;
using Microsoft.EntityFrameworkCore;
using SupPortal.NotificationService.API.Consumers;
using SupPortal.NotificationService.API.Repository.Abstract;
using SupPortal.NotificationService.API.Repository.Concrete;
using SupPortal.NotificationService.API.Service;
using SupPortal.Shared;
using SupPortal.Shared.Events;
using System.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using SupPortal.NotificationService.API.Data;
using Microsoft.Extensions.Options;
using SupPortal.NotificationService.API.Models.DTOs;
using Serilog;

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


// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<nsContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddScoped<IAuthSettings, AuthSettings>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<MailNotificationConsumer>();

    cfg.UsingRabbitMq((ct, cf) =>
    {
        cf.Host(builder.Configuration["RabbitMQConfig:Host"], x =>
        {
            x.Username(builder.Configuration["RabbitMQConfig:Username"]);
            x.Password(builder.Configuration["RabbitMQConfig:Password"]);
        });

        cf.ReceiveEndpoint(
            MQSettings.CreateTicketEvent,
            e =>
            {
                //e.ConfigureConsumeTopology = false; // Disable default bindings

                e.ConfigureConsumer<MailNotificationConsumer>(ct);
                //e.Bind<CreateTicketEvent>();
            });

        cf.ReceiveEndpoint(
            MQSettings.CreateCommentEvent,
            e =>
            {
                e.ConfigureConsumer<MailNotificationConsumer>(ct);
            }
    );

        cf.ReceiveEndpoint(
         MQSettings.UpdateTicketEvent,
         e =>
         {
             e.ConfigureConsumer<MailNotificationConsumer>(ct);
         }
 );


        cf.ReceiveEndpoint(
        MQSettings.ForgotPasswordCommand,
        e =>
        {
            e.ConfigureConsumer<MailNotificationConsumer>(ct);
        }
        );



        cf.ReceiveEndpoint(
        MQSettings.ResetPasswordInfoCommand,
        e =>
        {
            e.ConfigureConsumer<MailNotificationConsumer>(ct);
        }
        );

    });
});

     var mailSettingsInstance = new MailSettingsDto();
    var mailSettings = builder.Configuration.GetSection("MailSettings");

    mailSettings.Bind(mailSettingsInstance);
     builder.Services.AddSingleton<IOptions<MailSettingsDto>>(new OptionsWrapper<MailSettingsDto>(mailSettingsInstance));


    builder.Services.AddHostedService<ProcessMailService>();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.UseHttpsRedirection();


    app.MapControllers();


    app.Run();
