[![Build Status](https://secure.travis-ci.org/3scale/3scale_ws_api_for_dotnet.png?branch=master)](http://travis-ci.org/3scale/3scale_ws_api_for_dotnet)

3scale is an API Infrastructure service which handles API Keys, Rate Limiting, Analytics, Billing Payments and Developer Management. Includes a configurable API dashboard and developer portal CMS. More product stuff at http://www.3scale.net/, support information at http://support.3scale.net/.

Plugin Versions
===============

This is version 2.0.0 of the plugin, if you were using this plugin before October 1st 2013, you are using the old [version 0.1.3](https://github.com/3scale/3scale_ws_api_for_dotnet/releases/tag/v0.1.3) of it, but we strongly recommend you to port your code to this new version which contains more features.

Synopsis
========

This plugin supports the 3 main calls to the 3scale backend:

- *authrep* grants access to your API and reports the traffic on it in one call.
- *authorize* grants access to your API.
- *report* reports traffic on your API.

3scale supports 3 authentication modes: App Id, User Key and OAuth. The first two are similar on their calls to the backend, they support authrep. OAuth differs in its usage two calls are required: first authorize then report.

Install
=======

1. Obtain the library: CS_threescale.dll (Windows version). It is highly suggested to rebuild the dll from the source using Mono or Microsoft Visual Studio
2. 3scale library depends on:

```csharp
using System;
using System.Collections.Generic;
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

// build a hashtable of parameters
Hashtable parameters = new Hashtable();

parameters.Add("app_id", "your_app_id_");
// You can also add the app_key if required...
// parameters.Add("app_key", "your_app_key");
// ...and the service id
// parameters.Add("service_id", "your_application_service_id");

//Add a metric to the call
Hashtable usage = new Hashtable();
usage.Add("hits", "1");
parameters.Add("usage",usage);

try
{
    // The preferred way of calling the backend: authrep
    // The response will be the object AuthorizeResponse or an exception    
    AuthorizeResponse resp = _3ScaleAPI.authrep(parameters);
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

// build a hashtable of parameters
Hashtable parameters = new Hashtable();

parameters.Add("user_key", "your_user_key");
// parameters.Add("service_id", "your_user_key_service_id");

// Add a metric to the call
Hashtable usage = new Hashtable();
usage.Add("hits", "1");
parameters.Add("usage",usage);

try
{
    // The preferred way of calling the backend: authrep
    // The response will be the object AuthorizeResponse or an exception    
    AuthorizeResponse resp = _3ScaleAPI.authrep(parameters);
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

On OAuth you have to make two calls, first _authorize_ to grant naccess to your API and then _report_ the traffic on it.

```csharp
// import the 3scale library into your code
using CS_threescale;

// ... somewhere inside your code

// create the API object
IApi _3ScaleAPI = new Api("your_provider_key");

// build a hashtable of parameters
Hashtable parameters = new Hashtable();

parameters.Add("app_id", "your_oauth_app_id");
// You can also add the service id
// parameters.Add("service_id", "your_oauth_service_id");

//Add a metric to the call
Hashtable usage = new Hashtable();
usage.Add("hits", "1");
parameters.Add("usage",usage);

try
{
    AuthorizeResponse resp = _3ScaleAPI.oauth_authorize(parameters);
	
	if(resp.authorized)
	{
		// you can get the client secret like so
		string clientsecret = resp.GetClientSecret();
		
		//  now do a report
		Hashtable transaction = new Hashtable();
		transaction.Add("app_id", "your_oauth_app_id");
		
		Hashtable transaction_usage = new Hashtable();
		transaction_usage.Add("hits","1");
		transaction.Add("usage", transaction_usage);
		
		try
		{
			// Call report on the backend with the list of transactions to be reported on
			_3ScaleAPI.report(transaction);
		}
		catch(ApiException e)
		{
			Console.WriteLine("Exception on report:" + e.ToString());
		}
	}
}
catch (ApiException e)
{
    Console.WriteLine("Exception: " + e.ToString());
}
```

To test
=======

To test the plugin with your real data:

- Open up the [ConsoleDemo](https://github.com/3scale/3scale_ws_api_for_dotnet/tree/master/src/ConsoleDemo) project 
- Edit Main function in [Program](https://github.com/3scale/3scale_ws_api_for_dotnet/blob/master/src/ConsoleDemo/Program.cs) with your provider key and app id (or user_key if you are using that instead.)
- Run Program.cs

Legal
=====

Copyright (c) 2008 3scale networks S.L., released under the MIT license.



