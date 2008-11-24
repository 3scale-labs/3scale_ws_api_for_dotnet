
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
using System.Text;
using System.Collections;
using System.Net;


3) Examples of usage:

3.1) Initialize

_3ScaleAPI = new Api(3SCALE_SERVER,PROVIDER_KEY);

3.2) Add usage costs

Hashtable costs = new Hashtable();
costs.Add("hits", "1");
costs.Add("storage", "20480");

3.3) Start transaction

TransactionData tdata = _3ScaleAPI.Start(USER_KEY, costs);
or
TransactionData tdata = _3ScaleAPI.Start(USER_KEY);

TransactionData is an object with three getters:

ID: String tdata.ID
Contract name: String tdata.ContractName;
Provider verification key: String tdata.ProviderVerification

3.4) Confirm transaction

_3ScaleAPI.Confirm(tdata.ID);
or 
_3ScaleAPI.Confirm(tdata.ID,costs);

3.5) Cancel transaction

_3ScaleAPI.Cancel(tdata.ID);

3.6) Upon errors the API will return exceptions as ApiException

Console.WriteLine("Error occured : " + err.GetType().Name + "\n");
Console.WriteLine("Error type : " + err.ErrorReturn.ID + "\n");
Console.WriteLine("Description : " + err.ErrorReturn.ServerMessage + "\n");
