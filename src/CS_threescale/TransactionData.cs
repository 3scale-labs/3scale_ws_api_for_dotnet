using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using CS_threescale;

namespace CS_threescale
{
    [XmlRoot("transaction")]
    public class TransactionData
    {
        public TransactionData() { }

        public TransactionData(string xmlResponse)
        {
            TransactionData tsc =
                SerializeHelper<TransactionData>.Ressurect(xmlResponse);
            this.id = tsc.id;
            this.contract_name = tsc.contract_name;
            this.provider_verification = tsc.provider_verification;
        }

        
        private string id;
        [XmlElement("id")]
        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        
        private string contract_name;
        [XmlElement("contract_name")]
        public string ContractName
        {
            get { return contract_name; }
            set { contract_name = value; }
        }
        
        private string provider_verification;
        [XmlElement("provider_verification_key")]
        public string ProviderVerification
        {
            get { return provider_verification; }
            set { provider_verification = value; }
        }

        
    }
}
