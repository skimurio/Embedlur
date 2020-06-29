using System.Collections.Generic;

namespace Embedlur
{
    public interface IProviderDiscovery
    {
        List<IProvider> GetAllProviders();
    }
}
