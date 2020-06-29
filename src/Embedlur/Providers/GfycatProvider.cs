using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Embedlur.Providers
{
    public class GfycatProvider : BaseProvider
    {
        private readonly IRequestService _requestService;
        private Regex _webmSourceRegex = new Regex("<source id=\\\"(webmsource)\\\" src=\\\"([a-zA-Z0-9:\\/\\.]*)\\\"");
        private Regex _mp4SourceRegex = new Regex("<source id=\\\"(mp4source)\\\" src=\\\"([a-zA-Z0-9:\\/\\.]*)\\\"");

        public GfycatProvider(IRequestService requestService)
            :base("http://gfycat\\.com/([a-zA-Z]+)")
        {
            _requestService = requestService;
        }

        public override string Name { get { return "Gfycat"; } }

        protected override IEmbeddedResult ProcessUrl(string url)
        {
            int patternIndex;
            var match = Match(url, out patternIndex);

            if (!match.Success) return null;

            var endpoint = string.Format("http://gfycat.com/cajax/oembed/{0}", match.Groups[1].Value);
            var response = JsonConvert.DeserializeObject<OEmbedJsonResult>(_requestService.Get(endpoint));

            var result = new VideoEmbeddedResult(response.Html, response.Width, response.Height, response.Title, response.AuthorName, response.AuthorUrl, "Gfycat", response.ProviderUrl, response.CacheAge, response.ThumbnailUrl, response.ThumbnailWidth, response.ThumbnailHeight);
            result.AdditionalData.Add("Key", match.Groups[1].Value);

            return result;
        }

        public override IEmbeddedResult LocalEmbed(string url)
        {
            var result = base.LocalEmbed(url);
            if (result == null) return null;

            // now, let's make a quick request to gfycat to get the webm/mp4/poster files.
            var response = _requestService.Get(url, "text/html");

            var match = _mp4SourceRegex.Match(response);
            if (!match.Success) return result;
            result.AdditionalData.Add("Mp4", match.Groups[2].Value);

            match = _webmSourceRegex.Match(response);
            if (!match.Success) return result;
            result.AdditionalData.Add("Webm", match.Groups[2].Value);

            result.AdditionalData.Add("Poster", string.Format("//thumbs.gfycat.com/{0}-poster.jpg", result.AdditionalData["Key"]));

            return result;
        }
    }
}
