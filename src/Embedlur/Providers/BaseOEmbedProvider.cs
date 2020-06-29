using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Embedlur.Providers
{
    public abstract class BaseOEmbedProvider : BaseProvider
    {
        private readonly IRequestService _requestService;
        private readonly string _providerUrl;

        public BaseOEmbedProvider(IRequestService requestService, string providerUrl, params string[] patterns)
            : base(patterns)
        {
            _requestService = requestService;
            _providerUrl = providerUrl;
        }

        protected override IEmbeddedResult ProcessUrl(string url)
        {
            var result = JsonConvert.DeserializeObject<OEmbedJsonResult>(_requestService.Get(_providerUrl.Replace("{url}", WebUtility.UrlEncode(url))));

            switch (result.Type)
            {
                case "photo":
                    return new PhotoEmbeddedResult(result.Url, result.Width, result.Height, result.Title,
                        result.AuthorName, result.AuthorUrl, Name, result.ProviderUrl, result.CacheAge,
                        result.ThumbnailUrl, result.ThumbnailWidth, result.ThumbnailHeight);
                case "video":
                    return new VideoEmbeddedResult(result.Html, result.Width, result.Height, result.Title,
                        result.AuthorName, result.AuthorUrl, Name, result.ProviderUrl, result.CacheAge,
                        result.ThumbnailUrl, result.ThumbnailWidth, result.ThumbnailHeight);
                case "link":
                    return new LinkEmbeddedResult(result.Title, result.AuthorName, result.AuthorUrl,
                        Name, result.ProviderUrl, result.CacheAge, result.ThumbnailUrl, result.ThumbnailWidth,
                        result.ThumbnailHeight);
                case "rich":
                    return new RichEmbeddedResult(result.Html, result.Width, result.Height, result.Title,
                        result.AuthorName, result.AuthorUrl, Name, result.ProviderUrl, result.CacheAge,
                        result.ThumbnailUrl, result.ThumbnailWidth, result.ThumbnailHeight);
                default:
                    throw new Exception("Unknown oembed result type");
            }
        }
    }
}
