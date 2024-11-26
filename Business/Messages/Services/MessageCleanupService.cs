using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stretch4U.Business.Messages.Services;

public class MessageCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public MessageCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();

                await messageService.DeleteOldMessages();
            }

            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}
