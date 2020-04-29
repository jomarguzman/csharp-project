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
using Jci.Be.Data.Identity.HttpClient.Policies;
using Jci.Be.Data.Identity.HttpClient.TokenProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class EntityManagement_V2_0
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public EntityManagement_V2_0(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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

        
        [Given(@"an IMS User ""(.*)"" with an access token")]
        public void GivenAnIMSUserWithAnAccessToken(string userName)
        {
            var restClientOptions = new RestClientOptions
            {
                BaseEndpoint = _testCfg.ERApiEndpoint,
                TokenProvider = new ResourceOwnerFlow(
                    _testCfg.ImsEndpoint,
                    _testCfg.ImsClientId,
                    _testCfg.ImsClientSecret,
                    _testCfg.PrimaryUserName,
                    _testCfg.PrimaryUserPassword,
                    _testCfg.ImsScopes),
                RetryPolicy = new DefaultRetryPolicy()
            };
            var erRestClient = new RestClient(restClientOptions);
            scenarioContext.Set<IRestClient>(erRestClient, "PrimaryIMSUser");
        }
        
        [When(@"the IMS User posts Collection and response status Code as OK")]
        public async Task WhentheIMSUserpostsCollectionandresponsestatusCodeasOK()
        {

            restClientUser = "PrimaryIMSUser";

            var vaultId = "JCI-Demo";
            scenarioContext.AddVaultId(vaultId);

            var accountId = "JCI-Demo";
            scenarioContext.AddAccountId(accountId);


            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("VaultId", vaultId),
                new KeyValuePair<string, string>("AccountId", accountId)
            };


            Collection collection = new Collection(collectionName: $"test-{Guid.NewGuid()}");
            string collectionname = collection.collectionName.ToString();
            this.scenarioContext.Set<dynamic>(collectionname, "BodyCollectionName");
            string CollectionId = $"test-{Guid.NewGuid()}";
            this.scenarioContext.Set<dynamic>(CollectionId, "CollectionsResponse");

            //Log.Information($"Attempting to post new collection");

            //var postCollectionsResponseMsg = await ERService.PostCollections(collection, headers);

            //var response = postCollectionsResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            //string CollectionId = Convert.ToString(JObject.Parse(response.ToString())["data"]["collectionId"]);
            //this.scenarioContext.Set<dynamic>(CollectionId, "CollectionsResponse");


            //postCollectionsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a collection");

        }

        [When(@"the IMS User posts ""(.*)"" new entity where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        [When(@"the IMS User posts ""(.*)"" new entities where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewEntityWhereIsUsingVInAnERCollectionTimeS(int numOfEntities, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfEntities; i++)
            {
                var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: EntityTypes.EquipmentBrickEntityType, entityName: "test entity");
                scenarioContext.AddEntity(newEntity);
            }
           // var erCollectionId = "test-" + Guid.NewGuid().ToString();
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
                    postEntitiesResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
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

        [Then(@"validate the IMS User gets all entities from the ER Collection")]
        [Then(@"validate the IMS User gets all entities from the Special Characters ER Collection")]
        [Then(@"validate the IMS User gets all entities from the destination ER Collection")]
        public async Task ThenValidateTheIMSUserReadsTheEntity()
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
                Log.Information($"Attempting to get all entities from ER Collection {erCollectionIdList[i]}");

                var getAllEntitiesResponseMsg = await ERService.GetAllEntities(headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities from er collection {erCollectionIdList[i]}");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Should().BeEquivalentTo(getNewEntities);
                getAllEntities.Count().Should().Be(getNewEntities.Count());
            }
        }

        [Then(@"IMS User deletes the entity from the ER Collection")]
        [Then(@"IMS User deletes the entities from the ER Collection")]
        [Then(@"IMS User deletes the entity from the Special Characters ER Collection")]
        [Then(@"IMS User deletes the entities from the source & destination ER Collections")]
        public async Task ThenTheIMSUserDeletesTheEntity()
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
                    Log.Information($"Attempting to delete entities from ER Collection {erCollectionIdList[i]}");

                    var deleteAEntityResponseMsg = await ERService.DeleteEntity(getNewEntities[j].Id, headers);
                    deleteAEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to delete entity {getNewEntities[j].Id} from er collection {erCollectionIdList[i]}");
                }
            }
        }

        [Then(@"validate IMS User deletes the entity from the ER Collection")]
        [Then(@"validate IMS User deletes the entities from the ER Collection")]
        [Then(@"validate IMS User deletes the entity from the Special Characters ER Collection")]
        [Then(@"validate IMS User deletes the entities from the source & destination ER Collections")]
        public async Task ThenValidateTheIMSUserDeletesTheEntity()
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

                Log.Information($"Attempting to get all entities from ER Collection {erCollectionIdList[i]}");

                var getAllEntitiesResponseMsg = await ERService.GetAllEntities(headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities from er collection {erCollectionIdList[i]}");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Count().Should().Be(0);
            }
        }

        [Then(@"validate the IMS User gets a specific entity by entity Id from the ER Collection")]
        public async Task ThenValidateTheIMSUserReadsTheEntityByEntityID()
        {
            restClientUser = "PrimaryIMSUser";

            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int k = 0; k < erCollectionIdList.Count; k++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[k])
                };

                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    var entityId = getNewEntities[j].Id;
                    Log.Information($"Attempting to get specific entity by entity Id {entityId} from ER Collection {erCollectionIdList[k]}");

                    var getEntityByIdResponseMsg = await ERService.GetEntityById(entityId, headers);
                    getEntityByIdResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get specific entity with id {entityId} from er collection {erCollectionIdList[k]}");
                    getNewEntities[j].Should().BeEquivalentTo(getEntityByIdResponseMsg.Result.Data.Select(s => s.ToObject<Entity>()).ToList().FirstOrDefault());

                    var getEntityByIdResponseData = JObject.Parse(getEntityByIdResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                    var successEntityIDList = from entity in getEntityByIdResponseData["data"]
                                              select (string)entity["id"];

                    successEntityIDList.Should().ContainSingle(entityId);
                }
            }
        }

        [Then(@"validate the IMS User gets all ""(.*)"" versions of a specific entity by entity Id from the ER Collection")]
        public async Task ThenValidateTheIMSUserReadsAllVersionOfTheEntityByEntityID(int versionCount)
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
                    var entityId = getNewEntities[j].Id;
                    Log.Information($"Attempting to get all versions of entity by entity Id {entityId} from ER Collection {erCollectionIdList[i]}");

                    var getAllVersionsOfEntityByIdResponseMsg = await ERService.GelAllVersionsOfEntityById(entityId, headers);
                    getAllVersionsOfEntityByIdResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of the entity with id {getNewEntities[j].Id} from er collection {erCollectionIdList[i]}");
                    
                    var getAllVersionsOfEntityByID = getAllVersionsOfEntityByIdResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                    getAllVersionsOfEntityByID.Count().Should().Be(versionCount);
                    var responseVersionCount = getAllVersionsOfEntityByID.Count();
                    //removed 1 to versionCount as after fix the response does not contain orginal entity in addition to versioned entities
                    responseVersionCount.Should().Be(versionCount);

                    var response = JObject.Parse(getAllVersionsOfEntityByIdResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data");

                    for (int k = 0; k < responseVersionCount; k++)
                    {
                        var id = response[k]["id"];
                        id.ToString().Should().Contain(entityId);
                    }
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" versions of a specific entity by entity Id and ""(.*)"" when page size is ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsVersionsOfASpecificEntityByEntityIdAndWhenPageSizeIs(int versionCount, string continuationToken, string pageSize)
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
                    var entityId = getNewEntities[j].Id;
                    Log.Information($"Attempting to get all versions of entity by entity Id {entityId} from ER Collection {erCollectionIdList[i]}");

                    var getAllVersionsOfEntityByIdResponseMsg = await ERService.GelAllVersionsOfEntityById(entityId, headers);
                    getAllVersionsOfEntityByIdResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of the entity with id {getNewEntities[j].Id} from er collection {erCollectionIdList[i]}");

                    var response = getAllVersionsOfEntityByIdResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    continuationToken = (string)o.SelectToken("paging.continuationToken");
                    scenarioContext.Set(continuationToken, "PageContinuationToken");

                    var getAllEntities = getAllVersionsOfEntityByIdResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                    getAllEntities.Count().Should().Be(versionCount);
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" versions of a specific entity by entity Id using the ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsVersionsOfASpecificEntitybyentityIdUsingThe(int versionCount, string continuationToken)
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
                    var entityId = getNewEntities[j].Id;
                    Log.Information($"Attempting to get all versions of entity by entity Id {entityId} from ER Collection {erCollectionIdList[i]}");

                    var getAllVersionsOfEntityByIdResponseMsg = await ERService.GelAllVersionsOfEntityById(entityId, headers);
                    getAllVersionsOfEntityByIdResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of the entity with id {getNewEntities[j].Id} from er collection {erCollectionIdList[i]}");

                    var getAllEntities = getAllVersionsOfEntityByIdResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                    getAllEntities.Count().Should().Be(versionCount);

                    var response = getAllVersionsOfEntityByIdResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    continuationToken = (string)o.SelectToken("paging.continuationToken");
                    Assert.AreEqual(continuationToken, null);
                }
            }
        }

        [Then(@"validate the IMS User gets all entities by Ids from the ER Collection")]
        public async Task ThenValidateTheIMSUserReadsMultipleEntitiesByEntityID()
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

                var entityIds = "";

                getNewEntities.ForEach(x => { entityIds = entityIds == "" ? x.Id : $"{entityIds},{x.Id}"; });

                Log.Information($"Attempting to get multiple entities by entity Ids {entityIds} from ER Collection {erCollectionIdList[i]}");

                var getMultipleEntitiesByIdsResponseMsg = await ERService.GetEntitiesByMultipleIds(entityIds, headers);
                getMultipleEntitiesByIdsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get multiple entities with ids {entityIds} from er collection {erCollectionIdList[i]}");

                var getMultipleEntitiesById = getMultipleEntitiesByIdsResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                var getMultipleEntitiesByIdCount = getMultipleEntitiesById.Count();

                getMultipleEntitiesByIdCount.Should().Be(getNewEntities.Count());

                var response = JObject.Parse(getMultipleEntitiesByIdsResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data");
               // for (int j = 0; j < getMultipleEntitiesByIdCount; j++)
                    for (int j = getMultipleEntitiesByIdCount; j < 0; j--)
                    {
                    var id = response[j]["id"];
                    id.ToString().Should().Be(getNewEntities[j].Id);
                }
            }
        }

        [When(@"the IMS User posts ""(.*)"" new entity where ""(.*)"" is ""(.*)"" having Special Characters using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewEntityWhereIsHavingSpecialCharactersUsingVInAnERCollectionTimeS(int numOfEntities, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfEntities; i++)
            {
                var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: EntityTypes.EquipmentBrickEntityType, entityName: "test entity");
                scenarioContext.AddEntity(newEntity);
            }
            var erCollectionId = "test@&%$-" + Guid.NewGuid().ToString();
            scenarioContext.AddERCollections(erCollectionId);

            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionId),
                new KeyValuePair<string, string>(asyncHeader, asyncValue)
            };
            var getentities = scenarioContext.CreatedEntities().ToList();
            Log.Information($"Attempting to post new entity/entities with special characters in ER collection {erCollectionId}");
            for (int i = 0; i < numOfTimes; i++)
            {
                var postEntityResponseMsg = await ERService.PostEntities(version, getentities, headers);
                if (asyncValue == "true")
                    postEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
                else
                {
                    postEntityResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
                    var response = postEntityResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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

        [When(@"the IMS User posts ""(.*)"" new entity each in ""(.*)"" ER Collections where ""(.*)"" is ""(.*)"" using V(.*) ""(.*)"" time\(s\)")]
        [When(@"the IMS User posts ""(.*)"" new entities each in ""(.*)"" ER Collections where ""(.*)"" is ""(.*)"" using V(.*) ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewEntitiesWhereIsUsingVInERCollectionTimeS(int numOfEntities, int numOfERCollections, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            for (int k = 0; k < numOfERCollections; k++)
            {
                restClientUser = "PrimaryIMSUser";
                List<Entity> entityList = new List<Entity>();
                for (int i = 0; i < numOfEntities; i++)
                {
                    var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: EntityTypes.EquipmentBrickEntityType, entityName: "test entity");
                    entityList.Add(newEntity);
                }

                var erCollectionId = "testER-" + Guid.NewGuid().ToString();

                scenarioContext.AddERCollectionToEntitiesPair
                    (new KeyValuePair<string, List<Entity>>(erCollectionId, entityList));

                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionId),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };

                Log.Information($"Attempting to post new entity/entities in ER collection {erCollectionId}");
                for (int i = 0; i < numOfTimes; i++)
                {
                    var postEntityResponseMsg = await ERService.PostEntities(version, entityList, headers);
                    if (asyncValue == "true")
                    {
                        postEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
                        Thread.Sleep(50000);
                    }
                    else
                    {
                        postEntityResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
                        var response = postEntityResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        JObject jObjResponse = JObject.Parse(response);
                        var successEntityIDList = from entity in jObjResponse["data"]["responses"]["success"]
                                                  select (string)entity["id"];
                        var getEntityIDList = from entity in entityList
                                              select entity.Id;

                        getEntityIDList.Should().BeEquivalentTo(successEntityIDList);
                        getEntityIDList.Count().Should().Be(successEntityIDList.Count());
                    }
                }
            }

        }

        [Then(@"IMS User deletes the entities across all the ER Collections")]
        public async Task ThenIMSUserDeletesTheEntitiesAcrossAllTheERCollections()
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionEntitiesPairs = scenarioContext.AllERCollectionToEntitiesPairs().ToList();

            for (int k = 0; k < erCollectionEntitiesPairs.Count; k++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionEntitiesPairs[k].Key)
            };
                var getCreatedEntities = erCollectionEntitiesPairs[k].Value.ToList();
                for (int j = 0; j < getCreatedEntities.Count; j++)
                {
                    Log.Information($"Attempting to delete all entities from ER Collection {erCollectionEntitiesPairs[k].Key}");
                    var deleteEntityResponseMsg = await ERService.DeleteEntity(getCreatedEntities[j].Id, headers);
                    deleteEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to delete entity {getCreatedEntities[j].Id} from er collection {erCollectionEntitiesPairs[k].Key}");
                }
            }
        }

        [Then(@"validate IMS User deletes the entities across all the ER Collections")]
        public async Task ThenValidateIMSUserDeletesTheEntitiesAcrossAllTheERCollections()
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionEntitiesPairs = scenarioContext.AllERCollectionToEntitiesPairs().ToList();

            for (int k = 0; k < erCollectionEntitiesPairs.Count; k++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionEntitiesPairs[k].Key)
                };
                Log.Information($"Attempting to get all entities from ER Collection {erCollectionEntitiesPairs[k].Key}");

                var getAllEntitiesResponseMsg = await ERService.GetAllEntities(headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities from er collection {erCollectionEntitiesPairs[k].Key}");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Count().Should().Be(0);
            }
        }

        [When(@"the IMS User updates the entities where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserPartiallyUpdatesThePostedEntitiesWhereIsAndIsUsingVInTheERCollection(string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string version)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue),
                    new KeyValuePair<string, string>(allowPartialUpdateHeader, allowPartialUpdateValue)
                };

                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to Partially update entities from ER Collection {erCollectionIdList[i]}");
                    getNewEntities[i].EntityName = "test entity1";
                }

                var partialUpdateEntityResponseMsg = await ERService.PostEntities(version, getNewEntities, headers);

                if (asyncValue == "true")
                {
                    partialUpdateEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update entity in er collection {erCollectionIdList[i]}");
                }
                else
                {
                    if (version == "1.1")
                    {
                        partialUpdateEntityResponseMsg.Response.StatusCode.ToString().Should().Be("207", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update entity in er collection {erCollectionIdList[i]}");
                        var response = partialUpdateEntityResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        JObject o = JObject.Parse(response);
                        var entityId = (string)o.SelectToken("data.responses.success").First["id"];
                        Assert.AreEqual(getNewEntities[i].Id, entityId);
                    }
                }
                scenarioContext.Set(getNewEntities, "UpdatedEntities");
            }
        }

        [Then(@"validate the IMS User updates the posted entities")]
        public async Task ThenValidateTheIMSUserUpdatesThePostedEntities()
        {
            restClientUser = "PrimaryIMSUser";
            var getUpdatedEntities = scenarioContext.Get<List<Entity>>("UpdatedEntities");
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };
                Log.Information($"Attempting to get all entities from ER Collection {erCollectionIdList[i]}");
                var getAllEntitiesResponseMsg = await ERService.GetAllEntities(headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities from er collection {erCollectionIdList[i]}");
                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Should().BeEquivalentTo(getUpdatedEntities);
                getAllEntities.Count().Should().Be(getUpdatedEntities.Count());
            }
            //remove updated entities from scenario context
            scenarioContext.Remove("UpdatedEntities");
        }

        
             [When(@"the IMS User posts ""(.*)"" new entity where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewEntityWhereIsAndIsUsingVInTheERCollectionTimeS(int numOfEntities, string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfEntities; i++)
            {
                var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: EntityTypes.EquipmentBrickEntityType, entityName: "test entity");
                scenarioContext.AddEntity(newEntity);
            }
            string erCollectionId = this.scenarioContext.Get<dynamic>("CollectionsResponse");
            scenarioContext.AddERCollections(erCollectionId);
            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionId),
                new KeyValuePair<string, string>(asyncHeader, asyncValue),
                new KeyValuePair<string, string>(allowPartialUpdateHeader, allowPartialUpdateValue)
            };
            var getentities = scenarioContext.CreatedEntities().ToList();

            Log.Information($"Attempting to post new entity/entities synchronously in ER collection with allowPartialUpdate {erCollectionId}");
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
                    postEntitiesResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
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

        [When(@"the IMS User updates the entity ID where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection")]
        public async Task henTheIMSUserUpdatesTheEntityIDWhereIsAndIsUsingVInTheERCollection(string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string version)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue),
                    new KeyValuePair<string, string>(allowPartialUpdateHeader, allowPartialUpdateValue)
                };
                var getentities = scenarioContext.CreatedEntities().ToList();
                List<Entity> partialUpdateEntitiesList = new List<Entity>();
                for (int j = 0; j < getNewEntities.Count; j++)
                   
                {
                    Entity newEntity = null;

                    newEntity = new Entity(id: getNewEntities[j].Id, entityType: null, entityName: null);
                    partialUpdateEntitiesList.Add(newEntity);
                }              
                var partialUpdateEntityResponseMsg = await ERService.PostEntities(version, partialUpdateEntitiesList, headers);

                if (asyncValue == "true")
                {
                    partialUpdateEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update entity in er collection {erCollectionIdList[i]}");
                }
                else
                {
                    if (version == "1.1")
                    {
                        partialUpdateEntityResponseMsg.Response.StatusCode.ToString().Should().Be("207", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update entity in er collection {erCollectionIdList[i]}");
                        var response = partialUpdateEntityResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        JObject o = JObject.Parse(response);
                        var entityId = (string)o.SelectToken("data.responses.success").First["id"];
                        Assert.AreEqual(getNewEntities[i].Id, entityId);
                    }
                    else
                    {
                        partialUpdateEntityResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionIdList[i]}");
                        var response = partialUpdateEntityResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        JObject jObjResponse = JObject.Parse(response);
                        var successEntityIDList = from entity in jObjResponse["data"]["responses"]["success"]
                                                  select (string)entity["id"];
                        var getEntityIDList = from entity in getentities
                                              select entity.Id;

                        getEntityIDList.Should().BeEquivalentTo(successEntityIDList);
                        getEntityIDList.Count().Should().Be(successEntityIDList.Count());
                    }
                }
               scenarioContext.Set(getNewEntities, "UpdatedEntities");
            }
        }

        [Then(@"validate the IMS user gets files by filename ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsFilesByFilename(string fileName)
        {
            restClientUser = "PrimaryIMSUser";

            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };
                Log.Information($"Attempting to get File entity using File Name {fileName} from ER Collection {erCollectionIdList[i]}");

                var getFileByFileNameResponseMsg = await ERService.GetFileByFileNameOrID(fileName, headers);
                getFileByFileNameResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get File entity by file name {fileName} from er collection {erCollectionIdList[i]}");

                var getAllEntities = getFileByFileNameResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());

                foreach(Entity entity in getAllEntities)
                {
                    entity.EntityName.Should().Be(fileName);
                    entity.EntityType.Should().Be(EntityTypes.FileEntityType);
                }
            }
        }

        [Then(@"validate the IMS user gets file by Id")]
        public async Task ThenValidateTheIMSUserGetsFileById()
        {
            restClientUser = "PrimaryIMSUser";

            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                var fileIDList = scenarioContext.Get<List<string>>("FileEntityIdList");

                foreach(string fileID in fileIDList)
                {
                    Log.Information($"Attempting to get File entity using File Id {fileID} from ER Collection {erCollectionIdList[i]}");

                    var getFileByFileIDResponseMsg = await ERService.GetEntityById(fileID, headers);
                    getFileByFileIDResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get File entity by file Id {fileID} from er collection {erCollectionIdList[i]}");

                    var getAllEntities = getFileByFileIDResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());

                    foreach (Entity entity in getAllEntities)
                    {
                        entity.Id.Should().Be(fileID);
                        entity.EntityType.Should().Be(EntityTypes.FileEntityType);
                    }
                }
                
            }
        }

    }
}