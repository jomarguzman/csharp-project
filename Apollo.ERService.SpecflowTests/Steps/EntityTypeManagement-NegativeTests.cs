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
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Apollo.ERService.SpecflowTests.Steps
{
    [Binding]
    class EntityTypeManagement_NegativeTests
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public EntityTypeManagement_NegativeTests(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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

        [When(@"the IMS User gets all entities of specific entity type that doesn't exist in the ER Collection")]
        public async Task WhenTheIMSUserGetsAllEntitiesOfSpecificEntityTypeWhichDoesnotContainEntitiesInAnERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                     new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get all entities of specific entity type from ER Collection {erCollectionIdList[i]}");
                var getAllEntitiesResponseMsg = await ERService.GetAllEntitiesByEntityType(BrickEntityTypes.FloorBrickEntityType, headers: headers);
                scenarioContext.AddHttpResponse(getAllEntitiesResponseMsg);
              
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" entities of the entity type with ""(.*)"" Ok status code from the ER Collection")]
        public void ThenValidateTheIMSUserGetsEntitiesOfASpecificEntityTypeWithOkStatusCodeFromTheERCollection(int expectedFetchCount, HttpStatusCode responseCode)
        {
            
            var getAllResponses = scenarioContext.AllHttpResponses().ToList();

            for (int j = 0; j < getAllResponses.Count; j++)
            {
                getAllResponses[j].Response.StatusCode.Should().Be(responseCode, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to get all entities of entity type which does not have any Entity");
                var getAllEntities = getAllResponses[j].Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Count().Should().Be(expectedFetchCount);
            }

        }

    }
}
