<Query Kind="Program">
  <Reference>&lt;ProgramFilesX64&gt;\dotnet\shared\Microsoft.AspNetCore.App\3.0.0\Microsoft.AspNetCore.Diagnostics.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\dotnet\shared\Microsoft.AspNetCore.App\3.0.0\Microsoft.AspNetCore.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\dotnet\shared\Microsoft.AspNetCore.App\3.0.0\Microsoft.AspNetCore.Hosting.Abstractions.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\dotnet\shared\Microsoft.AspNetCore.App\3.0.0\Microsoft.AspNetCore.Hosting.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\dotnet\shared\Microsoft.AspNetCore.App\3.0.0\Microsoft.AspNetCore.Http.Abstractions.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\dotnet\shared\Microsoft.AspNetCore.App\3.0.0\Microsoft.AspNetCore.Http.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\dotnet\shared\Microsoft.AspNetCore.App\3.0.0\Microsoft.AspNetCore.Mvc.Core.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\dotnet\shared\Microsoft.AspNetCore.App\3.0.0\Microsoft.AspNetCore.Mvc.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\dotnet\shared\Microsoft.AspNetCore.App\3.0.0\Microsoft.AspNetCore.Mvc.ViewFeatures.dll</Reference>
  <Reference>&lt;ProgramFilesX64&gt;\dotnet\shared\Microsoft.AspNetCore.App\3.0.0\Microsoft.AspNetCore.Routing.dll</Reference>
  <NuGetReference>Microsoft.EntityFrameworkCore.InMemory</NuGetReference>
  <NuGetReference>Microsoft.EntityFrameworkCore.SqlServer</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Configuration</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Logging</NuGetReference>
  <Namespace>Microsoft.AspNetCore</Namespace>
  <Namespace>Microsoft.AspNetCore.Builder</Namespace>
  <Namespace>Microsoft.AspNetCore.Hosting</Namespace>
  <Namespace>Microsoft.AspNetCore.Http</Namespace>
  <Namespace>Microsoft.AspNetCore.Mvc</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore</Namespace>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

#load ".\DataContext.linq"
void Main(string[] args)
{
	Directory.SetCurrentDirectory(Path.GetDirectoryName(Util.CurrentQueryPath));
	var host = CreateHostBuilder(args).Build();
	SeedData(host);
	host.Run();
}

void SeedData(IHost host)
{
	using var scope = host.Services.CreateScope();
	var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
	dbContext.Responses.Add(new GuestResponse{
		Name = "Wagner",
		Email = "wagnerhsu@hotmail.com",
		Phone = "123456",
		WillAttend = true
	});
	dbContext.Responses.Add(new GuestResponse
	{
		Name = "Wagner01",
		Email = "wagnerhsu01@hotmail.com",
		Phone = "123456",
		WillAttend = false
	});
	dbContext.SaveChanges();
}

// Define other methods and classes here
public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddDbContext<DataContext>(c=>c.UseInMemoryDatabase("PartyInvites"));
		services.AddControllers();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}
[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
	DataContext _dbContext;
     public HomeController(DataContext dbContext)
	 {
	 	_dbContext = dbContext;
	 }
	 public async Task<IList<GuestResponse>> Get()
	 {
	 	return await _dbContext.Responses.ToListAsync();
	 }
}