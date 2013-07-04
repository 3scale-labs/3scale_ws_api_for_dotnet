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
            if (string.IsNullOrEmpty(provider_key)) throw new ApiException("argument error: undefined provider_key");
            this.provider_key = provider_key;
           
        }

        public Api(string hostURI, string provider_key):this(provider_key)
        {
            if (string.IsNullOrEmpty(hostURI)) throw new ApiException("argument error: undefined server");
            this.hostURI = hostURI;
        }

        #region IApiCommand Members

        public string HostURI
        {
            get { return hostURI; }
            set { hostURI = value; }
        }

		public AuthorizeResponse authrep(Hashtable parameters)
		{
			string URL = hostURI + "/transactions/authrep.xml";

			string content = "?provider_key=" + provider_key;

			if (!parameters.ContainsKey ("usage")) 
			{
				Hashtable usage = new Hashtable ();
				usage.Add ("hits", "1");
				parameters.Add ("usage", usage);
			}

			AddAuthRepParameters(ref content, parameters);

			URL += content;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);

			try
			{
				using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
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

        public AuthorizeResponse authorize(Hashtable parameters) 
		{
			string URL = hostURI + "/transactions/authorize.xml";

			string content = "?provider_key=" + provider_key;

			AddParameters (ref content, parameters);

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

		public AuthorizeResponse oauth_authorize(Hashtable parameters)
		{
			string URL = hostURI + "/transactions/oauth_authorize.xml";

			string content = "?provider_key=" + provider_key;

			AddAuthRepParameters (ref content, parameters);

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

		private void AddParameters(ref string content, Hashtable parameters)
		{
			foreach (string parameter in parameters.Keys) 
			{
				content += string.Format ("&{0}={1}", parameter, parameters[parameter]);
			}
		}

		private void AddAuthRepParameters(ref string content, Hashtable parameters)
		{
			foreach (string parameter in parameters.Keys) 
			{
				if(parameter.Equals("usage"))
				{
					Hashtable usage = (Hashtable)parameters[parameter];

					if ((usage == null) || (usage.Count <= 0)) throw new ApiException("argument error: usage is missing");

					foreach (string metric in usage.Keys) 
					{
						content += string.Format("&{0}[{1}]={2}",parameter,metric,usage[metric]);
					}
				}
				else{
					content += string.Format ("&{0}={1}", parameter, parameters[parameter]);
				}
			}
		}

		private void AddTransactions(ref string content, Hashtable transactions)
		{
			foreach(var entry in transactions.Keys)
			{
				Hashtable transaction = (Hashtable)transactions[entry];

				foreach (string parameter in transaction.Keys) 
				{
					if (parameter.Equals ("usage")) 
					{
						Hashtable usage = (Hashtable)transaction[parameter];

						if ((usage == null) || (usage.Count <= 0)) throw new ApiException("argument error: undefined transaction, usage is missing in one record");

						foreach (string metric in usage.Keys) 
						{
							content += string.Format("&transactions[{0}][{1}][{2}]={3}",entry,parameter,metric,usage[metric]);
						}
					} 
					else 
					{
						content += string.Format("&transactions[{0}][{1}]={2}",entry,parameter,transaction[parameter]);
					}
				}
			}
		}

//        private void AddTransactions(ref string content, Hashtable transactions)
//        {
//            string app_id;
//			string service_id;
//            //string client_ip;
//            string timestamp;
//            Hashtable obj;
//            Hashtable usage;
//
//            foreach (DictionaryEntry entri in transactions) 
//            {
//                app_id = null;
//				service_id = null;
//                //client_ip = null;
//                timestamp = null;
//                obj = null;
//                usage = null;
//
//                obj = (Hashtable)entri.Value;
//
//               	app_id = (string)obj["app_id"];
//                string user_key = (string)obj["user_key"];
//                //client_ip = (string)obj["client_ip"];
//                timestamp = (string)obj["timestamp"];
//                usage = (Hashtable)obj["usage"];
//
//                //if ((app_id == null) || (app_id.Length <= 0)) throw new ApiException("argument error: undefined transaction, app_id is missing in one record");
//                if ((usage == null) || (usage.Count <= 0)) throw new ApiException("argument error: undefined transaction, usage is missing in one record");
//
//				// Why are we clearing the TS here?!
//                if (!string.IsNullOrEmpty(timestamp)) timestamp=null;
//                
//                if (!string.IsNullOrEmpty(app_id)) {
//                    content = content + string.Format("&transactions[{0}][{1}]={2}",entri.Key,"app_id",app_id);
//                }
//				
//                if (!string.IsNullOrEmpty(user_key)) {
//                    content = content + string.Format("&transactions[{0}][{1}]={2}",entri.Key,"user_key",user_key);
//                }
//
//				if (!string.IsNullOrEmpty(service_id)) {
//					content = content + string.Format("&transactions[{0}][{1}]={2}",entri.Key,"service_id",service_id);
//				}
//
//                if (timestamp!=null) content = content + string.Format("&transactions[{0}][{1}]={2}",entri.Key,"timestamp",timestamp);
//
//                foreach (DictionaryEntry entri_usage in usage) 
//                {
//                    content = content + string.Format("&transactions[{0}][{1}][{2}]={3}",entri.Key,"usage",entri_usage.Key,entri_usage.Value);
//                }
//            }
//        }
        #endregion
    }
}
