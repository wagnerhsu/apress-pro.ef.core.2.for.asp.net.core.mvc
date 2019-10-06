using DataApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataApp
{
    public class Startup
    {
        public Startup(IConfiguration config) => Configuration = config;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(x => x.EnableEndpointRouting = false).AddNewtonsoftJson(); 
            string conString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<EFDatabaseContext>(options =>
                options.UseSqlServer(conString));

            string customerConString =
                Configuration["ConnectionStrings:CustomerConnection"];
            services.AddDbContext<EFCustomerContext>(options =>
                options.UseSqlServer(customerConString));

            services.AddScoped<IDataRepository, EFDataRepository>();
            services.AddScoped<ICustomerRepository, EFCustomerRepository>();
            services.AddScoped<MigrationsManager>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IGenericRepository<ContactDetails>,
                GenericRepository<ContactDetails>>();
            services.AddScoped<IGenericRepository<ContactLocation>,
                GenericRepository<ContactLocation>>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
                EFDatabaseContext prodCtx, EFCustomerContext custCtx)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            if (env.IsDevelopment())
            {
                SeedData.Seed(prodCtx);
                SeedData.Seed(custCtx);
            }
        }
    }
}