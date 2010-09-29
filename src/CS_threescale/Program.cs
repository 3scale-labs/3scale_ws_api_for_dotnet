using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net;

namespace CS_threescale
{
    /*
     * Due to your request about transaction consecutive usage, 
     * we have changed the way of using Api object.
     * Notice its usage is on sample below
     */


    class Program
    {
        //user_key=3scale-9f58660be36c0add4d52198c7c9ecb6e&provider_key=3scale-5fc9d398ac038e4e8f212cc1e8cf01d2
        static void Main(string[] args)
        {
        Api _3ScaleAPI = new Api("http://server.3scale.net", "3scale-5fc9d398ac038e4e8f212cc1e8cf01d2");

       

        Console.WriteLine("====>Test 1:Success<======");

        string resp = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><status><plan>Demo</plan><usage period=\"day\" metric=\"hits\"><period_start>2010-08-01 22:00:00</period_start><period_end>2010-08-02 21:59:59</period_end><current_value>0</current_value><max_value>10000</max_value></usage><usage period=\"hour\" metric=\"demoapiupload\"><period_start>2010-08-02 09:00:00</period_start><period_end>2010-08-02 09:59:59</period_end><current_value>0</current_value><max_value>1000</max_value></usage><usage period=\"hour\" metric=\"demoapiretrieve\"><period_start>2010-08-02 09:00:00</period_start><period_end>2010-08-02 09:59:59</period_end><current_value>0</current_value><max_value>1000</max_value></usage><usage period=\"hour\" metric=\"demoapisearch\"><period_start>2010-08-02 09:00:00</period_start><period_end>2010-08-02 09:59:59</period_end><current_value>0</current_value><max_value>2000</max_value></usage><usage period=\"day\" metric=\"demoapisearch\"><period_start>2010-08-01 22:00:00</period_start><period_end>2010-08-02 21:59:59</period_end><current_value>0</current_value><max_value>5000</max_value></usage></status>";

        AuthorizeResponse auth_response = new AuthorizeResponse(Encoding.UTF8.GetString(resp));
            
        }

    }
    

