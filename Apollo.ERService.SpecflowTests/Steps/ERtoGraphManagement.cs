/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Clients;
using Apollo.ERService.SpecflowTests.Extensions;
using Apollo.ERService.SpecflowTests.Models;
using Apollo.ERService.SpecflowTests.TestSettings;
using FluentAssertions;
using Jci.Be.Data.Apollo.Core.Constants;
using Jci.Be.Data.Identity.HttpClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Apollo.ERService.SpecflowTests.Steps
{
    [Binding]
    public class ERtoGraphManagement
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public ERtoGraphManagement(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext ?? throw new ArgumentNullException("ScenarioContext is null");
            _testCfg = testConfiguration ?? throw new Exception("Something went wrong with Test Setup. The setup provides a parallel safe version of test configuration settings.");
        }

        private GraphServiceClient GraphService
        {
            get
            {
                if (!scenarioContext.TryGetValue(restClientUser, out IRestClient user))
                {
                    restClientUser = "PrimaryIMSUser";
                }
                var graphServiceClient = new GraphServiceClient(scenarioContext.Get<IRestClient>(restClientUser));
                return graphServiceClient;
            }
        }

        [Then(@"validate IMS User deletes the relationship\(s\) from the ER Collection using Graph API")]
        public async Task ThenValidateIMSUserDeletesTheRelationshipSFromTheERCollectionUsingGraphAPI()
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
            };
                Log.Information($"Attempting to get all relationships from ER Collection {erCollectionIdList[i]}");

                var getAllRelationshipsResponseMsg = await GraphService.GetAllRelationship(headers);
                getAllRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all relationships from er collection {erCollectionIdList[i]}");

                var getAllRelationships = getAllRelationshipsResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                getAllRelationships.Count().Should().Be(0);
            }
        }
    }
}
