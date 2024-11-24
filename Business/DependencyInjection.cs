using Microsoft.Extensions.DependencyInjection;
using Stretch4U.Business.Messages.Services;
using Stretch4U.Business.Users.Services;

namespace Stretch4U.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services) =>
        services.AddTransient<IUserService, UserService>()
                .AddTransient<IMessageService, MessageService>();   
}
