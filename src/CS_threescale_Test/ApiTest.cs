using System;
using NUnit.Framework;
using CS_threescale;
using System.Net;
using Moq;

namespace CS_threescale_Test
{
    [TestFixture]
    public class ApiTest
    {
        private IApi m_api;
        private readonly string host = "http://su1.3scale.net";
        private readonly string provider_key = "12345678";
        private WebRequest request;

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
            m_api = new Api(provider_key, "example.com");
            Assert.AreEqual("example.com", m_api.HostURI);
        }

        private void AssertAuthrepUrlWithParams(string parameters)
        {
            string authrep_url = "http://" + host + "/transactions/authrep.xml?provider_key=" + provider_key + parameters;
        }
    }
}

