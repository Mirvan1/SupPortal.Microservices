using MassTransit;
using Microsoft.EntityFrameworkCore;
using SupPortal.NotificationService.API.Consumers;
using SupPortal.NotificationService.API.Models;
using SupPortal.NotificationService.API.Repository.Abstract;
using SupPortal.NotificationService.API.Repository.Concrete;
using SupPortal.NotificationService.API.Service;
using SupPortal.Shared;
using SupPortal.Shared.Events;
using System.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<nsContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMailService, MailService>();
//builder.Services.AddTransient<IHostedService, ProcessMailService>();

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

    });
});


builder.Services.AddHostedService<ProcessMailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
