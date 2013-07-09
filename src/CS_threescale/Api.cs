using System;
using System.Collections.Generic;
using System.Text;
using CS_threescale;
using System.Net;
using System.IO;
using System.Collections;

namespace CS_threescale
{
    public class Api:IApi
    {
        string provider_key;
        string hostURI;
        const string contentType = "application/x-www-form-urlencoded";

        public Api()
        {
            hostURI = "http://su1.3scale.net";
        }

        public Api(string provider_key):this()
        {
            if (string.IsNullOrEmpty(provider_key))
                throw new ApiException("argument error: undefined provider_key");
            this.provider_key = provider_key;
           
        }

        public Api(string hostURI, string provider_key):this(provider_key)
        {
            if (string.IsNullOrEmpty(hostURI))
                throw new ApiException("argument error: undefined server");
            this.hostURI = hostURI;
        }

        public string HostURI
        {
            get { return hostURI; }
            set { hostURI = value; }
        }

        /// <summary>
        /// Calls Authrep on the 3scale backend to authorize an application and report the associated transaction at the same time.
        /// </summary>
        /// <param name="parameters">A Hashtable of parameter name value pairs</param>
        public AuthorizeResponse authrep(Hashtable parameters)
        {
            return CallAuthorize("/transactions/authrep.xml", parameters);
        }

        /// <summary>
        /// Calls Authorize on the 3scale backend to:
        /// Check an application exists, is active and within limits. 
        /// Can also be used to authenticate a call using the required authentication parameters
        /// </summary>
        /// <param name="parameters">A Hashtable of parameter name value pairs</param>
        public AuthorizeResponse authorize(Hashtable parameters)
        {
            return CallAuthorize("/transactions/authorize.xml", parameters);
        }

        /// <summary>
        /// Report the specified transactions to the 3scale backend.
        /// </summary>
        /// <param name="transactions">A Hashtable containing the transactions to be reported</param>
        public void report(Hashtable transactions)
        {
            if ((transactions == null) || (transactions.Count <= 0))
                throw new ApiException("argument error: undefined transactions, must be at least one");

            string URL = hostURI + "/transactions.xml";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
          
            request.ContentType = contentType;
            request.Method = "POST";
            string content = "provider_key=" + provider_key;

            AddTransactions(ref content, transactions);

            Console.WriteLine("content: " + content);
            
            byte[] data = Encoding.UTF8.GetBytes(content);
            request.ContentLength = data.Length;

            try
            {
                request.ContentLength = data.Length;
                Stream str = request.GetRequestStream();
                str.Write(data, 0, data.Length);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream s = response.GetResponseStream();
                    List<byte> st = new List<byte>();
                    int ct = 0;
                    while ((ct = s.ReadByte()) != -1)
                    {
                        st.Add((byte)ct);
                    }
                    byte[] b = st.ToArray();
                    st.Clear();

                    //Console.WriteLine(".--------------- " + response.StatusCode + " :::: " + HttpStatusCode.OK);
                    
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            s.Close();
                            return;
                        case HttpStatusCode.Accepted:
                            s.Close();
                            return;

                    }
                    s.Close();
                }
            }
            catch (WebException w)
            {
                if (w.Response == null)
                    throw w;

                Stream s = w.Response.GetResponseStream();
                byte[] b = new byte[s.Length];
                s.Read(b, 0, b.Length);
                ApiError err = null;

                try
                {
                    err = new ApiError(Encoding.UTF8.GetString(b));
                }
                catch (Exception)
                {
                    err = null;
                }

                if (err != null)
                    throw new ApiException(err.code + " : " + err.message);

                switch (((HttpWebResponse)w.Response).StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        throw new ApiException("Forbidden");

                    case HttpStatusCode.BadRequest:
                        throw new ApiException("Bad request");

                    case HttpStatusCode.InternalServerError:
                        throw new ApiException("Internal server error");

                    case HttpStatusCode.NotFound:
                        throw new ApiException("Request route not found");

                    default:
                        throw new ApiException("Unknown Exception: " + Encoding.UTF8.GetString(b));
                }
            }

