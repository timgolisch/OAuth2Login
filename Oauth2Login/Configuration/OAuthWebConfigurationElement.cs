﻿using System.Configuration;

namespace Oauth2Login.Configuration
{
    public class OAuthWebConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("acceptedRedirectUrl", IsRequired = false)]
        public string AcceptedRedirectUrl
        {
            get { return base["acceptedRedirectUrl"].ToString(); }
        }

        [ConfigurationProperty("failedRedirectUrl", IsRequired = true)]
        public string FailedRedirectUrl
        {
            get { return base["failedRedirectUrl"].ToString(); }
        }

        [ConfigurationProperty("proxy", IsRequired = false)]
        public string Proxy
        {
            get { return base["proxy"].ToString(); }
        }
    }
}