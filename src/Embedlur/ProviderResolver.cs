using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embedlur
{
    public class ProviderResolver
    {
        private List<IProvider> _providers;

        public ProviderResolver(IProviderDiscovery providerDiscovery)
        {
            _providers = providerDiscovery.GetAllProviders();
        }

        public IProvider Resolve(string url)
        {
            return _providers.FirstOrDefault(provider => provider.CanServeUrl(url));
        }

        public IProvider ResolveByName(string name)
        {
            return _providers.FirstOrDefault(provider => provider.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
