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
    public class ErrorDesc
    {
        
        protected string id;

        [XmlAttribute("id")]
        public string ID
        {
            get
            {
                return id;
            }
            set { id = value; }
        }

        
        protected string _serverMessage;
        [XmlText()]
        public string ServerMessage
        {
            get { return _serverMessage; }
            set { _serverMessage = value; }
        }
    }
   
    public class ApiException:Exception
    {
        public ErrorDesc ErrorReturn;

        public ApiException():base("Api Error") 
        {
            
        }

        public ApiException(string xmlResponse)
            : this()
        {
            ErrorDesc err =
                SerializeHelper<ErrorDesc>.Ressurect(xmlResponse);

            ErrorReturn = err;
        }

        //static ApiError _errorSuccess;
        //public static ApiError Success
        //{
        //    get{
        //        if (_errorSuccess == null)
        //        {
        //            _errorSuccess = new ApiError();
        //            _errorSuccess.id = "";
        //            _errorSuccess._serverMessage = "Success";
        //        }
        //        return _errorSuccess;
        //    }
        //}
    }

    public class BadRequestException : ApiException
    {
        public BadRequestException() { }
        public BadRequestException(string xmlResponse) : base(xmlResponse) { }
    }

    public class ForbiddenException : ApiException
    {
        public ForbiddenException() { }
        public ForbiddenException(string xmlResponse) : base(xmlResponse) { }
    }

    public class NotFoundException : ApiException
    {
        public NotFoundException() { }
        public NotFoundException(string xmlResponse) : base(xmlResponse) { }
    }

    public class InternalException : ApiException
    {
        public InternalException() { }
        public InternalException(string xmlResponse) : base(xmlResponse) { }
    }
}
