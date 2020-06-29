using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embedlur.Providers
{
    public class TedProvider : BaseOEmbedProvider
    {
        public TedProvider(IRequestService requestService) 
            : base(requestService, "http://www.ted.com/talks/oembed.json?url={url}", "https?://www\\.ted\\.com/talks/.+")
        {
        }

        public override string Name { get { return "Ted"; } }
    }
}
