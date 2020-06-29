using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Embedlur
{
    public class OEmbedJsonResult
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "author_name")]
        public string AuthorName { get; set; }

        [JsonProperty(PropertyName = "author_url")]
        public string AuthorUrl { get; set; }

        [JsonProperty(PropertyName = "provider_name")]
        public string ProviderName { get; set; }

        [JsonProperty(PropertyName = "provider_url")]
        public string ProviderUrl { get; set; }

        [JsonProperty(PropertyName = "cache_age")]
        public string CacheAge { get; set; }

        [JsonProperty(PropertyName = "thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty(PropertyName = "thumbnail_width")]
        public string ThumbnailWidth { get; set; }

        [JsonProperty(PropertyName = "thumbnail_height")]
        public string ThumbnailHeight { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "html")]
        public string Html { get; set; }

        [JsonProperty(PropertyName = "width")]
        public string Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public string Height { get; set; }
    }
}
