using System.Data.Entity;

namespace ODataHelper.Example_Project
{
    public class ExampleContext : DbContext
    {
        public DbSet<ExampleModel> ExampleModels { get; set; }
    }
}