            return;
        }

        /// <summary>
        /// Calls Authorize on the 3scale backend with the specified parameters using the Oauth authentication pattern.
        /// </summary>
        /// <param name="parameters">A Hashtable of parameter name value pairs</param>
        public AuthorizeResponse oauth_authorize(Hashtable parameters)
        {
            return CallAuthorize("/transactions/oauth_authorize.xml", parameters);
        }

        /// <summary>
        /// Executes a different Authorize calls on the 3scale backend depending on the endpoint
        /// </summary>
        /// <returns>An AuthorizeResponse object representing the authorize response</returns>
        /// <param name="endPoint">The endpoint to hit</param>
        /// <param name="parameters">A Hashtable of parameter name value pairs.</param>
        private AuthorizeResponse CallAuthorize(string endPoint, Hashtable parameters)
        {
            string URL = hostURI + endPoint;

            string content = "?provider_key=" + provider_key;

            if (!parameters.ContainsKey("usage"))
            {
                Hashtable usage = new Hashtable();
                usage.Add("hits", "1");
                parameters.Add("usage", usage);
            }

            AddParameters(ref content, parameters);

            URL += content;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream s = response.GetResponseStream();
                    List<byte> st = new List<byte>();
                    int ct = 0;
                    while ((ct = s.ReadByte()) != -1)
                    {
                        st.Add((byte)ct);
                    }
                    byte[] b = st.ToArray();
                    st.Clear();

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            AuthorizeResponse auth_response = new AuthorizeResponse(Encoding.UTF8.GetString(b));
                            s.Close();
                            return auth_response;
                    }
                    s.Close();
                }
            }
            catch (WebException w)
            {
                if (w.Response == null)
                    throw w;

                Stream s = w.Response.GetResponseStream();
                byte[] b = new byte[s.Length];
                s.Read(b, 0, b.Length);
                s.Close();

                ApiError err = null;

                try
                {
                    err = new ApiError(Encoding.UTF8.GetString(b));
                }
                catch (Exception)
                {
                    err = null;
                }

                if (err != null)
                    throw new ApiException(err.code + " : " + err.message);

                switch (((HttpWebResponse)w.Response).StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        throw new ApiException("Forbidden");

                    case HttpStatusCode.BadRequest:
                        throw new ApiException("Bad request");

                    case HttpStatusCode.InternalServerError:
                        throw new ApiException("Internal server error");

                    case HttpStatusCode.NotFound:
                        throw new ApiException("Request route not found");

                    case HttpStatusCode.Conflict:
                        AuthorizeResponse auth_response = new AuthorizeResponse(Encoding.UTF8.GetString(b));
                        return auth_response;

                    default:
                        throw new ApiException("Unknown Exception: " + Encoding.UTF8.GetString(b));
                }
            }

            return null;
        }

        /// <summary>
        /// Encodes the parameters in the content part of the URL
        /// </summary>
        /// <param name="content">A string containing the content part of the URL</param>
        /// <param name="parameters">A Hashtable of parameter name value pairs</param>
        private void AddParameters(ref string content, Hashtable parameters)
        {
            foreach (string parameter in parameters.Keys)
            {
                if (parameter.Equals("usage"))
                {
                    Hashtable usage = (Hashtable)parameters[parameter];

                    if ((usage == null) || (usage.Count <= 0))
                        throw new ApiException("argument error: usage is missing");

                    foreach (string metric in usage.Keys)
                    {
                        content += string.Format("&{0}[{1}]={2}", parameter, metric, usage[metric]);
                    }
                }
                else
                {
                    content += string.Format("&{0}={1}", parameter, parameters[parameter]);
                }
            }
        }

        /// <summary>
        /// Encodes the transactions in the content part of the URL
        /// </summary>
        /// <param name="content">A string containing the content part of the URL</param>
        /// <param name="transactions">A Hashtable of transactions</param>
        private void AddTransactions(ref string content, Hashtable transactions)
        {
            foreach (var entry in transactions.Keys)
            {
                Hashtable transaction = (Hashtable)transactions[entry];

                foreach (string parameter in transaction.Keys)
                {
                    if (parameter.Equals("usage"))
                    {
                        Hashtable usage = (Hashtable)transaction[parameter];

                        if ((usage == null) || (usage.Count <= 0))
                            throw new ApiException("argument error: undefined transaction, usage is missing in one record");

                        foreach (string metric in usage.Keys)
                        {
                            content += string.Format("&transactions[{0}][{1}][{2}]={3}", entry, parameter, metric, usage[metric]);
                        }
                    }
                    else
                    {
                        content += string.Format("&transactions[{0}][{1}]={2}", entry, parameter, transaction[parameter]);
                    }
                }
            }
        }
    }
}
