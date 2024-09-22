# SupPortal Microservices
Developed microservices using Saga-Choreography, and Inbox-Outbox patterns for eventual consistency. Integrated MassTransit with RabbitMQ for message brokering and consistent data changes.

## SupPortal.Microservices.Gateway.API
Implemented API Gateway using Ocelot library with authentication, authorization, Swagger merging, and Health Check UI.

## SupPortal.TicketService.API
Developed CQRS pattern with MediatR. Technologies: MassTransit, Hangfire, FluentValidation, MediatR, AutoMapper, Serilog, Health Check and etc.

## SupPortal.UserService.API
Developed User Service using Repository Pattern. Implemented JWT authentication, FluentValidation, Serilog, Health Check and etc.

## SupPortal.NotificationService.API
Built Notification Service with Repository Pattern, consuming messages via message brokers, background service, Serilog, and Health Check. Sent emails via MailKit library with mail templates
