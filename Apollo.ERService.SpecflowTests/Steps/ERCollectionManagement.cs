/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Clients;
using Apollo.ERService.SpecflowTests.Constants;
using Apollo.ERService.SpecflowTests.Extensions;
using Apollo.ERService.SpecflowTests.Models;
using Apollo.ERService.SpecflowTests.TestSettings;
using FluentAssertions;
using Jci.Be.Data.Identity.HttpClient;
using Newtonsoft.Json.Linq;
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
    public class ERCollectionManagement
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public ERCollectionManagement(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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

        [When(@"the IMS User copies entities and relationships from this ER Collection to another ER Collection")]
        public async Task ThenValidateTheIMSUserCopiesEntitiesAndRelationshipsFromSourceERCollectionToDestinationERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            var sourceERCollectionId = "";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                sourceERCollectionId = erCollectionIdList[i];
            }
           // var destinationERCollectionId = "destination-" + Guid.NewGuid().ToString();

            string destinationERCollectionId = this.scenarioContext.Get<dynamic>("CollectionsResponse");
            scenarioContext.AddERCollections(destinationERCollectionId);
            var copyErCollectionsList = new List<CopyERCollections>{
                new CopyERCollections{
                    SourceERCollectionID = sourceERCollectionId,
                    DestinationERCollectionID = destinationERCollectionId
                }
            };

            Log.Information($"Attempting to copy entities and relationships from source ER collection {sourceERCollectionId} to destination ER collection{destinationERCollectionId}");
            var postEntitiesResponseMsg = await ERService.CopyERCollections(copyErCollectionsList);
            var response = JObject.Parse(postEntitiesResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data");
            postEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection");
        }

        [Then(@"validate the IMS User gets stats by ER Collection")]
        public async Task ThenValidateTheIMSUserGetsStatsByERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                Log.Information($"Attempting to get stats by ER Collection {erCollectionIdList[i]}");
                var getStatsByerCollectionResponseMsg = await ERService.GetStatsByERCollection(erCollectionIdList[i]);
                getStatsByerCollectionResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get stats by er collection {erCollectionIdList[i]}");

                var ERSummaryResponse = JObject.Parse(getStatsByerCollectionResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data").First["Summary"];
                ERSummaryResponse.SelectToken("Entities").ToString().Should().Be(getNewEntities.Count.ToString());
                ERSummaryResponse.SelectToken("Relationships").ToString().Should().Be(getNewRelationships.Count.ToString());
                ERSummaryResponse.SelectToken("Assets").ToString().Should().Be(getNewEntities.FindAll(s => s.EntityType.Equals(EntityTypes.EquipmentBrickEntityType)).Count.ToString());
                ERSummaryResponse.SelectToken("Spaces").ToString().Should().Be(getNewEntities.FindAll(s => s.EntityType.Equals(EntityTypes.BuildingBrickEntityType) || s.EntityType.Equals(EntityTypes.FloorBrickEntityType) || s.EntityType.Equals(EntityTypes.RoomBrickEntityType)).Count.ToString());

                var EntitiesResponse = JObject.Parse(getStatsByerCollectionResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data").First["Entities"];
                for (int j=0; j<getNewEntities.Count(); j++)
                {
                    EntitiesResponse.SelectToken(getNewEntities[j].EntityType).ToString().Should().Be(getNewEntities.FindAll(s => s.EntityType.Equals(getNewEntities[j].EntityType)).Count.ToString());
                }

                var RelationshipsResponse = JObject.Parse(getStatsByerCollectionResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data").First["Relationships"];
                for (int j = 0; j < getNewRelationships.Count(); j++)
                {
                    RelationshipsResponse.SelectToken(getNewRelationships[j].RelationshipType).ToString().Should().Be(getNewRelationships.FindAll(s => s.RelationshipType.Equals(getNewRelationships[j].RelationshipType)).Count.ToString());
                }
            }
        }
    }
}
