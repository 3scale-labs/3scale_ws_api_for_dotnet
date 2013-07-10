using System;
using System.Net;
using System.IO;

namespace CS_threescale_Test
{
    public class MockWebRequest : WebRequest
    {
        MemoryStream requestStream = new MemoryStream();

        MemoryStream responseStream;

        public MockWebRequest()
        {
            responseStream = new MemoryStream();
        }

        public string ContentAsString()
        {
            return System.Text.Encoding.UTF8.GetString(requestStream.ToArray());
        }

        public override Stream GetRequestStream()
        {
            return requestStream;
        }

        public override WebResponse GetResponse()
        {
            return new MockWebResponse(responseStream);
        }
    }
}

