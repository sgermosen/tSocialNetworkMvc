using Tetas.Web.Helpers;

namespace Tetas.Tests;

public class ContentSanitizerTests
{
    private readonly ContentSanitizer _sanitizer = new();

    [Fact]
    public void Removes_script_tags()
    {
        var result = _sanitizer.Sanitize("<b>hi</b><script>alert(1)</script>");

        Assert.DoesNotContain("<script", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("<b>hi</b>", result);
    }

    [Fact]
    public void Removes_event_handlers_and_unsafe_tags()
    {
        var result = _sanitizer.Sanitize("<img src=x onerror=alert(1)>text");

        Assert.DoesNotContain("onerror", result, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("<img", result, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("text", result);
    }

    [Fact]
    public void Blocks_javascript_scheme_in_links()
    {
        var result = _sanitizer.Sanitize("<a href=\"javascript:alert(1)\">x</a>");

        Assert.DoesNotContain("javascript:", result, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Keeps_safe_https_links()
    {
        var result = _sanitizer.Sanitize("<a href=\"https://example.com\">x</a>");

        Assert.Contains("https://example.com", result);
    }
}
