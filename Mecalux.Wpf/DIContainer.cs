using Mecalux.Wpf.Interfaces;
using Mecalux.Wpf.Services;
using Mecalux.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Mecalux.Wpf
{
    public static class DIContainer
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        public static void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            // Register main services and view models
            serviceCollection.AddSingleton<MainWindow>();
            serviceCollection.AddSingleton<MainWindowViewModel>();

            // Other services needed
            serviceCollection.AddTransient<IDataService, DataService>();
            serviceCollection.AddScoped(sp => new HttpClient() { BaseAddress = new Uri(uriString: $"https://localhost:7265/api/") });

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}