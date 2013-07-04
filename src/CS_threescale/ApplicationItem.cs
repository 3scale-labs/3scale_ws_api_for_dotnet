using System;
using System.Xml.Serialization;

namespace CS_threescale
{
	public class ApplicationItem
	{
		[XmlElement("id")]
		public string id;

		[XmlElement("key")]
		public string clientsecret;

		[XmlElement("redirect_url")]
		public string redirecturl;

		public ApplicationItem ()
		{
		}
	}
}

