using System;
using System.Collections.Generic;
using CS_threescale;
using System.Text;
using System.Net;
using System.Collections;

namespace ConsoleDemo
{


    class Program
    {

        static void print(AuthorizeResponse ar)
        {
            if (ar.authorized) {
                Console.WriteLine("Authorized!!");
            }
            else {
                Console.WriteLine("NOT Authorized!!" + ar.reason);
            }

            Console.WriteLine("PLAN: " + ar.plan);
            Console.WriteLine("#usages: " + ar.usages.Count);

            int i=0;
            foreach( UsageItem item in ar.usages){
                Console.WriteLine("Usage: " + i);
                Console.WriteLine("     Metric:     " + item.metric);
                Console.WriteLine("     Period:     " + item.period);
                Console.WriteLine("     CurrValue:  " + item.current_value);
                Console.WriteLine("     MaxValue:   " + item.max_value);
                Console.WriteLine("     PeriodStart:" + item.period_start);
                Console.WriteLine("     PeriodEnd:  " + item.period_start);
                i++;
            }

        }

        static void Main(string[] args)
        {


            try {
				string provider_key = "YOUR_PROVIDER_KEY";
				string app_id = "YOUR_APP_ID";
				// string app_key = "YOUR_APP_KEY";

                Api _3ScaleAPI = new Api("http://su1.3scale.net", provider_key);

				// Try authorize
				Hashtable parameters = new Hashtable();
				parameters.Add("app_id", app_id);

                AuthorizeResponse resp = _3ScaleAPI.authorize(parameters);

                print(resp);

                Console.WriteLine("Done authorize...");

				// Try authrep
				System.Collections.Hashtable usage = new Hashtable();
				usage.Add("hits", "1");
				parameters.Add("usage", usage);

				AuthorizeResponse authRepResp = _3ScaleAPI.authrep(parameters);

				print(authRepResp);

				Console.WriteLine("Done authrep");

				// Try report
                System.Collections.Hashtable transactions = new System.Collections.Hashtable();
                System.Collections.Hashtable transaction = null;
                transaction = new System.Collections.Hashtable();
                transaction.Add("app_id",app_id);
                transaction.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss K"));
                usage = new System.Collections.Hashtable();
                usage.Add("hits", 10);
                transaction.Add("usage",usage);
                transactions.Add("0", transaction);

                transaction = new System.Collections.Hashtable();
                transaction.Add("app_id", app_id);
                transaction.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss K"));
                usage = new System.Collections.Hashtable();
                usage.Add("hits", 1);
                transaction.Add("usage", usage);
                transactions.Add("1", transaction);
                _3ScaleAPI.report(transactions);

                Console.WriteLine("Done report...");

				// Try oauth_authorize and report
				AuthorizeResponse oAuthResp = _3ScaleAPI.oauth_authorize(parameters);
				print(oAuthResp);
				_3ScaleAPI.report(transactions);

				Console.WriteLine("Done OAuth authorize and report");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            string s = Console.ReadLine();
        }
    }



}
