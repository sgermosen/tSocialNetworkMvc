using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Tetas.Tests;

public class ApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ApiIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    private async Task<string> RegisterAsync(HttpClient client, string email)
    {
        var response = await client.PostAsJsonAsync("/api/auth/register", new
        {
            firstName = "Test",
            lastName = "User",
            email,
            password = "Password1"
        });
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        return body.GetProperty("token").GetString()!;
    }

    [Fact]
    public async Task Register_returns_a_token()
    {
        var client = _factory.CreateClient();
        var token = await RegisterAsync(client, $"reg-{Guid.NewGuid():N}@example.com");
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public async Task Posts_endpoint_requires_authentication()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/posts");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Can_create_and_read_a_post()
    {
        var client = _factory.CreateClient();
        var token = await RegisterAsync(client, $"post-{Guid.NewGuid():N}@example.com");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var create = await client.PostAsJsonAsync("/api/posts", new
        {
            name = "Hello",
            body = "world"
        });
        create.EnsureSuccessStatusCode();

        var feed = await client.GetFromJsonAsync<JsonElement>("/api/posts", JsonOptions);
        Assert.True(feed.GetArrayLength() >= 1);
    }

    [Fact]
    public async Task Reacting_toggles_the_count()
    {
        var client = _factory.CreateClient();
        var token = await RegisterAsync(client, $"react-{Guid.NewGuid():N}@example.com");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var create = await client.PostAsJsonAsync("/api/posts", new { name = "R", body = "react" });
        var created = await create.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        var id = created.GetProperty("id").GetInt64();

        var on = await client.PostAsJsonAsync($"/api/posts/{id}/reactions", new { type = "Like" });
        var onSummary = await on.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.Equal(1, onSummary.GetProperty("total").GetInt32());

        var off = await client.PostAsJsonAsync($"/api/posts/{id}/reactions", new { type = "Like" });
        var offSummary = await off.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        Assert.Equal(0, offSummary.GetProperty("total").GetInt32());
    }

    [Fact]
    public async Task Body_is_sanitized_on_create()
    {
        var client = _factory.CreateClient();
        var token = await RegisterAsync(client, $"xss-{Guid.NewGuid():N}@example.com");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var create = await client.PostAsJsonAsync("/api/posts", new
        {
            name = "X",
            body = "<b>ok</b><script>alert(1)</script>"
        });
        var created = await create.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
        var body = created.GetProperty("body").GetString()!;

        Assert.DoesNotContain("<script", body, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("<b>ok</b>", body);
    }
}
