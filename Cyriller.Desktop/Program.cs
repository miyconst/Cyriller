using System;
using Microsoft.Extensions.DependencyInjection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using Cyriller.Desktop.ViewModels;
using Cyriller.Desktop.Views;
using Cyriller.Desktop.Models;

namespace Cyriller.Desktop
{
    public class Program
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp().Start(AppMain, args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseDataGrid()
                .LogToDebug()
                .UseReactiveUI();

        // Your application's entry point. Here you can initialize your MVVM framework, DI
        // container, etc.
        private static void AppMain(Application app, string[] args)
        {
            ServiceProvider = new ServiceCollection()
                .AddSingleton<Application>(app)
                .AddSingleton<CyrCollectionContainer>()

                .AddSingleton<MainWindowViewModel>()
                .AddSingleton<MainWindow>()

                .AddTransient<NounViewModel>()
                .AddTransient<AdjectiveViewModel>()

                .BuildServiceProvider();

            MainWindow window = ServiceProvider.GetService<MainWindow>();

            app.Run(window);
        }
    }
}
