using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embedlur
{
    public class EmbeddedResult : IEmbeddedResult
    {
        public EmbeddedResult(string type,
            string title = null,
            string authorName = null,
            string authorUrl = null,
            string providerName = null,
            string providerUrl = null,
            string cacheAge = null,
            string thumbnailUrl = null,
            string thumbnailWidth = null,
            string thumbnailHeight = null)
        {
            Version = "1.0";

            if (string.IsNullOrEmpty(type)) throw new ArgumentNullException(nameof(type));
            switch (type)
            {
                case "photo":
                case "video":
                case "link":
                case "rich":
                    break;
                default:
                    throw new Exception("Unknown type " + type);
            }

            Type = type;
            Title = title;
            AuthorName = authorName;
            AuthorUrl = authorUrl;
            ProviderName = providerName;
            ProviderUrl = providerUrl;
            CacheAge = cacheAge;
            ThumbnailUrl = thumbnailUrl;
            ThumbnailWidth = thumbnailWidth;
            ThumbnailHeight = thumbnailHeight;
            AdditionalData = new Dictionary<string, object>();
        }

        public string Type { get; }

        public string Version { get; }

        public string Title { get; }

        public string AuthorName { get; }

        public string AuthorUrl { get; }

        public string ProviderName { get; }

        public string ProviderUrl { get; }

        public string CacheAge { get; }

        public string ThumbnailUrl { get; }

        public string ThumbnailWidth { get; }

        public string ThumbnailHeight { get; }

        public Dictionary<string, object> AdditionalData { get; }
    }

    public class PhotoEmbeddedResult : EmbeddedResult, IPhotoEmbeddedResult
    {
        public PhotoEmbeddedResult(string url,
            string width,
            string height,
            string title = null,
            string authorName = null,
            string authorUrl = null,
            string providerName = null,
            string providerUrl = null,
            string cacheAge = null,
            string thumbnailUrl = null,
            string thumbnailWidth = null,
            string thumbnailHeight = null)
            : base("photo",
                  title,
                  authorName,
                  authorUrl,
                  providerName,
                  providerUrl,
                  cacheAge,
                  thumbnailUrl,
                  thumbnailWidth,
                  thumbnailHeight)
        {
            Url = url;
            Width = width;
            Height = height;
        }

        public string Url { get; }
        public string Width { get; }
        public string Height { get; }
    }

    public class VideoEmbeddedResult : EmbeddedResult, IVideoEmbeddedResult
    {
        public VideoEmbeddedResult(string html,
            string width,
            string height,
            string title = null,
            string authorName = null,
            string authorUrl = null,
            string providerName = null,
            string providerUrl = null,
            string cacheAge = null,
            string thumbnailUrl = null,
            string thumbnailWidth = null,
            string thumbnailHeight = null)
            : base("video",
                  title,
                  authorName,
                  authorUrl,
                  providerName,
                  providerUrl,
                  cacheAge,
                  thumbnailUrl,
                  thumbnailWidth,
                  thumbnailHeight)
        {
            Html = html;
            Width = width;
            Height = height;
        }

        public string Html { get; }

        public string Width { get; }

        public string Height { get; }
    }

    public class RichEmbeddedResult : EmbeddedResult, IRichEmbeddedResult
    {
        public RichEmbeddedResult(string html,
            string width,
            string height,
            string title = null,
            string authorName = null,
            string authorUrl = null,
            string providerName = null,
            string providerUrl = null,
            string cacheAge = null,
            string thumbnailUrl = null,
            string thumbnailWidth = null,
            string thumbnailHeight = null)
            : base("rich",
                  title,
                  authorName,
                  authorUrl,
                  providerName,
                  providerUrl,
                  cacheAge,
                  thumbnailUrl,
                  thumbnailWidth,
                  thumbnailHeight)
        {
            Html = html;
            Width = width;
            Height = height;
        }

        public string Html { get; }

        public string Width { get; }

        public string Height { get; }
    }

    public class LinkEmbeddedResult : EmbeddedResult, ILinkEmbeddedResult
    {
        public LinkEmbeddedResult(string title = null,
            string authorName = null,
            string authorUrl = null,
            string providerName = null,
            string providerUrl = null,
            string cacheAge = null,
            string thumbnailUrl = null,
            string thumbnailWidth = null,
            string thumbnailHeight = null)
            : base("link",
                  title,
                  authorName,
                  authorUrl,
                  providerName,
                  providerUrl,
                  cacheAge,
                  thumbnailUrl,
                  thumbnailWidth,
                  thumbnailHeight)
        {

        }
    }
}
