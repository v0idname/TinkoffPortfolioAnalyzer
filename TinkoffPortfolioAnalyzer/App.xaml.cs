using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using TinkoffPortfolioAnalyzer.Data;
using TinkoffPortfolioAnalyzer.Data.Repositories;
using TinkoffPortfolioAnalyzer.Services;
using TinkoffPortfolioAnalyzer.ViewModels;

namespace TinkoffPortfolioAnalyzer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IHost __Host;

        public static IHost Host => __Host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;

            using var scopedServices = host.Services.CreateScope();
            await scopedServices.ServiceProvider.GetDatabaseContext().Database.MigrateAsync();
            //await scopedServices.ServiceProvider.GetRequiredService<DbInitializer>().InitAsync();

            base.OnStartup(e);
            await host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            var host = Host;
            await host.StopAsync().ConfigureAwait(false);
            host.Dispose();
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddDatabase(host.Configuration.GetSection("Database"));
            services.AddScoped<IConnectionService, TinkConnectionService>();
            services.AddScoped<IDataService, TinkDataService>();
            //services.AddScoped<ISnapshotsRepository, SnapshotsXmlRepository>();
            services.AddScoped<ISnapshotsRepository, SnapshotsDbRepository>();
            //services.AddScoped<ITokensRepository, TokensXmlRepository>();
            services.AddScoped<ITokensRepository, TokensDbRepository>();
            services.AddScoped<MainWindowViewModel>();
            services.AddScoped<AvailSecuritiesViewModel>();
            services.AddScoped<TokensManagementViewModel>();
            //services.AddTransient<DbInitializer>();
        }
    }
}
