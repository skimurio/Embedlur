using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Embedlur.Helpers
{
    public class HtmlParser : IHtmlParser
    {
        // <meta[\s]+((\S+)=["']((?:.(?!["']?\s+(?:\S+)=|[>"']))+.)["']+[\s]*)*/>
        private readonly Regex _metaRegex = new Regex("<meta[\\s]+((\\S+)=[\"']((?:.(?![\"']?\\s+(?:\\S+)=|[>\"']))+.)[\"']+[\\s]*)*/>");

        public List<HtmlMetaTag> ParseMetaTags(string html)
        {
            var result = new List<HtmlMetaTag>();

            if (string.IsNullOrEmpty(html))
            {
                // return empty array
                return result;
            }

            var matches = _metaRegex.Matches(html);

            // loop through matches
            foreach(Match match in matches)
            {
                if (match.Success)
                {
                    if (match.Groups.Count != 4)
                    {
                        throw new Exception("Invalid length.");
                    }

                    var nameCaptures = match.Groups[2].Captures;
                    var valueCaptures = match.Groups[3].Captures;

                    if (nameCaptures.Count != valueCaptures.Count)
                    {
                        // there is not a matching value for each name
                        throw new Exception("There should be a matching value caught for each name.");
                    }

                    if (nameCaptures.Count == 0)
                    {
                        continue;
                    }

                    var metaTag = new HtmlMetaTag();

                    for (var index = 0; index < nameCaptures.Count; index++)
                    {
                        switch(nameCaptures[index].Value)
                        {
                            case "property":
                                metaTag.Property = valueCaptures[index].Value;
                                break;
                            case "name":
                                metaTag.Name = valueCaptures[index].Value;
                                break;
                            case "content":
                                metaTag.Content = valueCaptures[index].Value;
                                break;
                        }
                    }

                    result.Add(metaTag);
                }
            }

            return result;
        }
    }
}
