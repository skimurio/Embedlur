using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Embedlur.Helpers;

namespace Embedlur.Providers
{
    public class ImgurProvider : BaseProvider
    {
        private readonly IHtmlParser _htmlParser;
        private readonly IRequestService _requestService;

        public ImgurProvider(IHtmlParser htmlParser, IRequestService requestService)
            : base("https?://imgur\\.com/(gallery/)?([0-9a-zA-Z]+)", "https?://i\\.imgur\\.com/([0-9a-zA-Z]+)")
        {
            _htmlParser = htmlParser;
            _requestService = requestService;
        }

        public override string Name { get { return "Imgur"; } }

        protected override IEmbeddedResult ProcessUrl(string url)
        {
            int matchIndex;
            var match = Match(url, out matchIndex);

            if (matchIndex == 1)
            {
                // this is a directly link to an image.
                // let's get the real url so that we can render the image as a propert imgur image.
                url = "http://imgur.com/gallery/" + match.Groups[1].Value;
                match = Match(url);
            }


            var imgurId = match.Groups[2].Value;
            
            var responseHtml = _requestService.Get(url, "text/html");

            var metaTags = _htmlParser.ParseMetaTags(responseHtml);

            var twitterCardMeta = metaTags.FirstOrDefault(x => x.Name == "twitter:card");

            if (twitterCardMeta == null) return null;

            if (twitterCardMeta.Content == "photo")
            {
                var twitterImgSrcMeta = metaTags.FirstOrDefault(x => x.Name == "twitter:image:src");
                if (twitterImgSrcMeta == null) return null;
                if (string.IsNullOrEmpty(twitterImgSrcMeta.Content)) return null;

                var twitterImgSrcWidthMeta = metaTags.FirstOrDefault(x => x.Name == "twitter:image:width");
                if (twitterImgSrcWidthMeta == null) return null;
                if (string.IsNullOrEmpty(twitterImgSrcWidthMeta.Content)) return null;

                var twitterImgSrcHeightMeta = metaTags.FirstOrDefault(x => x.Name == "twitter:image:height");
                if (twitterImgSrcHeightMeta == null) return null;
                if (string.IsNullOrEmpty(twitterImgSrcHeightMeta.Content)) return null;

                var openGraphTitleMeta = metaTags.FirstOrDefault(x => x.Property == "og:title");

                var result = new PhotoEmbeddedResult(twitterImgSrcMeta.Content,
                    twitterImgSrcWidthMeta.Content,
                    twitterImgSrcHeightMeta.Content,
                    openGraphTitleMeta != null ? openGraphTitleMeta.Content : null,
                    "http://imgur.com/",
                    null,
                    "Imgur",
                    "http://imgur.com/");

                // attempt to add the image as a gallery item so that the image appears and fancy-like.
                int width;
                int height;
                if (!int.TryParse(twitterImgSrcWidthMeta.Content, out width))
                    return result;
                if (!int.TryParse(twitterImgSrcHeightMeta.Content, out height))
                    return result;
                result.AdditionalData["Items"] = new List<ImgurGalleryItem>
                {
                    new ImgurGalleryItem
                    {
                        Url = result.Url,
                        Width = width,
                        Height = height,
                        Type = ImgurGalleryItemType.Photo
                    }
                };

                return result;
            }

            if (twitterCardMeta.Content == "gallery")
            {
                var openGraphTitleMeta = metaTags.FirstOrDefault(x => x.Property == "og:title");

                var html = string.Format("<blockquote class=\"imgur-embed-pub\" lang=\"en\" data-id=\"a/{0}\"><a href=\"//imgur.com/a/{0}\">{1}</a></blockquote><script async src=\"//s.imgur.com/min/embed.js\" charset=\"utf-8\"></script>", imgurId, openGraphTitleMeta != null ? openGraphTitleMeta.Content : null);
                var result = new RichEmbeddedResult(html,
                    "540",
                    "500",
                    "",
                    "Imgur",
                    null,
                    "Imgur",
                    "http://imgur.com/");

                var galleryItems = new List<ImgurGalleryItem>();

                int? currentWidth = null;
                int? currentHeight = null;

                foreach (var openGraphImage in metaTags.Where(x => x.Property == "og:image" || x.Property == "og:image:width" || x.Property == "og:image:height")
                    // the layout in the markup is image > width > height.
                    // this reorders so dimensions are first.
                    .Reverse())
                {
                    if (openGraphImage.Property == "og:image")
                    {
                        var openGraphImageUrl = openGraphImage.Content;
                        if (string.IsNullOrEmpty(openGraphImageUrl))
                        {
                            currentWidth = null;
                            currentHeight = null;
                            continue;
                        }

                        if (!currentWidth.HasValue || !currentHeight.HasValue)
                        {
                            currentWidth = null;
                            currentHeight = null;
                            continue;
                        }

                        var galleryItem = new ImgurGalleryItem();
                        galleryItem.Url = openGraphImageUrl;
                        galleryItem.Width = currentWidth.Value;
                        galleryItem.Height = currentHeight.Value;

                        if (galleryItem.Url.EndsWith("?fb"))
                            galleryItem.Url = galleryItem.Url.Substring(0, galleryItem.Url.Length - 3);

                        if (galleryItem.Url.EndsWith(".gif"))
                        {
                            galleryItem.Type = ImgurGalleryItemType.Gif;
                            galleryItem.Mp4 = galleryItem.Url.Substring(0, galleryItem.Url.Length - 3) + "mp4";
                            galleryItem.Webm = galleryItem.Url.Substring(0, galleryItem.Url.Length - 3) + "webm";
                        }
                        else
                        {
                            galleryItem.Type = ImgurGalleryItemType.Photo;
                        }

                        galleryItems.Add(galleryItem);

                        currentWidth = null;
                        currentHeight = null;
                    }
                    else if (openGraphImage.Property == "og:image:width")
                    {
                        int temp;
                        if (int.TryParse(openGraphImage.Content, out temp))
                            currentWidth = temp;
                    }
                    else if (openGraphImage.Property == "og:image:height")
                    {
                        int temp;
                        if (int.TryParse(openGraphImage.Content, out temp))
                            currentHeight = temp;
                    }
                }

                if (galleryItems.Count > 0)
                {
                    result.AdditionalData["Items"] = galleryItems;
                }

                return result;
            }

            if (twitterCardMeta.Content == "player")
            {
                var twitterPlayerWidthMeta = metaTags.FirstOrDefault(x => x.Name == "twitter:player:width");
                if (twitterPlayerWidthMeta == null) return null;
                if (string.IsNullOrEmpty(twitterPlayerWidthMeta.Content)) return null;

                var twitterPlayerHeightMeta = metaTags.FirstOrDefault(x => x.Name == "twitter:player:height");
                if (twitterPlayerHeightMeta == null) return null;
                if (string.IsNullOrEmpty(twitterPlayerHeightMeta.Content)) return null;
                
                var openGraphTitleMeta = metaTags.FirstOrDefault(x => x.Property == "og:title");

                var result = new PhotoEmbeddedResult(string.Format("https://i.imgur.com/{0}.gif", imgurId),
                    twitterPlayerWidthMeta.Content,
                    twitterPlayerHeightMeta.Content,
                    openGraphTitleMeta != null ? openGraphTitleMeta.Content : null,
                    "http://imgur.com/",
                    null,
                    "Imgur",
                    "http://imgur.com/");

                // attempt to add the image as a gallery item so that the image appears and fancy-like.
                int width;
                int height;
                if (!int.TryParse(twitterPlayerWidthMeta.Content, out width))
                    return result;
                if (!int.TryParse(twitterPlayerHeightMeta.Content, out height))
                    return result;
                result.AdditionalData["Items"] = new List<ImgurGalleryItem>
                {
                    new ImgurGalleryItem
                    {
                        Url = result.Url,
                        Mp4 = result.Url.Substring(0, result.Url.Length - 3) + "mp4",
                        Webm = result.Url.Substring(0, result.Url.Length - 3) + "webm",
                        Width = width,
                        Height = height,
                        Type = ImgurGalleryItemType.Gif
                    }
                };

                return result;
            }

            return null;
        }

        public class ImgurGalleryItem
        {
            public ImgurGalleryItemType Type { get; set; }

            public string Url { get; set; }

            public string Mp4 { get; set; }

            public string Webm { get; set; }

            public int Width { get; set; }

            public int Height { get; set; }
        }

        public enum ImgurGalleryItemType
        {
            Photo,
            Gif
        }
    }
}
