using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace SettlementBookingSystem.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceModule(this IServiceCollection services, Assembly assembly = null)
        {
            assembly ??= Assembly.GetExecutingAssembly();
            var interfaces = assembly.GetTypes().Where(t => t.IsInterface && t.Name.StartsWith("I") && t.Name.EndsWith("Service"));
            foreach (var @interface in interfaces)
            {
                var implementation = assembly.GetTypes().FirstOrDefault(t => @interface.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                if (implementation != null)
                {
                    services.AddScoped(@interface, implementation);
                }
            }
            return services;
        }
        public static IServiceCollection AddCustomServices(this IServiceCollection services, Assembly assembly = null)
        {
            services.AddServiceModule(assembly);

            return services;
        }
    }
}
