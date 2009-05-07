using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CS_threescale
{
    public interface IApi
    {
        TransactionData Start(string user_contract_key, Hashtable metrics);
        TransactionData Start(string user_contract_key);
        void Confirm(string transaction_id, Hashtable metrics);
        void Confirm(string transaction_id);
        bool Cancel(string transaction_id);

        string HostURI
        {
            get;set;
        }
    }
}
