using System;
using System.Windows;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ReactiveUI;

using Serilog;
using Serilog.Events;

using SimpleViewer.Shell;

using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.Logging;

namespace SimpleViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .WriteTo.Async(c => c.Debug())
                            .WriteTo.Async(c => c.File("Logs/logs.txt"))
                            .CreateLogger();

            try
            {
                Log.Information("Starting host");
                IHost host = Host.CreateDefaultBuilder()
                             .UseSerilog()
                             .ConfigureLogging(loggingBuilder=> loggingBuilder.AddSplat())
                             .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                             .ConfigureContainer<ContainerBuilder>((hostcontext, builder) =>
                             {
                                 builder.RegisterType<ShellView>().PropertiesAutowired();
                                 builder.RegisterType<ShellViewModel>().PropertiesAutowired();
                             })
                             .ConfigureServices((hostContext, services) =>
                            {
                                services.UseMicrosoftDependencyResolver();
                                var resolver = Locator.CurrentMutable;
                                resolver.InitializeSplat();
                                resolver.InitializeReactiveUI();
                            })
                            .Build();

                host.Services.UseMicrosoftDependencyResolver();
                host.Services.GetRequiredService<ShellView>()?.Show();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Log.CloseAndFlush();
        }
    }
}
