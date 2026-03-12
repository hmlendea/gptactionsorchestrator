using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NuciLog;
using NuciLog.Configuration;
using NuciLog.Core;
using GptActionsOrchestrator.Configuration;
using GptActionsOrchestrator.Service;
using GptActionsOrchestrator.Integrations.PersonalLogManager.Configuration;
using GptActionsOrchestrator.Integrations.PersonalLogManager.Service;

namespace GptActionsOrchestrator
{
    public static class ServiceCollectionExtensions
    {
        static SecuritySettings securitySettings;
        static NuciLoggerSettings loggingSettings;
        static PersonalLogManagerSettings personalLogManagerSettings;

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            securitySettings = new SecuritySettings();
            loggingSettings = new NuciLoggerSettings();
            personalLogManagerSettings = new PersonalLogManagerSettings();

            configuration.Bind(nameof(SecuritySettings), securitySettings);
            configuration.Bind(nameof(NuciLoggerSettings), loggingSettings);
            configuration.Bind(nameof(PersonalLogManagerSettings), personalLogManagerSettings);

            services.AddSingleton(securitySettings);
            services.AddSingleton(loggingSettings);
            services.AddSingleton(personalLogManagerSettings);

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services) => services
            .AddSingleton<IActionsOrchestrator, ActionsOrchestrator>()
            .AddSingleton<IPersonalLogManagerService, PersonalLogManagerService>()
            .AddSingleton<ISteamStoreService, SteamStoreService>()
            .AddSingleton<ILogger, NuciLogger>();
    }
}
