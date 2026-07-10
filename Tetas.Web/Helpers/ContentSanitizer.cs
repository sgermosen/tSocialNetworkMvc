namespace Tetas.Web.Helpers
{
    using Ganss.Xss;

    public class ContentSanitizer : IContentSanitizer
    {
        private readonly HtmlSanitizer _sanitizer;

        public ContentSanitizer()
        {
            _sanitizer = new HtmlSanitizer();
            _sanitizer.AllowedTags.Clear();
            foreach (var tag in new[]
            {
                "b", "strong", "i", "em", "u", "s", "br", "p", "ul", "ol",
                "li", "blockquote", "code", "pre", "a", "h3", "h4"
            })
            {
                _sanitizer.AllowedTags.Add(tag);
            }

            _sanitizer.AllowedAttributes.Clear();
            _sanitizer.AllowedAttributes.Add("href");

            _sanitizer.AllowedSchemes.Clear();
            _sanitizer.AllowedSchemes.Add("http");
            _sanitizer.AllowedSchemes.Add("https");
            _sanitizer.AllowedSchemes.Add("mailto");

            _sanitizer.KeepChildNodes = true;
        }

        public string Sanitize(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            return _sanitizer.Sanitize(html);
        }
    }
}
