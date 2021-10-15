using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace TinkoffPortfolioAnalyzer
{

    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }

        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host_builder = Host.CreateDefaultBuilder(args);
            host_builder.UseContentRoot(Environment.CurrentDirectory);
            host_builder.ConfigureAppConfiguration((host, cfg) =>
            {
                cfg.SetBasePath(Environment.CurrentDirectory);
                cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            });
            host_builder.ConfigureServices(App.ConfigureServices);
            return host_builder;
        }
    }
}
