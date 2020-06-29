using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Embedlur.Providers
{
    public class TwitterProvider : BaseProvider
    {
        private readonly IRequestService _requestService;
        // <link rel=\"alternate\" type=\"text/xml\+oembed\" href=\"([a-zA-Z0-9:\/\.\?\=])*\"
        //private readonly Regex _ombededUrlPattern = new Regex("<link rel=\\\"alternate\\\" type=\\\"text/xml\\+oembed\\\"\" href=\\\"([a-zA-Z0-9:\\/\\.\\?\\=])*\\\"");
        private readonly Regex _ombededUrlPattern = new Regex("<link rel=\\\"alternate\\\" type=\\\"text/xml\\+oembed\\\" href=\\\"(([a-zA-Z0-9:\\/\\.\\?\\=])*)\\\"");

        public TwitterProvider(IRequestService requestService)
            : base("https?://(?:www|mobile\\.)?twitter\\.com/(?:#!/)?[^/]+/status(?:es)?/(\\d+)/?$", "https?://t\\.co/[a-zA-Z0-9]+")
        {
            _requestService = requestService;
        }

        public override string Name { get { return "Twitter"; } }

        protected override IEmbeddedResult ProcessUrl(string url)
        {
            var html = _requestService.Get(url);

            var match = _ombededUrlPattern.Match(html);

            if (!match.Success)
                return null;

            var oembeddedUrl = match.Groups[1].Value.Replace(".xml", ".json");

            var result = JsonConvert.DeserializeObject<OEmbedJsonResult>(_requestService.Get(oembeddedUrl));

            return new RichEmbeddedResult(result.Html, 
                result.Width, 
                result.Height,
                result.Title, 
                result.AuthorName, 
                result.AuthorUrl, 
                "Twitter",
                result.ProviderUrl, 
                result.CacheAge, 
                result.ThumbnailUrl,
                result.ThumbnailWidth, 
                result.ThumbnailHeight);
        }
    }
}
