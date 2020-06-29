using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Embedlur.Providers
{
    public class SoundcloudProvider : BaseProvider
    {
        private readonly IRequestService _requestService;

        public SoundcloudProvider(IRequestService requestService)
            :base("https?://soundcloud.com/.*/.*")
        {
            _requestService = requestService;
        }

        public override string Name { get { return "Soundcloud"; } }

        protected override IEmbeddedResult ProcessUrl(string url)
        {
            var endpoint = string.Format("http://soundcloud.com/oembed?url={0}&format=json", WebUtility.UrlEncode(url));
            var result = JsonConvert.DeserializeObject<OEmbedJsonResult>(_requestService.Get(endpoint));
            return new RichEmbeddedResult(result.Html, result.Width, result.Height, result.Title, result.AuthorName, result.AuthorUrl, "Soundcloud", result.ProviderUrl, result.CacheAge, result.ThumbnailUrl, result.ThumbnailWidth, result.ThumbnailHeight);
        }
    }
}
