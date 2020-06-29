using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Embedlur.Providers
{
    public class FlickrProvider : BaseProvider
    {
        private readonly IRequestService _requestService;

        public FlickrProvider(IRequestService requestService)
            :base("https?://(?:www\\.)?flickr\\.com/.*", "https?://flic\\.kr/p/[a-zA-Z0-9]+")
        {
            _requestService = requestService;
        }

        public override string Name { get { return "Flickr"; } }

        protected override IEmbeddedResult ProcessUrl(string url)
        {
            if (!CanServeUrl(url))
                throw new Exception("Invalid url");

            var result = JsonConvert.DeserializeObject<OEmbedJsonResult>(_requestService.Get("http://www.flickr.com/services/oembed/?url=" + WebUtility.UrlEncode(url) + "&format=json"));

            return new PhotoEmbeddedResult(result.Url, 
                result.Width, 
                result.Height, 
                result.Title,
                result.AuthorName,
                result.AuthorUrl,
                "Flickr",
                result.ProviderUrl, 
                result.CacheAge,
                result.ThumbnailUrl,
                result.ThumbnailWidth,
                result.ThumbnailHeight);
        }
    }
}
