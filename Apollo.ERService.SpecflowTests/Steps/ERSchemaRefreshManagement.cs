/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Clients;
using Apollo.ERService.SpecflowTests.Models;
using Apollo.ERService.SpecflowTests.TestSettings;
using FluentAssertions;
using Jci.Be.Data.Identity.HttpClient;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Apollo.ERService.SpecflowTests.Steps
{
    [Binding]
    class ERSchemaRefreshManagement
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;
        private readonly List<RestClientResponse<PlatformItemResponse>> refreshSchemaList;

        public ERSchemaRefreshManagement(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext ?? throw new ArgumentNullException("ScenarioContext is null");
            _testCfg = testConfiguration ?? throw new Exception("Something went wrong with Test Setup. The setup provides a parallel safe version of test configuration settings.");
            refreshSchemaList = new List<RestClientResponse<PlatformItemResponse>>();
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

        [When(@"IMS User refreshes ""(.*)"" entity type schema")]
        public async Task WhenIMSUserRefreshTheEntitySchema(string entityType)
        {
            restClientUser = "PrimaryIMSUser";
            Log.Information($"Attempting to refresh entity schema");
            var entitySchemaRefreshResponseMsg = await ERService.RefreshEntitySchema(entityType);
            refreshSchemaList.Add(entitySchemaRefreshResponseMsg);
        }

        [When(@"IMS User refreshes ""(.*)"" relationship type schema")]
        public async Task WhenIMSUserRefreshTheRelationshipSchema(string relationshipType)
        {
            restClientUser = "PrimaryIMSUser";
            Log.Information($"Attempting to refresh relationship schema");
            var relationshipSchemaRefreshResponseMsg = await ERService.RefreshRelationshipSchema(relationshipType);
            refreshSchemaList.Add(relationshipSchemaRefreshResponseMsg);
        }

        [Then(@"validate the IMS User gets (.*) Ok status code for the refreshed schemas")]
        public void ThenValidateTheIMSUserGetsOkStatusCodeForRefreshingSchema(HttpStatusCode statusCode)
        {
            for (int i = 0; i < refreshSchemaList.Count; i++)
            {
                refreshSchemaList[i].Response.StatusCode.Should().Be(HttpStatusCode.OK, $"user with {_testCfg.ImsScopes} scopes should be able to refresh entity and relationship schemas");
                var response = JObject.Parse(refreshSchemaList[i].Response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                response.SelectToken("status.message").ToString().Should().Be("Cache refreshed.");
            }
        }
    }
}
