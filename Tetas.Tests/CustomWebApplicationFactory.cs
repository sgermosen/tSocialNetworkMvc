using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Tetas.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Tetas.Web.Program>
{
    private readonly string _dbPath = Path.Combine(
        Path.GetTempPath(), $"tetas-test-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DatabaseProvider"] = "Sqlite",
                ["ConnectionStrings:SqliteCnn"] = $"Data Source={_dbPath}",
                ["Tokens:Key"] = "test-signing-key-test-signing-key-test-signing-key-123456",
                ["Tokens:Issuer"] = "localhost",
                ["Tokens:Audience"] = "users"
            });
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing && File.Exists(_dbPath))
        {
            try
            {
                File.Delete(_dbPath);
            }
            catch
            {
            }
        }
    }
}
