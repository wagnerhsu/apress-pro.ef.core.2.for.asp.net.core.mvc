<Query Kind="Program">
  <NuGetReference>Microsoft.EntityFrameworkCore.SqlServer</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Configuration.Json</NuGetReference>
  <NuGetReference>Microsoft.Extensions.DependencyInjection</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Hosting</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Logging.Console</NuGetReference>
  <Namespace>Microsoft.Extensions.DependencyInjection</Namespace>
  <Namespace>Microsoft.Extensions.Hosting</Namespace>
  <Namespace>Microsoft.Extensions.Logging</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>Microsoft.EntityFrameworkCore</Namespace>
</Query>

void Main(string[] args)
{
	using (IHost host = Host.CreateDefaultBuilder(args)
				.ConfigureServices(ConfigureServices)
				.Build())
	{
		var scope = host.Services.CreateScope();
		using (var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>())
		{
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			SeedData(dbContext);
		}
		UseContainer(host.Services);
	}
}

void SeedData(DataContext dbContext)
{
	var c1 = new Category
	{
		Name = "C#",
		Description = "CSharp"
	};
	dbContext.Categories.Add(c1);
	var c2 = new Category
	{
		Name = "C++",
		Description = "CPlusPlus"
	};
	dbContext.Categories.Add(c2);
	dbContext.Products.Add(new Product{
		 Category = c1,
		  Name = "P-C#"
		   
	});
	dbContext.SaveChanges();
}

void UseContainer(IServiceProvider services)
{
	var mainConsole = services.GetRequiredService<MainConsole>();
	mainConsole.DoWork();
}

void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
	services.AddScoped<MainConsole>();
	services.AddDbContext<DataContext>(x=>x.UseSqlServer("Server=.;Database=07_01;Trusted_Connection=True;MultipleActiveResultSets=true"));
}

public class MainConsole
{
	ILogger<MainConsole> _logger;
	DataContext _dbContext;
	public MainConsole(ILogger<MainConsole> logger, DataContext dbContext)
	{
		_logger = logger;
		_dbContext = dbContext;
	}
	public void DoWork()
	{
		_logger.LogInformation("DoWork");
		_dbContext.Products.Include(x=>x.Category).ToList().Dump("Products");
		_dbContext.Categories.ToList().Dump("Categories");
	}
}

public class Product
{

	public long Id { get; set; }

	public string Name { get; set; }
	public decimal PurchasePrice { get; set; }
	public decimal RetailPrice { get; set; }

	public long CategoryId { get; set; }
	public Category Category { get; set; }
}
public class Category
{
	public long Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
}
public class DataContext : DbContext
{

	public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }

	public DbSet<Product> Products { get; set; }

	public DbSet<Category> Categories { get; set; }
}