    /*
    class Program
    {
        static Api _3ScaleAPI;
        static Hashtable costs;

        static void Main(string[] args)
        {
            _3ScaleAPI = new Api("5b20da34365d584af6476697453935");
            _3ScaleAPI.HostURI = "http://www.3scale.net";

            //you may set server address directly in constructor
            //Api _3ScaleAPI = new Api("http://beta.3scale.net", "5b20da34365d584af647669745393560"); 

            costs = new Hashtable();
            costs.Add("hits", "1");
            costs.Add("storage", "20480");

            Console.WriteLine("=================Functional test==================");
            Console.WriteLine("1) start (with usage), confirm");
            if (Test1())
                Console.WriteLine("====>Test 1:Success<======");
            else
                Console.WriteLine("====>Test 1:not passed)");

            Console.WriteLine("2) start, confirm (with usage)");
            if (Test2())
                Console.WriteLine("====>Test 2:success<======");
            else
                Console.WriteLine("====>Test 2:not passed)");

            Console.WriteLine("3) start, confirm (with usage), confirm (with usage) (that should not give any error)");
            if (Test3())
                Console.WriteLine("====>Test 3:Success<======");
            else
                Console.WriteLine("====>Test 3:not passed)");

            Console.WriteLine("4) start, cancel");
            if (Test4())
                Console.WriteLine("====>Test 4:Success<======");
            else
                Console.WriteLine("====>Test 4:not passed)");

            Console.WriteLine("5) start, confirm (with usage), cancel");
            if (Test5())
                Console.WriteLine("====>Test 5:Success<======");
            else
                Console.WriteLine("====>Test 5:not passed)");

            Console.WriteLine("================Exception testing =================");
            Console.WriteLine("1) start with the wrong user key");
            ApiException excp = ErrorTest1();
            if (excp != null)
            {
                PrintApiException(excp);
                Console.WriteLine("====>Error Test #1 Succes<======");
            }
            else
                Console.WriteLine("No Error Occured");

            Console.WriteLine("2) start, confirm with the wrong transaction id)");
            excp = ErrorTest2();
            if (excp != null)
            {
                PrintApiException(excp);
                Console.WriteLine("====>Error Test #2 Succes<======");
            }
            else
                Console.WriteLine("No Error Occured");

            Console.WriteLine("3) start, cancel, confirm ");
            excp = ErrorTest3();
            if (excp != null)
            {
                PrintApiException(excp);
                Console.WriteLine("====>Error Test #3 Succes<======");
            }
            else
                Console.WriteLine("No Error Occured");

            Console.WriteLine("4) start, cancel, cancel");

            excp = ErrorTest4();
            if (excp != null)
            {
                PrintApiException(excp);
                Console.WriteLine("====>Error Test #4 Succes<======");
            }
            else
                Console.WriteLine("No Error Occured");


            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static void PrintApiException(ApiException err)
        {
            Console.WriteLine("Error occured : " + err.GetType().Name + "\n");
            Console.WriteLine("Error type : " + err.ErrorReturn.ID + "\n");
            Console.WriteLine("Description : " + err.ErrorReturn.ServerMessage + "\n");
        }

        static bool Test1() 
        {
            TransactionData tdata;

            try
            {
                Console.WriteLine("Starting Transaction");
                tdata = _3ScaleAPI.Start("526843c1e7f45003646ed13b6c885a61", costs);

                Console.WriteLine("Transaction data:");
                Console.WriteLine("ID :\t" + tdata.ID);
                Console.WriteLine("Contract name :\t" + tdata.ContractName);
                Console.WriteLine("Start transaction success");

                Console.WriteLine("Confirming Transaction");
               
                _3ScaleAPI.Confirm(tdata.ID);

                Console.WriteLine("Transaction confirmed");
                return true;

            }
            catch (ApiException err)
            {
                PrintApiException(err);
            }
            catch (WebException netEx)
            {
                Console.WriteLine("Network exception : " + netEx.Message);
            }
            catch (Exception genEx)
            {
                Console.WriteLine("General exception : " + genEx.Message);
            }

        
            return false;
        }

        static bool Test2()
        {
            TransactionData tdata;

            try
            {
                Console.WriteLine("Starting Transaction");
                tdata = _3ScaleAPI.Start("526843c1e7f45003646ed13b6c885a61");

                Console.WriteLine("Transaction data:");
                Console.WriteLine("ID :\t" + tdata.ID);
                Console.WriteLine("Contract name :\t" + tdata.ContractName);
                Console.WriteLine("Start transaction success");

                Console.WriteLine("Confirming Transaction");
                _3ScaleAPI.Confirm(tdata.ID, costs);
                Console.WriteLine("Transaction confirmed");
                return true;
            }
            catch (ApiException err)
            {
                Console.WriteLine("Error occured : " + err.GetType().Name + "\n");
                Console.WriteLine("Error type : " + err.ErrorReturn.ID + "\n");
                Console.WriteLine("Description : " + err.ErrorReturn.ServerMessage + "\n");
            }
            catch (WebException netEx)
            {
                Console.WriteLine("Network exception : " + netEx.Message);
            }
            catch (Exception genEx)
            {
                Console.WriteLine("General exception : " + genEx.Message);
            }
            return false;
        }

        static bool Test3()
        {
            TransactionData tdata;

            try
            {
                Console.WriteLine("Starting Transaction");
                tdata = _3ScaleAPI.Start("526843c1e7f45003646ed13b6c885a61");

                Console.WriteLine("Transaction data:");
                Console.WriteLine("ID :\t" + tdata.ID);
                Console.WriteLine("Contract name :\t" + tdata.ContractName);
                Console.WriteLine("Start transaction success");

                Console.WriteLine("Confirming Transaction");


                //3# Confirm transaction without using costs
                _3ScaleAPI.Confirm(tdata.ID, costs);
                _3ScaleAPI.Confirm(tdata.ID, costs);
                //#4Confirm transaction by using costs;
                //_3ScaleAPI.Confirm(tdata.ID,costs); 

                Console.WriteLine("Transaction confirmed");

                return true;

            }
            catch (ApiException err)
            {
                PrintApiException(err);
            }
            catch (WebException netEx)
            {
                Console.WriteLine("Network exception : " + netEx.Message);
            }
            catch (Exception genEx)
            {
                Console.WriteLine("General exception : " + genEx.Message);
            }
            return true;
        }

        static bool Test4()
        {
            TransactionData tdata;

            try
            {
                Console.WriteLine("Starting Transaction");
                tdata = _3ScaleAPI.Start("526843c1e7f45003646ed13b6c885a61");

                Console.WriteLine("Transaction data:");
                Console.WriteLine("ID :\t" + tdata.ID);
                Console.WriteLine("Contract name :\t" + tdata.ContractName);
                Console.WriteLine("Start transaction success");

                Console.WriteLine("Confirming Transaction");


                Console.WriteLine("Cancel Transaction");
                _3ScaleAPI.Cancel(tdata.ID);
                Console.WriteLine("Transaction Canceled");
                return true;
            }
            catch (ApiException err)
            {
                Console.WriteLine("Error occured : " + err.GetType().Name + "\n");
                Console.WriteLine("Error type : " + err.ErrorReturn.ID + "\n");
                Console.WriteLine("Description : " + err.ErrorReturn.ServerMessage + "\n");
            }
            catch (WebException netEx)
            {
                Console.WriteLine("Network exception : " + netEx.Message);
            }
            catch (Exception genEx)
            {
                Console.WriteLine("General exception : " + genEx.Message);
            }
            return false;
        }

        static bool Test5()
        {
            TransactionData tdata;

            try
            {
                Console.WriteLine("Starting Transaction");
                tdata = _3ScaleAPI.Start("526843c1e7f45003646ed13b6c885a61");

                Console.WriteLine("Transaction data:");
                Console.WriteLine("ID :\t" + tdata.ID);
                Console.WriteLine("Contract name :\t" + tdata.ContractName);
                Console.WriteLine("Start transaction success");

                Console.WriteLine("Confirming Transaction");

                //#4Confirm transaction by using costs;
                _3ScaleAPI.Confirm(tdata.ID,costs); 

                Console.WriteLine("Transaction confirmed");

                Console.WriteLine("Cancel Transaction");
                _3ScaleAPI.Cancel(tdata.ID);
                Console.WriteLine("Transaction Canceled");
                return true;
            }
            catch (ApiException err)
            {
               PrintApiException(err);
            }
            catch (WebException netEx)
            {
                Console.WriteLine("Network exception : " + netEx.Message);
            }
            catch (Exception genEx)
            {
                Console.WriteLine("General exception : " + genEx.Message);
            }
            return false;
        }

        static ApiException ErrorTest1()
        {
            TransactionData tdata;

            try
            {
                Console.WriteLine("Starting transaction with wrong user key");
                tdata = _3ScaleAPI.Start("Wrong User Key");
            }
            catch (ApiException err)
            {
                return err;
            }
            catch (WebException netEx)
            {
                Console.WriteLine("Network exception : " + netEx.Message);
                return null;
            }
            catch (Exception genEx)
            {
                Console.WriteLine("General exception : " + genEx.Message);
                return null;
            }

            return null;
        }

        static ApiException ErrorTest2()
        {
            TransactionData tdata;

            try
            {
                Console.WriteLine("Starting Transaction");
                tdata = _3ScaleAPI.Start("526843c1e7f45003646ed13b6c885a61");

                Console.WriteLine("Transaction data:");
                Console.WriteLine("ID :\t" + tdata.ID);
                Console.WriteLine("Contract name :\t" + tdata.ContractName);
                Console.WriteLine("Start transaction success");

                Console.WriteLine("Confirming transaction with wrong ID");
                //3# Confirm transaction without using costs
                _3ScaleAPI.Confirm("Wrong ID");
                

            }
            catch (ApiException err)
            {
                return err;
            }
            catch (WebException netEx)
            {
                Console.WriteLine("Network exception : " + netEx.Message);
            }
            catch (Exception genEx)
            {
                Console.WriteLine("General exception : " + genEx.Message);
            }
            return null;
        }

        static ApiException ErrorTest3()
        {
            TransactionData tdata;

            try
            {
                Console.WriteLine("Starting Transaction");
                tdata = _3ScaleAPI.Start("526843c1e7f45003646ed13b6c885a61");

                Console.WriteLine("Transaction data:");
                Console.WriteLine("ID :\t" + tdata.ID);
                Console.WriteLine("Contract name :\t" + tdata.ContractName);
                Console.WriteLine("Start transaction success");

                Console.WriteLine("Cancel Transaction");
                _3ScaleAPI.Cancel(tdata.ID);
                Console.WriteLine("Transaction Canceled");

                Console.WriteLine("Confirming transaction that canceled");
                //3# Confirm transaction without using costs
                _3ScaleAPI.Confirm(tdata.ID);

                //#4Confirm transaction by using costs;
                //_3ScaleAPI.Confirm(tdata.ID,costs); 

                Console.WriteLine("Transaction confirmed");

                

            }
            catch (ApiException err)
            {
                return err;
            }
            catch (WebException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        static ApiException ErrorTest4()
        {
            TransactionData tdata;

            try
            {
                Console.WriteLine("Starting Transaction");
                tdata = _3ScaleAPI.Start("526843c1e7f45003646ed13b6c885a61");

                Console.WriteLine("Transaction data:");
                Console.WriteLine("ID :\t" + tdata.ID);
                Console.WriteLine("Contract name :\t" + tdata.ContractName);
                Console.WriteLine("Start transaction success");
                Console.WriteLine("Cancel Transaction");
                _3ScaleAPI.Cancel(tdata.ID);
                Console.WriteLine("Transaction Canceled");
                Console.WriteLine("Cancel transaction again");
                _3ScaleAPI.Cancel(tdata.ID);
                Console.WriteLine("Transaction Canceled");

            }
            catch (ApiException err)
            {
                return err;
            }
            catch (WebException netEx)
            {
                Console.WriteLine("Network exception : " + netEx.Message);
                return null;
            }
            catch (Exception genEx)
            {
                Console.WriteLine("General exception : " + genEx.Message);
                return null;
            }
            return null;
        }
    }

    */

}
