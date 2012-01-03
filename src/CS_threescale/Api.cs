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

        const int APP_ID = 1;
        const int USER_KEY = 2;  

        const string contentType = "application/x-www-form-urlencoded";

        public Api()
        {
            hostURI = "http://su1.3scale.net";
        }

        public Api(string provider_key):this()
        {
            if ((provider_key == null) || (provider_key.Length <= 0)) throw new ApiException("argument error: undefined provider_key");
            this.provider_key = provider_key;
           
        }

        public Api(string hostURI, string provider_key):this(provider_key)
        {
            if ((hostURI == null) || (hostURI.Length <= 0)) throw new ApiException("argument error: undefined server");
            this.hostURI = hostURI;
        }

        #region IApiCommand Members

        public string HostURI
        {
            get { return hostURI; }
            set { hostURI = value; }
        }

        public AuthorizeResponse authorize(string app_id) {
            return aux_authorize(APP_ID,app_id, null);
        }

        public AuthorizeResponse authorize(string app_id, string app_key) {
            return aux_authorize(APP_ID,app_id, app_key)  
        }

        public AuthorizeResponse authorize_user_key(string user_key) {
            return aux_authorize(USER_KEY,user_key, null)  
        }

        private AuthorizeResponse aux_authorize(int key_type, string id, string key) {

            if ((key_type==APP_ID) && ((id==null) || (id.Length <= 0))) throw new ApiException("argument error: undefined app_id");
            if ((key_type==USER_KEY) && ((id==null) || (id.Length <= 0))) throw new ApiException("argument error: undefined user_key");

            string URL = hostURI + "/transactions/authorize.xml";

            string content = "?provider_key=" + provider_key;

            if (key_type==USER_KEY) content = "&user_key=" + id;
            else content ? "&app_id=" + id; 
              
            if (key_type==APP_ID) && (key!=null) {
                if (key.Length <= 0) throw new ApiException("argument error: undefined app_key");
                else content = content + "&app_key=" + key;
            }

            URL = URL + content;
                
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

                if (w.Response == null) throw w;

                Stream s = w.Response.GetResponseStream();
                byte[] b = new byte[s.Length];
                s.Read(b, 0, b.Length);
                s.Close();

                ApiError err = null;

                try
                {
                    err = new ApiError(Encoding.UTF8.GetString(b));
                }
                catch (Exception) {
                    err = null;
                }

                if (err != null) throw new ApiException(err.code + " : " + err.message);


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

        
        public void report(Hashtable transactions)
        {

            if ((transactions == null) || (transactions.Count <= 0)) throw new ApiException("argument error: undefined transactions, must be at least one");

            string URL = hostURI + "/transactions.xml";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
          
            request.ContentType = contentType;
            request.Method = "POST";
            string content = "provider_key=" + provider_key;

            AddTransactions(ref content, transactions);

            //Console.WriteLine("content: " + content);

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
                if (w.Response == null) throw w;

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

                if (err != null) throw new ApiException(err.code + " : " + err.message);

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
        

        
        private void AddTransactions(ref string content, Hashtable transactions)
        {
            string app_id;
            //string client_ip;
            string timestamp;
            Hashtable obj;
            Hashtable usage;

            foreach (DictionaryEntry entri in transactions) 
            {
                app_id = null;
                //client_ip = null;
                timestamp = null;
                obj = null;
                usage = null;

                obj = (Hashtable)entri.Value;

               	app_id = (string)obj["app_id"];
                user_key = (string)obj["user_key"];
                //client_ip = (string)obj["client_ip"];
                timestamp = (string)obj["timestamp"];
                usage = (Hashtable)obj["usage"];

                //if ((app_id == null) || (app_id.Length <= 0)) throw new ApiException("argument error: undefined transaction, app_id is missing in one record");
                if ((usage == null) || (usage.Count <= 0)) throw new ApiException("argument error: undefined transaction, usage is missing in one record");

                if ((timestamp!=null) && (timestamp.Length <=0)) timestamp=null;
                
                if ((app_id!=null) && (app_id.Length>0)) {
                    content = content + string.Format("&transactions[{0}][{1}]={2}",entri.Key,"app_id",app_id);
                }
				
                if ((user_key!=null) && (user_key.Length>0)) {
                    content = content + string.Format("&transactions[{0}][{1}]={2}",entri.Key,"user_key",user_key);
                }
				
                if (timestamp!=null) content = content + string.Format("&transactions[{0}][{1}]={2}",entri.Key,"timestamp",timestamp);

                foreach (DictionaryEntry entri_usage in usage) 
                {
                    content = content + string.Format("&transactions[{0}][{1}][{2}]={3}",entri.Key,"usage",entri_usage.Key,entri_usage.Value);
                }
            }

        }
        
        
        #endregion
    }
}
