using Microsoft.EntityFrameworkCore.Design;

namespace Judge.Data;

internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        return new DataContext("Data Source=.;Initial Catalog=Judge;Integrated Security=True");
    }
}