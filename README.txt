
3scale .NET API HOWTO
---------------------


1) Obtain the library

Get the CS_threescale.dll (Windows version) 
Alternatively, you can rebuild the dll using Mono or other versions of Visual Studio
(the library source is in threescale.net.src.tar.gz)

2) Import the 3scale lib to your C sharp code

using CS_threescale;

3scale library depends on:

using System;
using System.Collections.Generic;
using CS_threescale;
using System.Text;
using System.Net

3) Examples of usage:

3.1) Initialize

Api _3ScaleAPI = new Api(3SCALE_SERVER,PROVIDER_KEY);

3.1) Authorize

AuthorizeResponse resp = _3ScaleAPI.authorize(APPLICATION_ID);

or 

AuthorizeResponse resp = _3ScaleAPI.authorize(APPLICATION_ID,APPLICATION_KEY);

The response will be the object AuthorizeResponse or an exception.

The object AuthorizeReponse can be printed with this function:

static void print(AuthorizeResponse ar) 
{
    if (ar.authorized) {
        Console.WriteLine("Authorized!!");
    }
    else {
        Console.WriteLine("NOT Authorized!!" + ar.reason);
    }

    Console.WriteLine("PLAN: " + ar.plan);

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



3.2) Report

_3ScaleAPI.report(transactions);

Where transactions is a Hashtable containing a list of transaction. For instance:


System.Collections.Hashtable transactions = new System.Collections.Hashtable();
System.Collections.Hashtable transaction = null;
System.Collections.Hashtable usage = null;

transaction = new System.Collections.Hashtable();
transaction.Add("app_id",app_id);

// Timestamp is optional, if left undefined it will take the current time. If defined
// it must have the format "yyyy-MM-dd HH:mm:ss K"
transaction.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss K"));

// Usage is a Hashtable that contains the metrics to be reported
usage = new System.Collections.Hashtable();
usage.Add("hits", 10);
transaction.Add("usage",usage);

// Adding the transaction to transactions
transactions.Add("0", transaction);


// create a second transaction to be reported in a single _3ScaleAPI.report()
transaction = new System.Collections.Hashtable();
transaction.Add("app_id", app_id);
transaction.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss K"));
usage = new System.Collections.Hashtable();
usage.Add("hits", 1);
transaction.Add("usage", usage);
transactions.Add("1", transaction);

_3ScaleAPI.report(transactions);


4) Legal

Copyright (c) 2008 3scale networks S.L., released under the MIT license.

