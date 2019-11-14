using MeeHealth.NetStandard.NLogComponent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;
using DataApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WpfAppTest01
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NLogService nLogService = new NLogService(Directory.GetCurrentDirectory());
            var host = nLogService.BuildHost(CreateHostBuilder);
            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private IConfiguration Configuration { get; set; }
        public IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x=>Configuration = x.Build())
                .ConfigureServices(ConfigureServices);

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EFDatabaseContext>(option =>
                {
                    option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                });
            services.AddScoped(typeof(MainWindow));
        }
    }
}