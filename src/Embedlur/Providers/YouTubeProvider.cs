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
    public class YouTubeProvider : BaseProvider
    {
        private readonly IRequestService _requestService;

        public YouTubeProvider(IRequestService requestService)
            :base("https?://(?:[^\\.]+\\.)?youtube\\.com/watch/?\\?(?:.+&)?v=([a-zA-Z0-9_-]+)", "https?://youtu\\.be/([a-zA-Z0-9_-]+)")
        {
            _requestService = requestService;
        }

        public override string Name { get { return "YouTube"; } }

        protected override IEmbeddedResult ProcessUrl(string url)
        {
            if(!CanServeUrl(url))
                throw new Exception("Invalid url");

            int index;
            var match = Match(url, out index);
            if (match == null)
                return null;

            var id = match.Groups[1].Value;

            var result = JsonConvert.DeserializeObject<OEmbedJsonResult>(_requestService.Get("http://www.youtube.com/oembed?url=" + WebUtility.UrlEncode("https://www.youtube.com/watch?v=" + id)));

            return new VideoEmbeddedResult(result.Html, result.Width, result.Height, result.Title, result.AuthorName, result.AuthorUrl, "YouTube", result.ProviderUrl, result.CacheAge, result.ThumbnailUrl, result.ThumbnailWidth, result.ThumbnailHeight);
        }
    }
}
