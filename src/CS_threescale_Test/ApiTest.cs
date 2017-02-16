using System;
using NUnit.Framework;
using CS_threescale;
using System.Collections;

namespace CS_threescale_Test
{
    [TestFixture]
    public class ApiTest
    {
        private IApi m_api;

        private readonly string backend = "http://su1.3scale.net:80";
        private readonly string default_backend = "https://su1.3scale.net:443";

        private readonly string provider_key = Environment.GetEnvironmentVariable ("DOTNET_PLUGIN_NUNIT_PROVIDER_KEY");
        private readonly string app_key = Environment.GetEnvironmentVariable ("DOTNET_PLUGIN_NUNIT_APP_KEY");
        private readonly string app_id = Environment.GetEnvironmentVariable ("DOTNET_PLUGIN_NUNIT_APP_ID");
        private readonly string service_id = Environment.GetEnvironmentVariable ("DOTNET_PLUGIN_NUNIT_SERVICE_ID");
        private readonly string service_token = Environment.GetEnvironmentVariable ("DOTNET_PLUGIN_NUNIT_SERVICE_TOKEN");

        private readonly string invalid_provider_key = "InvalidProviderKey";
        private readonly string invalid_app_id = "InvalidAppId";
        private readonly string invalid_app_key = "InvalidAppKey";
        private readonly string invalid_service_id = "InvalidServiceId";

        [Test]
        public void TestDefaultHost ()
        {
            m_api = new Api ();
            Assert.AreEqual (default_backend, m_api.HostURI);
        }

        [Test]
        public void TestCustomHost ()
        {
            m_api = new Api ("example.com", provider_key);
            Assert.AreEqual ("example.com", m_api.HostURI);
        }

        #region Authrep tests

        [Test]
        public void TestAuthrepWithValidCredentials ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", service_id);

            var response = m_api.authrep (parameters);

            Assert.IsTrue (response.authorized);
        }

        [Test]
        public void TestAuthRepWithServiceToken ()
        {
            m_api = new Api ();

            //Hack to get around tls errors with Mono
            m_api.HostURI = backend;

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", service_id);
            parameters.Add ("service_token", service_token);

            var response = m_api.authrep (parameters);

            Assert.IsTrue (response.authorized);
        }

        [Test]
        public void TestAuthrepWithInvalidProviderKey ()
        {
            m_api = new Api (backend, invalid_provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", service_id);

            var ex = Assert.Throws<ApiException> (() => m_api.authrep (parameters));
            Assert.That (ex.Message, Is.EqualTo ("provider_key_invalid : provider key \"" + invalid_provider_key + "\" is invalid"));
        }

        [Test]
        public void TestAuthrepWithInvalidServiceId ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", invalid_service_id);

            var ex = Assert.Throws<ApiException> (() => m_api.authrep (parameters));
            Assert.That (ex.Message, Is.EqualTo ("service_id_invalid : service id \"" + invalid_service_id + "\" is invalid"));
        }

        [Test]
        public void TestAuthrepWithInvalidAppId ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", "1a2b3c4d5e6f7a8b9c");
            parameters.Add ("app_id", invalid_app_id);
            parameters.Add ("service_id", service_id);

