<Query Kind="Program">
  <NuGetReference>Microsoft.EntityFrameworkCore.SqlServer</NuGetReference>
  <Namespace>Microsoft.EntityFrameworkCore</Namespace>
</Query>

void Main()
{
	
}

// Define other methods, classes and namespaces here
public class DataContext : DbContext
{

	public DataContext(DbContextOptions<DataContext> options)
		: base(options) { }

	public DbSet<GuestResponse> Responses { get; set; }
}
public class GuestResponse
{

	public long Id { get; set; }

	public string Name { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	public bool? WillAttend { get; set; }
}