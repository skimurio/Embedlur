using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Embedlur.Helpers;
using Embedlur.Providers;

namespace Embedlur
{
    public class ProviderDiscovery : IProviderDiscovery
    {
        private readonly IRequestService _requestService;
        private readonly IHtmlParser _htmlParser;

        public ProviderDiscovery(IRequestService requestService, IHtmlParser htmlParser)
        {
            _requestService = requestService;
            _htmlParser = htmlParser;
        }

        public List<IProvider> GetAllProviders()
        {
            var result = new List<IProvider>();
            result.Add(new TwitterProvider(_requestService));
            result.Add(new YouTubeProvider(_requestService));
            result.Add(new FlickrProvider(_requestService));
            result.Add(new VimeoProvider(_requestService));
            result.Add(new SoundcloudProvider(_requestService));
            result.Add(new GfycatProvider(_requestService));
            result.Add(new ImgurProvider(_htmlParser, _requestService));
            result.Add(new HuluProvider(_requestService));
            result.Add(new TedProvider(_requestService));
            return result;
        }
    }
}
