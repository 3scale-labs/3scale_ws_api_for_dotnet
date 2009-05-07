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
            hostURI = "http://server.3scale.net";
        }

        public Api(string provider_key):this()
        {
            this.provider_key = provider_key;
        }

        public Api(string hostURI, string provider_key):this(provider_key)
        {
            this.hostURI = hostURI;
        }

        #region IApiCommand Members

        public string HostURI
        {
            get { return hostURI; }
            set { hostURI = value; }
        }
        public TransactionData Start(string user_contract_key, Hashtable metrics)
        {
            string URL = hostURI + "/transactions.xml";
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(URL);
            
            request.ContentType = contentType;
            request.Method = "POST";
            string content =
                "user_key=" + user_contract_key + "&provider_key=" + provider_key;
            if (metrics != null)
            {
                AddMetrics(ref content, metrics);
            }
           
            byte[] data = Encoding.UTF8.GetBytes(content);
            request.ContentLength = data.Length;

            
            try
            {
                request.ContentLength = data.Length;
                Stream str = request.GetRequestStream();
                str.Write(data, 0, data.Length);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //LastTransaction = null;
                    Stream s = response.GetResponseStream();
                    List<byte> st = new List<byte>();
                    //byte[] b = new byte[s.Length];
                    //s.Read(b, 0, b.Length);
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
                            TransactionData Transact = new TransactionData(Encoding.UTF8.GetString(b));
                            //_error = ApiError.Success;
                            s.Close();
                            return Transact;
                           
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

                switch (((HttpWebResponse)w.Response).StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new BadRequestException(Encoding.UTF8.GetString(b));

                    case HttpStatusCode.Forbidden:
                        throw new ForbiddenException(Encoding.UTF8.GetString(b));

                    case HttpStatusCode.InternalServerError:
                        throw new InternalException(Encoding.UTF8.GetString(b));

                }
                
            }
            return null;
        }

        private void AddMetrics(ref string content, Hashtable metrics)
        {

            foreach (DictionaryEntry entri in metrics)
            {
                content = content + string.Format("&usage[{0}]={1}", entri.Key, entri.Value);
            }
           
        }

        public TransactionData Start(string user_contract_key)
        {
            return Start(user_contract_key, null);
        }

        public void Confirm(string transaction_id)
        {
            Confirm(transaction_id, null);
        }
        public void Confirm(string transaction_id, Hashtable metrics)
        {
           
            string URL = hostURI + "/transactions/" +
            transaction_id + "/confirm.xml";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.ContentType = contentType;
            request.Method = "POST";

            string content =
                "provider_key=" + provider_key;

            if (metrics != null)
            {
                AddMetrics(ref content, metrics);
            }
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
                    //byte[] b = new byte[s.Length];
                    //s.Read(b, 0, b.Length);
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
                            //LastTrasaction = new TransactionData(Encoding.UTF8.GetString(b));
                            //s.Close();
                            break;
                        
                    }
                    s.Close();
                }

            }
            catch (WebException w)
            {
                if (w.Response == null)
                    throw w;

                Stream s = w.Response.GetResponseStream();
                List<byte> st = new List<byte>();
                //byte[] b = new byte[s.Length];
                //s.Read(b, 0, b.Length);
                int ct = 0;
                while ((ct = s.ReadByte()) != -1)
                {
                    st.Add((byte)ct);
                }
                byte[] b = st.ToArray();
                st.Clear();

                switch (((HttpWebResponse)w.Response).StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new BadRequestException(Encoding.UTF8.GetString(b));

                    case HttpStatusCode.Forbidden:
                        throw new ForbiddenException(Encoding.UTF8.GetString(b));
                    case HttpStatusCode.NotFound:
                        throw new NotFoundException(Encoding.UTF8.GetString(b));
                    case HttpStatusCode.InternalServerError:
                        throw new InternalException(Encoding.UTF8.GetString(b));
                }
                throw new Exception("WebException", w);
            }
        }

        public bool Cancel(string transaction_id)
        {
            string URL = hostURI.Insert(hostURI.Length, string.Format("/transactions/{0}.xml?_method=delete&provider_key={1}", transaction_id, provider_key));
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                   
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                        
                            return true;
                    }
                }
            }
            catch (WebException w)
            {
                if (w.Response == null)
                    throw w;

                Stream s = w.Response.GetResponseStream();
                byte[] b = new byte[s.Length];
                s.Read(b, 0, b.Length);

                switch (((HttpWebResponse)w.Response).StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        throw new ForbiddenException(Encoding.UTF8.GetString(b));
                    case HttpStatusCode.NotFound:
                        throw new NotFoundException(Encoding.UTF8.GetString(b));
                    case HttpStatusCode.InternalServerError:
                        throw new InternalException(Encoding.UTF8.GetString(b));
                }
                throw new Exception("WebException", w);
            }
            return false;
        }

        #endregion
    }
}
