using System;
using NUnit.Framework;
using CS_threescale;
using System.Net;
using Moq;
using System.Collections;

namespace CS_threescale_Test
{
    [TestFixture]
    public class ApiTest
    {
        private IApi m_api;
        private readonly string host = "http://su1.3scale.net";
        private readonly string provider_key = "12345678";

        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void TestDefaultHost()
        {
            m_api = new Api();
            Assert.AreEqual("http://su1.3scale.net", m_api.HostURI);
        }

        [Test]
        public void TestCustomHost()
        {
            m_api = new Api("example.com", provider_key);
            Assert.AreEqual("example.com", m_api.HostURI);
        }

        [Test]
        public void TestAuthrepUsageIsEncoded()
        {
            WebRequest.RegisterPrefix("test", new MockWebRequestCreate());

            var request = MockWebRequestCreate.CreateMockHttpWebRequestWithGivenResponseCode(HttpStatusCode.OK);
            m_api = new Api("test://backend", provider_key);

            // build a hashtable of parameters
            Hashtable parameters = new Hashtable();

            parameters.Add("app_id", "1a2b3c4d5e6f");

            //Add a metric to the call
            Hashtable usage = new Hashtable();
            usage.Add("hits", "1");
            parameters.Add("usage",usage);

            m_api.authrep(parameters);

            var uri = request.RequestUri;

            Assert.AreEqual("test://backend/", uri.ToString());
        }

        private void AssertAuthrepUrlWithParams(string parameters)
        {
            string authrep_url = "http://" + host + "/transactions/authrep.xml?provider_key=" + provider_key + parameters;


        }
    }
}

