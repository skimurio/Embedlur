using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Embedlur.Providers
{
    public class HuluProvider : BaseOEmbedProvider
    {
        public HuluProvider(IRequestService requestService)
            :base(requestService, "http://www.hulu.com/api/oembed.json?url={url}", "http:\\/\\/www.hulu.com\\/watch\\/*")
        {
        }

        public override string Name { get { return "Hulu"; } }
    }
}
