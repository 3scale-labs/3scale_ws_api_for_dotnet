using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using CS_threescale;

namespace CS_threescale
{
    public class UsageItem 
    {

        [XmlAttribute("period")] public string period;
        [XmlAttribute("metric")] public string metric;
        [XmlElement("period_start")] public string period_start;
        [XmlElement("period_end")] public string period_end;
        [XmlElement("current_value")] public int current_value;
        [XmlElement("max_value")] public int max_value;
      
        public UsageItem()
        {
        }

    }
         
}
