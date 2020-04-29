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
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class EntityTypeManagement
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public EntityTypeManagement(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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
        [Then(@"validate the IMS User gets all ""(.*)"" versions of a specific entity by entity type from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllVersionsOfASpecificEntityByEntityTypeFromTheERCollection(int versionCount)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
            };

                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to get all versions of a specific entity by entity type from the ER Collection {erCollectionIdList[i]}");
                    var entityType = getNewEntities[j].EntityType.ToString();
                    var entityId = getNewEntities[j].Id;
                    var getAllVersionsOfEntityResponseMsg = await ERService.GetAllVersionsOfEntityByEntityType(entityType, entityId, headers);
                    getAllVersionsOfEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of a specific entity by entity type from er collection {erCollectionIdList[i]}");
                    var getAllEntities = getAllVersionsOfEntityResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                    getAllEntities.Count().Should().Be(versionCount);
                   
                                
                    var responseVersionCount = getAllEntities.Count();
                    //removed 1 to versionCount as after bug fix the response does not contain orginal entity in addition to versioned entities
                    responseVersionCount.Should().Be(versionCount);

                    var response = JObject.Parse(getAllVersionsOfEntityResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data");

                    for (int k = 0; k < responseVersionCount; k++)
                    {
                        var id = response[k]["id"];
                        id.ToString().Should().Contain(entityId);
                    }
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" versions of a specific entity by entity type and ""(.*)"" when page size is ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsVersionsOfASpecificEntityByEntityTypeAndWhenPageSizeIs(int versionCount, string continuationToken, string pageSize)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>("PageSize", pageSize)
                };

                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to get all versions of a specific entity by entity type from the ER Collection {erCollectionIdList[i]}");
                    var entityType = getNewEntities[j].EntityType.ToString();
                    var entityId = getNewEntities[j].Id;
                    var getAllVersionsOfEntityResponseMsg = await ERService.GetAllVersionsOfEntityByEntityType(entityType, entityId, headers);
                    getAllVersionsOfEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of a specific entity by entity type from er collection {erCollectionIdList[i]}");
                    var getAllEntities = getAllVersionsOfEntityResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                    getAllEntities.Count().Should().Be(versionCount);

                    var response = getAllVersionsOfEntityResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    continuationToken = (string)o.SelectToken("paging.continuationToken");
                    scenarioContext.Set(continuationToken, "Page_ContinuationToken");

                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" versions of a specific entity by entity type using the ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsVersionsOfASpecificEntityByEntityTypeUsingThe(int versionCount, string continuationToken)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var pageContinuationToken = scenarioContext.Get<string>("Page_ContinuationToken");

                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                new KeyValuePair<string, string>(continuationToken, pageContinuationToken)
            };

                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to get all versions of a specific entity by entity type from the ER Collection {erCollectionIdList[i]}");
                    var entityType = getNewEntities[j].EntityType.ToString();
                    var entityId = getNewEntities[j].Id;
                    var getAllVersionsOfEntityResponseMsg = await ERService.GetAllVersionsOfEntityByEntityType(entityType, entityId, headers);
                    getAllVersionsOfEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of a specific entity by entity type from er collection {erCollectionIdList[i]}");
                    var getAllEntities = getAllVersionsOfEntityResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                    getAllEntities.Count().Should().Be(versionCount);

                    var response = getAllVersionsOfEntityResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    continuationToken = (string)o.SelectToken("paging.continuationToken");
                    Assert.AreEqual(continuationToken, null);
                }
            }
        }

        [Then(@"validate the IMS User gets all entities of specific entity type from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllEntitiesOfSpecificEntityTypeFromTheERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
            };

                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to get all entities of specific entity type from ER Collection {erCollectionIdList[i]}");
                    var entityType = getNewEntities[j].EntityType.ToString();
                    var getAllEntitiesResponseMsg = await ERService.GetAllEntitiesByEntityType(entityType, headers: headers);
                    getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities of specific entity type from er collection {erCollectionIdList[i]}");

                    var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                    getAllEntities.Should().BeEquivalentTo(getNewEntities);
                    getAllEntities.Count().Should().Be(getNewEntities.Count());
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" entity and ""(.*)"" when page size is ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsEntityAndWhenPageSizeIs(int entityCount, string continuationToken, string pageSize)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                new KeyValuePair<string, string>("PageSize", pageSize)
            };
                Log.Information($"Attempting to get specific entity by entity type from the ER Collection {erCollectionIdList[i]}");
                var entityType = getNewEntities.FirstOrDefault().EntityType.ToString();
                var getAllEntitiesResponseMsg = await ERService.GetAllEntitiesByEntityType(entityType, headers: headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities with Entity Tpe from er collection {erCollectionIdList[i]}");
                var response = getAllEntitiesResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                JObject o = JObject.Parse(response);
                continuationToken = (string)o.SelectToken("paging.continuationToken");
                scenarioContext.Set(continuationToken, "PageContinuationToken");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Count().Should().Be(entityCount);
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" entity using the ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsEntityUsingThe(int entityCount, string continuationToken)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var pageContinuationToken = scenarioContext.Get<string>("PageContinuationToken");

                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                new KeyValuePair<string, string>(continuationToken, pageContinuationToken)
            };

                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to get specific entity by entity type from the ER Collection {erCollectionIdList[i]}");
                    var entityType = getNewEntities[j].EntityType.ToString();
                    var getAllEntitiesResponseMsg = await ERService.GetAllEntitiesByEntityType(entityType, headers: headers);
                    getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities with Entity Tpe from er collection {erCollectionIdList[i]}");
                    var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                    getAllEntities.Count().Should().Be(entityCount);
                }
            }
        }

        [Then(@"validate the IMS User gets all entities of a specific type with system data when includeSystemData is ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsAllEntitiesOfASpecificTypeWithSystemDataWhenIncludeSystemDataIs(bool includeSystemData)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
            };

                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to get all entities of a specific type with system data from ER Collection {erCollectionIdList[i]}");
                    var entityType = getNewEntities[j].EntityType.ToString();
                    var getAllEntitiesResponseMsg = await ERService.GetAllEntitiesByEntityType(entityType, includeSystemData, headers);
                    getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities of a specific type with system data from er collection {erCollectionIdList[i]}");
                    var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                    getAllEntities.Should().BeEquivalentTo(getNewEntities);
                    getAllEntities.Count().Should().Be(getNewEntities.Count());
                    var response = getAllEntitiesResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    o.SelectToken("data").First["sysData"].Should().NotBeEmpty();
                }
            }
        }
    }
}
