using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embedlur
{
    /// <summary>
    /// A provider that can convert urls into an embedded object
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// The name of this provider.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// All the patterns used to match urls against this provider.
        /// </summary>
        List<string> Patterns { get; }

        /// <summary>
        /// Can this provider serve embedded content for the given url?
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool CanServeUrl(string url);

        /// <summary>
        /// Embed the url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IEmbeddedResult Embed(string url);

        /// <summary>
        /// Prepare the item to be locally embeded.
        /// This may contain a little bit more data on the object.
        /// It may also take a little longer to perform.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IEmbeddedResult LocalEmbed(string url);
    }
}
