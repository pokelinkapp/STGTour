using GTour.Abstractions;
using GTour.Abstractions.JsInterop;
using GTour.Interops;
using Microsoft.Extensions.DependencyInjection;

namespace GTour {
    public static class ServiceExtension {
        public static IServiceCollection UseGTour(this IServiceCollection serviceCollection) {
            serviceCollection.AddSingleton<IGTourService, GTourService>();

            serviceCollection.AddScoped<IJsInteropPopper, JsInteropPopper>();
            serviceCollection.AddScoped<IJsInteropCommon, JsInteropCommon>();

            return serviceCollection;
        }
    }
}
