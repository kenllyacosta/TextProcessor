using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Mecalux.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure DI
            DIContainer.ConfigureServices();

            // Resolve the main window and show it
            var mainWindow = DIContainer.ServiceProvider?.GetRequiredService<MainWindow>();
            mainWindow?.Show();
        }
    }
}