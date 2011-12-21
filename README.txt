
3scale .NET API HOWTO
---------------------


1) Obtain the library

Get the CS_threescale.dll (Windows version)

It is highly suggested to rebuild the dll from the source using Mono or Microsoft Visual Studio

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

3.2) Authorize 

There are two modes of authorization:

3.2.1) With app_id/{app_key}

AuthorizeResponse resp = _3ScaleAPI.authorize(APPLICATION_ID);

or 

AuthorizeResponse resp = _3ScaleAPI.authorize(APPLICATION_ID,APPLICATION_KEY);

3.2.2) With user_key

AuthorizeResponse resp = _3ScaleAPI.authorize_user_key(USER_KEY);


The response for either 3.2.1 or 3.2.2 will be the object AuthorizeResponse or an exception.

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


3.3) Report

_3ScaleAPI.report(transactions);

Where transactions is a Hashtable containing a list of transaction. For instance:


System.Collections.Hashtable transactions = new System.Collections.Hashtable();
System.Collections.Hashtable transaction = null;
System.Collections.Hashtable usage = null;

transaction = new System.Collections.Hashtable();

In the case of app_id mode of authorization, add the app_id like this

transaction.Add("app_id",app_id);

Otherwise, if using a user_key add the key like this

transaction.Add("user_key",user_key);

It's important to note that it's either app_id or user_key, it cannot be both at the
same time


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
transaction.Add("app_id", app_id); or transaction.Add("user_key", user_key);
transaction.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss K"));
usage = new System.Collections.Hashtable();
usage.Add("hits", 1);
transaction.Add("usage", usage);
transactions.Add("1", transaction);

_3ScaleAPI.report(transactions);


4) Legal

Copyright (c) 2008 3scale networks S.L., released under the MIT license.



