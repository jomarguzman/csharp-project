/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Clients;
using Apollo.ERService.SpecflowTests.TestSettings;
using System;
using TechTalk.SpecFlow;
using Jci.Be.Data.Identity.HttpClient;
using Serilog;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apollo.ERService.SpecflowTests.Steps
{
    [Binding]
    public class ERStatusManagement
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public ERStatusManagement(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext ?? throw new ArgumentNullException("ScenarioContext is null");
            _testCfg = testConfiguration ?? throw new Exception("Something went wrong with Test Setup. The setup provides a parallel safe version of test configuration settings.");
        }

        private ERServiceClient ERService
        {
            get
            {
                if (!scenarioContext.TryGetValue(restClientUser, out IRestClient user))
                {
                    restClientUser = "PrimaryIMSUser";
                }
                var erServiceClient = new ERServiceClient(scenarioContext.Get<IRestClient>(restClientUser));
                return erServiceClient;
            }
        }

        [Then(@"the IMS User gets ER Status")]
        public async Task ThenTheIMSUserGetsERStatus()
        {
            restClientUser = "PrimaryIMSUser";
            Log.Information($"Attempting to get API status");
            var getApiStatusResponseMsg = await ERService.GetApiStatus();
            getApiStatusResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"user should be able to get api status");
            var response = getApiStatusResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            JObject o = JObject.Parse(response);
            var apiName = (string)(o.SelectToken("api"));
            scenarioContext.Set(apiName, "ApiName");
            var version = (string)(o.SelectToken("version"));
            scenarioContext.Set(version, "version");
        }

        [Then(@"validate the status message points to ""(.*)"" with a version number")]
        public void ThenValidateTheStatusMessagePointsToWithAVersionNumber(string apiName)
        {
            var getApiName = scenarioContext.Get<string>("ApiName");
            var version = scenarioContext.Get<string>("version");
            Assert.AreEqual(apiName, getApiName);
            Assert.IsNotNull(version);
        }
    }
}
