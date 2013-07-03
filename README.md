[![Build Status](https://secure.travis-ci.org/3scale/3scale_ws_api_for_dotnet.png?branch=master)](http://travis-ci.org/3scale/3scale_ws_api_for_dotnet)

3scale is an API Infrastructure service which handles API Keys, Rate Limiting, Analytics, Billing Payments and Developer Management. Includes a configurable API dashboard and developer portal CMS. More product stuff at http://www.3scale.net/, support information at http://support.3scale.net/.

Plugin Versions
===============

?

Synopsis
========

This plugin supports the 3 main calls to the 3scale backend:

- *authrep* grants access to your API and reports the traffic on it in one call.
- *authorize* grants access to your API.
- *report* reports traffic on your API.


Install
=======

1. Obtain the library

Get the CS_threescale.dll (Windows version)

It is highly suggested to rebuild the dll from the source using Mono or Microsoft Visual Studio

2. 3scale library depends on:

```csharp
using System;
using System.Collections.Generic;
using CS_threescale;
using System.Text;
using System.Net
```

Usage on App Id auth mode
=========================

On App Id mode you call *authrep* to: grant access to your API, and also report the traffic on it at the same time.

```csharp
// import the 3scale library into your code
using CS_threescale;

// ... somewhere inside your code

// create the API object
IApi _3ScaleAPI = new Api("your_provider_key");

try
{
    // The 'preferred way of calling the backend: authrep'
    
    // The response will be the object AuthorizeResponse or an exception    
    AuthorizeResponse resp = _3ScaleAPI.authorize("your_app_id");

    // You can also use:
    // AuthorizeResponse resp = _3ScaleAPI.authorize("your_app_id","your_app_key");
}
catch (ApiException e)
{
    Console.WriteLine("Exception: " + e.ToString());
}
```

The object AuthorizeReponse can be printed with this function:

```csharp
static void print(AuthorizeResponse resp) 
{
    if (resp.authorized) 
    {
        Console.WriteLine("Authorized!!");
    }
    else 
    {
        Console.WriteLine("NOT Authorized!!" + resp.reason);
    }

    Console.WriteLine("PLAN: " + resp.plan);

    int i=0;
    foreach( UsageItem item in resp.usages)
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
```

You can also call *report* to report traffic on your API: 

```csharp
// Create a Hashtable containing a list of transactions to be reported on, e.g:
System.Collections.Hashtable transactions = new System.Collections.Hashtable();
System.Collections.Hashtable transaction = null;
System.Collections.Hashtable usage = null;

// create a transaction 
transaction = new System.Collections.Hashtable();

// Add the app_id:
string app_id = "your_app_id";
transaction.Add("app_id",app_id);

// Timestamp is optional, if left undefined it will take the current time. If defined
// it must have the format "yyyy-MM-dd HH:mm:ss K"
// transaction.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss K"));

// Usage is a Hashtable that contains the metrics to be reported
usage = new System.Collections.Hashtable();
usage.Add("hits", 10);
transaction.Add("usage",usage);

// Adding the transaction to transactions
transactions.Add("0", transaction);

// You can create a second transaction and have both be reported in a single _3ScaleAPI.report()
transaction = new System.Collections.Hashtable();
transaction.Add("app_id", app_id);
transaction.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss K"));
usage = new System.Collections.Hashtable();
usage.Add("hits", 1);
transaction.Add("usage", usage);
transactions.Add("1", transaction);

// Call report on the backend with the list of transactions to be reported on
_3ScaleAPI.report(transactions);
``` 

Usage on API Key auth mode
==========================

On API Key mode you call *authrep* to: grant access to your API, and also report the traffic on it at the same time.

```csharp
// import the 3scale library into your code
using CS_threescale;

// ... somewhere inside your code

// create the API object
IApi _3ScaleAPI = new Api("your_provider_key");

try
{
    // The 'preferred way of calling the backend: authrep'
    
    // The response will be the object AuthorizeResponse or an exception    
    AuthorizeResponse resp = _3ScaleAPI.authorize_user_key("your_user_key");
}
catch (ApiException e)
{
    Console.WriteLine("Exception: " + e.ToString());
}
```

The object AuthorizeReponse can be printed with this function:

```csharp
static void print(AuthorizeResponse resp) 
{
    if (resp.authorized) 
    {
        Console.WriteLine("Authorized!!");
    }
    else 
    {
        Console.WriteLine("NOT Authorized!!" + resp.reason);
    }

    Console.WriteLine("PLAN: " + resp.plan);

    int i=0;
    foreach( UsageItem item in resp.usages)
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
```


You can also call *report* to report traffic on your API: 

```csharp
// Create a Hashtable containing a list of transactions to be reported on, e.g:
System.Collections.Hashtable transactions = new System.Collections.Hashtable();
System.Collections.Hashtable transaction = null;
System.Collections.Hashtable usage = null;

// create a transaction
transaction = new System.Collections.Hashtable();

// Add the user key
string user_key = "your_user_key";
transaction.Add("user_key",user_key);

// Timestamp is optional, if left undefined it will take the current time. If defined
// it must have the format "yyyy-MM-dd HH:mm:ss K"
// transaction.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss K"));

// Usage is a Hashtable that contains the metrics to be reported
usage = new System.Collections.Hashtable();
usage.Add("hits", 10);
transaction.Add("usage",usage);

// Adding the transaction to transactions
transactions.Add("0", transaction);


// You can create a second transaction and have both be reported in a single _3ScaleAPI.report()
transaction = new System.Collections.Hashtable();
transaction.Add("user_key", user_key);
transaction.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss K"));
usage = new System.Collections.Hashtable();
usage.Add("hits", 1);
transaction.Add("usage", usage);
transactions.Add("1", transaction);

// Call report on the backend with the list of transactions to be reported on
_3ScaleAPI.report(transactions);
``` 

Usage on OAuth auth mode
==========================

?

To test
=======

?

Legal
=====

Copyright (c) 2008 3scale networks S.L., released under the MIT license.



