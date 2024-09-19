
using NCrontab;

namespace SupPortal.NotificationService.API.Service;

public class ProcessMailService : BackgroundService
{

    private readonly IServiceScopeFactory _scopeFactory;
     

    private Timer _timer;
    public ProcessMailService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

     }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        _timer =   new Timer( _ =>
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();
                Task.Run(() =>
                {
                    mailService.ProcessMail();

                });
            }
        }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
 
    }
}
