using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using CS_threescale;

namespace CS_threescale
{
    [Serializable]
    [XmlRoot("error")]

    public class ApiError
    {
        
        public ApiError(string xmlResponse)
        {
            ApiError ae = SerializeHelper<ApiError>.Ressurect(xmlResponse);
            this.code = ae.code;
            this.message = ae.message;
        }

        public ApiError() { 
        }
         

        [XmlAttribute("code")]
        public string code;
        [XmlText()]
        public string message;

    }
   
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message) { }
    }

}
