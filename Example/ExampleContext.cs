using System.Data.Entity;

namespace Example
{
    public class ExampleContext : DbContext
    {
        public DbSet<ExampleModel> ExampleModels { get; set; }
    }
}