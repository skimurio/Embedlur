using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Embedlur
{
    public class RequestService : IRequestService
    {
        public string Get(string url, string contentType = "application/json")
        {
            // create the request
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = contentType;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            // get the response
            using (var response = (HttpWebResponse) request.GetResponse())
            {
                // make sure that we have a 200 response code
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        break;
                    default:
                        throw new Exception("response code is not 200 (" + response.StatusCode + ")");
                }

                // read the stream from the response
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        // couldn't get a resonse stream
                        throw new Exception("Couldn't get the response stream");
                    }

                    using (var streamReader = new StreamReader(responseStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}
