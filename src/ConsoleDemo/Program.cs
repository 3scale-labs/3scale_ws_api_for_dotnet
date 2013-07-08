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
                //string provider_key = "6c8ad2ee6fd8471902b226283281583e";
                //string app_id = "69eac0c8";

                string provider_key = "24e03d2127fd2089220d1bbc45a08ae3";
                string service_id = "1006371741601";
                string app_id = "802a6aaa";
                string app_key = "b230a8663a9c367d0459651cc1661bf3";

                Api _3ScaleAPI = new Api(provider_key);

                // Try authorize
                Hashtable parameters = new Hashtable();
                parameters.Add("app_id", app_id);
                parameters.Add("app_key", app_key);
                parameters.Add("service_id", service_id);

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
                transaction.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss K"));
                usage = new System.Collections.Hashtable();
                usage.Add("hits", 10);
                transaction.Add("usage", usage);
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

            string s = Console.ReadLine();
        }
    }
}