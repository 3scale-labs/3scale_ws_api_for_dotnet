using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CS_threescale
{
    public interface IApi
    {
        AuthorizeResponse authrep(Hashtable parameters);

        AuthorizeResponse authorize(Hashtable parameters);

        AuthorizeResponse oauth_authorize(Hashtable parameters);

        void report(Hashtable transactions);

        string HostURI
        {
            get;
            set;
        }
    }
}
