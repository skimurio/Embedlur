using System;
using System.Collections.Generic;

namespace Embedlur.Helpers
{
    public interface IHtmlParser
    {
        List<HtmlMetaTag> ParseMetaTags(string html);
    }

    public class HtmlMetaTag
    {
        public string Property { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }
    }
}
