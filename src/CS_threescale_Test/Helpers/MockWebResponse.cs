using System;
using System.Net;
using System.IO;

namespace CS_threescale_Test
{
    public class MockWebResponse : WebResponse
    {
        Stream responseStream;

        public MockWebResponse(Stream responseStream)
        {
            this.responseStream = responseStream;
        }

        public override Stream GetResponseStream()
        {
            return responseStream;
        }
    }
}

