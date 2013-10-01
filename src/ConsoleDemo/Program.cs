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
            if (ar.authorized)
            {
                Console.WriteLine("Authorized!!");
            }
            else
            {
                Console.WriteLine("NOT Authorized!!" + ar.reason);
            }

            Console.WriteLine("PLAN: " + ar.plan);
            Console.WriteLine("#usages: " + ar.usages.Count);

            int i = 0;
            foreach (UsageItem item in ar.usages)
            {
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
            try
            {
                string provider_key = "YOUR_PROVIDER_KEY";

                // If needed, add your service_id
                //string service_id = "YOUR_SERVICE_ID";

                // Authorise using app_id (and app_key if necessary)
                string app_id = "YOUR_APP_ID";
                string app_key = "YOUR_APP_KEY";

                // Or alternatively you can authorise using user_key
                // string user_key = "YOUR_USER_KEY"

                Api _3ScaleAPI = new Api(provider_key);

                // Try authorize
                Hashtable parameters = new Hashtable();
                // Add service_id if needed
                //parameters.Add("service_id", service_id);

                // Add app_id (and app_key if necessary) to list of parameters to send
                parameters.Add("app_id", app_id);
                parameters.Add("app_key", app_key);

                // Alternatively, add user_key to list of parameters to send
                //parameters.Add("user_key", user_key);

                System.Collections.Hashtable usage = new Hashtable();
                usage.Add("hits", "1");
                parameters.Add("usage", usage);


                AuthorizeResponse resp = _3ScaleAPI.authorize(parameters);

                print(resp);
                Console.WriteLine("Done authorize...");

                // Try authrep

                AuthorizeResponse authRepResp = _3ScaleAPI.authrep(parameters);

                print(authRepResp);
                Console.WriteLine("Done authrep");

                // Try report
                System.Collections.Hashtable transactions = new System.Collections.Hashtable();
                System.Collections.Hashtable transaction = null;
                transaction = new System.Collections.Hashtable();
                transaction.Add("app_id", app_id);
                usage = new System.Collections.Hashtable();
                usage.Add("hits", 10);
                transaction.Add("usage", usage);
                transactions.Add("0", transaction);

                transaction = new System.Collections.Hashtable();
                transaction.Add("app_id", app_id);
                usage = new System.Collections.Hashtable();
                usage.Add("hits", 1);
                transaction.Add("usage", usage);
                transactions.Add("1", transaction);
                
                _3ScaleAPI.report(transactions);

                Console.WriteLine("Done report...");

                // Try oauth_authorize and report
                AuthorizeResponse oAuthResp = _3ScaleAPI.oauth_authorize(parameters);
                print(oAuthResp);

                if(oAuthResp.authorized)
                {
                    _3ScaleAPI.report(transactions);
                    Console.WriteLine("Done OAuth authorize and report");
                }
                else
                {
                    Console.WriteLine("OAuth authorize called, report not done as not authorized");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}