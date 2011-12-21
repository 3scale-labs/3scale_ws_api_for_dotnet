using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CS_threescale
{
    public interface IApi
    {
        AuthorizeResponse authorize(string app_id);
        AuthorizeResponse authorize_user_key(string user_key);
        AuthorizeResponse authorize(string app_id, string app_key);
        void report(Hashtable transactions);
       
        string HostURI
        {
            get;set;
        }
    }
}