            var ex = Assert.Throws<ApiException> (() => m_api.authrep (parameters));
            Assert.That (ex.Message, Is.EqualTo ("application_not_found : application with id=\"" + invalid_app_id + "\" was not found"));
        }

        [Test]
        public void TestAuthrepWithInvalidAppKey ()
        {
            m_api = new Api (backend, provider_key);

            var parameters = new Hashtable ();

            parameters.Add ("app_id", app_id);
            parameters.Add ("app_key", invalid_app_key);
            parameters.Add ("service_id", service_id);

            var resp = m_api.authrep (parameters);

            Assert.IsFalse (resp.authorized);
            Assert.AreEqual (resp.reason, "application key \"InvalidAppKey\" is invalid");
        }

        [Test]
        public void TestAuthrepWithMissingAppKey ()
        {
            m_api = new Api (backend, provider_key);

            var parameters = new Hashtable ();

            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", service_id);

            var resp = m_api.authrep (parameters);

            Assert.IsFalse (resp.authorized);
            Assert.AreEqual (resp.reason, "application key is missing");
        }

        [Test]
        public void TestAuthRepWithInvalidMetric ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", service_id);

            Hashtable metrics = new Hashtable ();
            metrics.Add ("invalid_metric", "1");
            parameters.Add ("usage", metrics);

            var ex = Assert.Throws<ApiException> (() => m_api.authrep (parameters));
            Assert.That (ex.Message, Is.EqualTo ("metric_invalid : metric \"invalid_metric\" is invalid"));
        }

        #endregion

        #region Authorize tests

        [Test]
        public void TestAuthorizeWithValidCredentials ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", service_id);

            var response = m_api.authorize (parameters);

            Assert.IsTrue (response.authorized);
        }

        public void TestAuthorizeWithServiceToken ()
        {
            m_api = new Api ();

            //Hack to get around tls errors with Mono
            m_api.HostURI = backend;

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", service_id);
            parameters.Add ("service_token", service_token);

            var response = m_api.authorize (parameters);

            Assert.IsTrue (response.authorized);
        }


        [Test]
        public void TestAuthorizeWithInvalidProviderKey ()
        {
            m_api = new Api (backend, invalid_provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", service_id);

            var ex = Assert.Throws<ApiException> (() => m_api.authorize (parameters));
            Assert.That (ex.Message, Is.EqualTo ("provider_key_invalid : provider key \"" + invalid_provider_key + "\" is invalid"));
        }

        [Test]
        public void TestAuthorizeWithInvalidServiceId ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", invalid_service_id);

            var ex = Assert.Throws<ApiException> (() => m_api.authorize (parameters));
            Assert.That (ex.Message, Is.EqualTo ("service_id_invalid : service id \"" + invalid_service_id + "\" is invalid"));
        }

        [Test]
        public void TestAuthorizeWithInvalidAppId ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", "1a2b3c4d5e6f7a8b9c");
            parameters.Add ("app_id", invalid_app_id);
            parameters.Add ("service_id", service_id);

            var ex = Assert.Throws<ApiException> (() => m_api.authorize (parameters));
            Assert.That (ex.Message, Is.EqualTo ("application_not_found : application with id=\"" + invalid_app_id + "\" was not found"));
        }

        [Test]
        public void TestAuthorizeResponsePlan ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", service_id);

            var resp = m_api.authorize (parameters);

            Assert.AreEqual ("Default", resp.plan);
        }

        [Test]
        public void TestAuthorizeResponseUsageReport ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable parameters = new Hashtable ();

            parameters.Add ("app_key", app_key);
            parameters.Add ("app_id", app_id);
            parameters.Add ("service_id", service_id);

            var response = m_api.authorize (parameters);
            var metric = ((UsageItem)response.usages [0]).metric;

            Assert.AreEqual ("hits", metric);
        }

        #endregion

        #region Report Test

        public void TestReportWithServiceToken ()
        {
            m_api = new Api ();

            bool exception = false;

            //Hack to get around tls errors with Mono
            m_api.HostURI = backend;

            Hashtable transactions = new Hashtable ();
            Hashtable transaction = null;
            transaction = new Hashtable ();
            transaction.Add ("app_id", app_id);
            transaction.Add ("app_key", app_key);
            transaction.Add ("service_id", service_id);
            transaction.Add ("service_token", service_token);

            Hashtable usage = new Hashtable ();
            usage.Add ("hits", "1");
            transaction.Add ("usage", usage);
            transactions.Add ("0", transaction);


            try {
                m_api.report (transactions);
            } catch (Exception) {
                exception = true;
            }

            Assert.False (exception);
        }

        [Test]
        public void TestReportWithInvalidProviderKey ()
        {
            m_api = new Api (backend, invalid_provider_key);

            Hashtable transactions = new Hashtable ();
            Hashtable transaction = null;
            transaction = new Hashtable ();
            transaction.Add ("app_id", app_id);
            transaction.Add ("app_key", app_key);
            transaction.Add ("service_id", service_id);

            Hashtable usage = new Hashtable ();
            usage.Add ("hits", "1");
            transaction.Add ("usage", usage);
            transactions.Add ("0", transaction);

            var ex = Assert.Throws<ApiException> (() => m_api.report (transactions));
            Assert.That (ex.Message, Is.EqualTo ("provider_key_invalid : provider key \"" + invalid_provider_key + "\" is invalid"));
        }

        [Test]
        public void TestReportWithNoTransactions ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable transactions = new Hashtable ();

            var ex = Assert.Throws<ApiException> (() => m_api.report (transactions));
            Assert.That (ex.Message, Is.EqualTo ("argument error: undefined transactions, must be at least one"));
        }

        [Test]
        public void TestReportWithEmptyTransaction ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable transactions = new Hashtable ();
            Hashtable transaction = new Hashtable ();
            transactions.Add ("0", transaction);

            var ex = Assert.Throws<ApiException> (() => m_api.report (transactions));
            Assert.That (ex.Message, Is.EqualTo ("Bad request"));
        }

        [Test]
        public void TestReportWithNoUsage ()
        {
            m_api = new Api (backend, provider_key);

            Hashtable transactions = new Hashtable ();
            Hashtable transaction = new Hashtable ();
            transaction.Add ("app_id", app_id);
            transaction.Add ("app_key", app_key);
            transaction.Add ("service_id", service_id);

            Hashtable usage = new Hashtable ();
            transaction.Add ("usage", usage);

            transactions.Add ("0", transaction);


            var ex = Assert.Throws<ApiException> (() => m_api.report (transactions));
            Assert.That (ex.Message, Is.EqualTo ("argument error: undefined transaction, usage is missing in one record"));
        }

        #endregion
    }
}

