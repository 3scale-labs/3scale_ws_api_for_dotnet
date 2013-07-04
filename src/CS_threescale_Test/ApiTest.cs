using System;
using NUnit.Framework;
using CS_threescale;

namespace CS_threescale_Test
{
	[TestFixture]
	public class ApiTest
	{
		private IApi m_api;

		[Test]
		public void TestDefaultHost ()
		{
			m_api = new Api();
			Assert.AreEqual("http://su1.3scale.net", m_api.HostURI);
		}


	}
}

