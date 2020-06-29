using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Embedlur.Providers
{
    public class VimeoProvider : BaseProvider
    {
        private readonly IRequestService _requestService;

        public VimeoProvider(IRequestService requestService)
            :base("https?://(?:www\\.)?vimeo\\.com/.+")
        {
            _requestService = requestService;
        }

        public override string Name { get { return "Vimeo"; } }

        protected override IEmbeddedResult ProcessUrl(string url)
        {
            var result = JsonConvert.DeserializeObject<OEmbedJsonResult>(_requestService.Get("https://vimeo.com/api/oembed.json?url=" + WebUtility.UrlEncode(url)));
            return new VideoEmbeddedResult(result.Html, result.Width, result.Height, result.Title, result.AuthorName, result.AuthorUrl, "Vimeo", result.ProviderUrl, result.CacheAge, result.ThumbnailUrl, result.ThumbnailWidth, result.ThumbnailHeight);
        }
    }
}
