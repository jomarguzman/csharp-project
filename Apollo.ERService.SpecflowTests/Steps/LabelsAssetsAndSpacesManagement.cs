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
using Jci.Be.Data.Apollo.Core.Constants;
using Jci.Be.Data.Identity.HttpClient;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Apollo.ERService.SpecflowTests.Steps
{
    [Binding]
    public class LabelsAssetsAndSpacesManagement
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;
        private readonly char separator = Char.Parse(",");

        public LabelsAssetsAndSpacesManagement(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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

        [When(@"the IMS User posts ""(.*)"" new entities with ""(.*)"" label\(s\) where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewEntitiesWithLabelsWhereIsUsingVInAnERCollectionTimeS(int numOfEntities, int numOfLabels, string asyncHeader, string asyncValue, string version, int numOfERCollections)
        {
            restClientUser = "PrimaryIMSUserER";
            await PostEntitiesWithLabels(numOfEntities, numOfLabels, asyncHeader, asyncValue, version, numOfERCollections: 1); //post in 1 ER Collection
        }

        private async Task PostEntitiesWithLabels(int numOfEntities, int numOfLabels, string asyncHeader, string asyncValue, string version, int numOfERCollections)
        {
            for (int i = 0; i < numOfEntities; i++)
            {
                var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: EntityTypes.EquipmentBrickEntityType, entityName: "test entity")
                {
                    Labels = new List<string>()
                };
                for (int k = 0; k < numOfLabels; k++)
                {
                    newEntity.Labels.Add($"testLabel-{Guid.NewGuid()}");
                }
                scenarioContext.AddEntity(newEntity);
            }
            for (int j = 0; j < numOfERCollections; j++)
            {
                //var erCollectionId = "test-" + Guid.NewGuid().ToString();
                string erCollectionId = this.scenarioContext.Get<dynamic>("CollectionsResponse");
                scenarioContext.AddERCollections(erCollectionId);
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionId),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };
                var getentities = scenarioContext.CreatedEntities().ToList();

                Log.Information($"Attempting to post new entity/entities synchronously in ER collection {erCollectionId}");

                var postEntitiesResponseMsg = await ERService.PostEntities(version, getentities, headers);
                if (asyncValue == "true")
                {
                    postEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
                    Thread.Sleep(10000);
                }
                else
                {
                    postEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
                    var response = postEntitiesResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject jObjResponse = JObject.Parse(response);
                    var successEntityIDList = from entity in jObjResponse["data"]["responses"]["success"]
                                              select (string)entity["id"];
                    var getEntityIDList = from entity in getentities
                                          select entity.Id;

                    getEntityIDList.Should().BeEquivalentTo(successEntityIDList);
                    getEntityIDList.Count().Should().Be(successEntityIDList.Count());
                }
            }
        }

        [Then(@"validate the IMS User gets all entities by specific label from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllEntitiesBySpecificLabelFromTheERCollection()
        {
            restClientUser = "PrimaryIMSUserER";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };
                Log.Information($"Attempting to get all entities by specific label from ER Collection {erCollectionIdList[i]}");
                var label = getNewEntities.FirstOrDefault().Labels.FirstOrDefault().ToString();
                var getEntityByLabelResponseMsg = await ERService.GetEntitiesByLabel(label, headers: headers);
                getEntityByLabelResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities by specific label from er collection {erCollectionIdList[i]}");

                var getAllEntities = getEntityByLabelResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Should().BeEquivalentTo(getNewEntities.FindAll(s => s.Labels.Contains(label)));
                getAllEntities.Count().Should().Be(getNewEntities.FindAll(s => s.Labels.Contains(label)).Count());
            }
        }

        [Then(@"validate the IMS User gets all entities by multiple labels with OR condition from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllEntitiesByMultipleLabelsWithORConditionFromTheERCollection()
        {
            restClientUser = "PrimaryIMSUserER";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };
                Log.Information($"Attempting to get all entities by multiple labels with OR condition from ER Collection {erCollectionIdList[i]}");
                var labels = getNewEntities.Select(e => e.Labels.FirstOrDefault()).ToList();

                var getEntityByLabelResponseMsg = await ERService.GetEntitiesByMultipleLabelsWithORCondition(labels, headers: headers);
                getEntityByLabelResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities by multiple labels qith OR condition from er collection {erCollectionIdList[i]}");

                var getAllEntities = getEntityByLabelResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());

                getAllEntities.Should().BeEquivalentTo(getNewEntities);
                getAllEntities.Count().Should().Be(getNewEntities.Count());
            }
        }

        [Then(@"validate the IMS User gets all entities by multiple labels with AND condition from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllEntitiesByMultipleLabelsWithANDConditionFromTheERCollection()
        {
            restClientUser = "PrimaryIMSUserER";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };
                Log.Information($"Attempting to get all entities by multiple labels with AND condition from ER Collection {erCollectionIdList[i]}");
                var labels = getNewEntities.Select(e => e.Labels).FirstOrDefault(); //take only first entity labels 

                var getEntityByLabelResponseMsg = await ERService.GetEntitiesByMultipleLabelsWithANDCondition(labels, headers: headers);
                getEntityByLabelResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities by multiple labels qith AND condition from er collection {erCollectionIdList[i]}");

                var getAllEntities = getEntityByLabelResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                foreach (var entity in getAllEntities)
                {
                    getNewEntities.Should().Contain(e => e.Id == entity.Id);
                }
                getAllEntities.Count().Should().Be(1); //as we passing only first entity labels
            }
        }

        [When(@"the IMS User posts ""(.*)"" new entities of types ""(.*)"" where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewEntitiesOfTypesWhereIsUsingVInAnERCollectionTimeS(int numOfEntities, List<string> entityTypes, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfEntities; i++)
            {
                var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: entityTypes[i], entityName: "test entity");
                scenarioContext.AddEntity(newEntity);
            }
            //var erCollectionId = "test-" + Guid.NewGuid().ToString();
            string erCollectionId = this.scenarioContext.Get<dynamic>("CollectionsResponse");
            scenarioContext.AddERCollections(erCollectionId);
            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionId),
                new KeyValuePair<string, string>(asyncHeader, asyncValue)
            };
            var getentities = scenarioContext.CreatedEntities().ToList();

            Log.Information($"Attempting to post new entity/entities synchronously in ER collection {erCollectionId}");
            for (int i = 0; i < numOfTimes; i++)
            {
                var postEntitiesResponseMsg = await ERService.PostEntities(version, getentities, headers);
                if (asyncValue == "true")
                {
                    postEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
                    Thread.Sleep(10000);
                }
                else
                {
                    postEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
                    var response = postEntitiesResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject jObjResponse = JObject.Parse(response);
                    var successEntityIDList = from entity in jObjResponse["data"]["responses"]["success"]
                                              select (string)entity["id"];
                    var getEntityIDList = from entity in getentities
                                          select entity.Id;

                    getEntityIDList.Should().BeEquivalentTo(successEntityIDList);
                    getEntityIDList.Count().Should().Be(successEntityIDList.Count());
                }
            }
        }

        [Then(@"validate the IMS User gets the list of Assets within the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsTheListOfAssetsWithinTheERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };
                Log.Information($"Attempting to get the list of Assets from ER Collection {erCollectionIdList[i]}");

                var getAllAssetsResponseMsg = await ERService.GetAllAssets(headers);
                getAllAssetsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get the list of Assets from er collection {erCollectionIdList[i]}");

                var getExpectedEntities = scenarioContext.CreatedEntities().ToList().FindAll(s => s.EntityType.Equals(EntityTypes.EquipmentBrickEntityType));
                var getAllEntities = getAllAssetsResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Should().BeEquivalentTo(getExpectedEntities);
                getAllEntities.Count().Should().Be(getExpectedEntities.Count());
            }
        }

        [Then(@"validate the IMS User gets the list of Spaces within the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsTheListOfSpacesWithinTheERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };
                Log.Information($"Attempting to get list of Spaces from ER Collection {erCollectionIdList[i]}");

                var getAllEntitiesResponseMsg = await ERService.GetAllSpaces(headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get list of Spaces from er collection {erCollectionIdList[i]}");

                var getExpectedEntities = scenarioContext.CreatedEntities().ToList().FindAll(s => s.EntityType.Equals(EntityTypes.BuildingBrickEntityType));
                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Should().BeEquivalentTo(getExpectedEntities);
                getAllEntities.Count().Should().Be(getExpectedEntities.Count());
            }
        }
        

        [StepArgumentTransformation]
        public List<String> TransformToListOfString(string commaSeparatedList)
        {
            return commaSeparatedList.Split(separator).ToList();
        }
    }
}
