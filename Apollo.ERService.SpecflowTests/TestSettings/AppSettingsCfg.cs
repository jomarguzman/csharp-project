/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using System.Configuration;

namespace Apollo.ERService.SpecflowTests.TestSettings
{
    public class AppSettingsCfg : ITestConfiguration
    {
        public string ImsEndpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["ImsEndpoint"];
            }
        }
        public string ImsClientId
        {
            get
            {
                return ConfigurationManager.AppSettings["ImsClientId"];
            }
        }
        public string ImsClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["ImsClientSecret"];
            }
        }
        public string PrimaryUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["PrimaryUserName"];
            }
        }
        public string PrimaryUserPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["PrimaryUserPassword"];
            }
        }
        public string SecondaryUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["SecondaryUsername"];
            }
        }
        public string SecondaryPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SecondaryPassword"];
            }
        }
        public string InvalidImsEndpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["InvalidImsEndpoint"];
            }
        }
        public string InvalidImsClientSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["InvalidImsClientSecret"];
            }
        }
        public string ImsScopes
        {
            get
            {
                return ConfigurationManager.AppSettings["ImsScopes"];
            }
        }
        public string ACLEndpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["ACLEndpoint"];
            }
        }
        public string ERApiEndpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["ERApiEndpoint"];
            }
        }
        public string GraphApiEndpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["GraphApiEndpoint"];
            }
        }
        public string FilesApiEndpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["FilesApiEndpoint"];
            }
        }
    }
}