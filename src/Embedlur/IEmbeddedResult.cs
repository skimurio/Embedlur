using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embedlur
{
    /// <summary>
    /// The command embedded result parameters
    /// </summary>
    public interface IEmbeddedResult
    {
        /// <summary>
        /// (required)
        /// The resource type.Valid values, along with value-specific parameters, are described below.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// (required) 
        /// The oEmbed version number.This must be 1.0.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// (optional)
        /// A text title, describing the resource.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// (optional)
        /// The name of the author/owner of the resource.
        /// </summary>
        string AuthorName { get; }

        /// <summary>
        /// (optional)
        /// A URL for the author/owner of the resource
        /// </summary>
        string AuthorUrl { get; }

        /// <summary>
        /// (optional)
        /// The name of the resource provider.
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// (optional)
        /// The url of the resource provider.
        /// </summary>
        string ProviderUrl { get; }

        /// <summary>
        /// (optional)
        /// The suggested cache lifetime for this resource, in seconds.Consumers may choose to use this value or not.
        /// </summary>
        string CacheAge { get; }

        /// <summary>
        /// (optional)
        /// A URL to a thumbnail image representing the resource.The thumbnail must respect any maxwidth and maxheight parameters.If this parameter is present, thumbnail_width and thumbnail_height must also be present.
        /// </summary>
        string ThumbnailUrl { get; }

        /// <summary>
        /// (optional)
        /// The width of the optional thumbnail.If this parameter is present, thumbnail_url and thumbnail_height must also be present.
        /// </summary>
        string ThumbnailWidth { get; }

        /// <summary>
        /// (optional)
        /// The height of the optional thumbnail.If this parameter is present, thumbnail_url and thumbnail_width must also be present.
        /// </summary>
        string ThumbnailHeight { get; }

        /// <summary>
        /// Addtional data for this result.
        /// Used internally.
        /// </summary>
        Dictionary<string, object> AdditionalData { get; }
    }

    /// <summary>
    /// This type is used for representing static photos. The following parameters are defined:
    /// </summary>
    public interface IPhotoEmbeddedResult : IEmbeddedResult
    {
        /// <summary>
        /// (required)
        /// The source URL of the image.Consumers should be able to insert this URL into an<img> element. Only HTTP and HTTPS URLs are valid.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// (required)
        /// The width in pixels of the image specified in the url parameter.
        /// </summary>
        string Width { get; }

        /// <summary>
        /// (required)
        /// The height in pixels of the image specified in the url parameter.
        /// </summary>
        string Height { get; }
    }

    /// <summary>
    /// This type is used for representing playable videos. The following parameters are defined:
    /// </summary>
    public interface IVideoEmbeddedResult : IEmbeddedResult
    {
        /// <summary>
        /// (required)
        /// The HTML required to embed a video player.The HTML should have no padding or margins.Consumers may wish to load the HTML in an off-domain iframe to avoid XSS vulnerabilities.
        /// </summary>
        string Html { get; }

        /// <summary>
        /// The width in pixels required to display the HTML.
        /// </summary>
        string Width { get; }

        /// <summary>
        /// The height in pixels required to display the HTML.
        /// </summary>
        string Height { get; }
    }

    /// <summary>
    /// Responses of this type allow a provider to return any generic embed data (such as title and author_name), without providing either the url or html parameters. The consumer may then link to the resource, using the URL specified in the original request.
    /// </summary>
    public interface ILinkEmbeddedResult : IEmbeddedResult
    {

    }

    public interface IRichEmbeddedResult : IEmbeddedResult
    {
        /// <summary>
        /// (required)
        /// The HTML required to display the resource. The HTML should have no padding or margins. Consumers may wish to load the HTML in an off-domain iframe to avoid XSS vulnerabilities. The markup should be valid XHTML 1.0 Basic.
        /// </summary>
        string Html { get; }

        /// <summary>
        /// The width in pixels required to display the HTML.
        /// </summary>
        string Width { get; }

        /// <summary>
        /// The height in pixels required to display the HTML.
        /// </summary>
        string Height { get; }
    }
}
