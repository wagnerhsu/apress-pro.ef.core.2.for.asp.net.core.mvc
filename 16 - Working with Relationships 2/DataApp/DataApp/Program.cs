using DataApp.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DataApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            EnsureDatabaseExist(host);
            host.Run();
        }

        private static void EnsureDatabaseExist(IWebHost host)
        {
            var scope = host.Services.CreateScope();
            var customerContext = scope.ServiceProvider.GetRequiredService<EFCustomerContext>();
            customerContext.Database.EnsureCreated();

            var dataContext = scope.ServiceProvider.GetRequiredService<EFDatabaseContext>();
            dataContext.Database.EnsureCreated();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}