using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CS_threescale
{
    public interface IApi
    {
        AuthorizeResponse authorize(string app_id);
        AuthorizeResponse authorize(string app_id, string app_key);
        void report(Hashtable transactions);

        string HostURI
        {
            get;set;
        }
    }
}
