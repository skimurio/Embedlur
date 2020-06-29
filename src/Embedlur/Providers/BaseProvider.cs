using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Embedlur.Providers
{
    public abstract class BaseProvider : IProvider
    {
        private readonly Dictionary<string, Regex> _patterns;

        protected BaseProvider(params string[] patterns)
        {
            _patterns = patterns.ToDictionary(x => x, x => new Regex(x));
        }

        public abstract string Name { get; }

        public List<string> Patterns {
            get {
                return _patterns.Keys.ToList();
            }
        }

        public bool CanServeUrl(string url)
        {
            return _patterns.Values.Any(pattern => pattern.IsMatch(url));
        }

        public IEmbeddedResult Embed(string url)
        {
            if (!CanServeUrl(url))
            {
                throw new Exception("The given url is invalid");
            }

            return ProcessUrl(url);
        }

        public virtual IEmbeddedResult LocalEmbed(string url)
        {
            return Embed(url);
        }

        protected abstract IEmbeddedResult ProcessUrl(string url);

        protected Match Match(string url, out int patternIndex)
        {
            int index = 0;

            // loop through patterns and try to find a match
            foreach (var key in _patterns.Keys)
            {
                var match = _patterns[key].Match(url);
                if (match.Success)
                {
                    patternIndex = index;
                    return match;
                }
                index++;
            }

            patternIndex = -1;
            return null;
        }

        protected Match Match(string url)
        {
            int index;
            return Match(url, out index);
        }
        
    }
}
