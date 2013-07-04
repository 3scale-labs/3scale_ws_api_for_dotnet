using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using CS_threescale;

namespace CS_threescale
{
    [XmlRoot("status")]
    public class AuthorizeResponse
    {
        public AuthorizeResponse() { }

        public AuthorizeResponse(string xmlResponse)
        {
            AuthorizeResponse tsc = SerializeHelper<AuthorizeResponse>.Ressurect(xmlResponse);
            this.plan = tsc.plan;
            this.reason = tsc.reason;
            this.authorized = tsc.authorized;
            this.usages = tsc.usages;
        }
        
        [XmlElement("plan")]
        public string plan;

        [XmlElement("reason")]
        public string reason;

        [XmlElement("authorized")]
        public bool authorized;

		[XmlElement("application")]
		public ApplicationItem application;

        [XmlArray("usage_reports"), XmlArrayItem("usage_report", typeof(UsageItem))]
        public System.Collections.ArrayList usages;
		
        
        public int AddUsageItem(UsageItem usageitem) 
        {
            return usages.Add(usageitem);
        }

		public string GetClientSecret ()
		{
			return application.clientsecret;
		}
    }

}  

