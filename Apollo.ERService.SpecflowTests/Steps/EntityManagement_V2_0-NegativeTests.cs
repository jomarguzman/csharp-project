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
using Jci.Be.Data.Identity.HttpClient.Policies;
using Jci.Be.Data.Identity.HttpClient.TokenProvider;
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
    class EntityManagement_V2_0_NegativeTests
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public EntityManagement_V2_0_NegativeTests(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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

        private void GenerateTokenForInvalidIMSUser()
        {
            var restClientOptions = new RestClientOptions
            {
                BaseEndpoint = _testCfg.ERApiEndpoint,
                TokenProvider = new ResourceOwnerFlow(
                        _testCfg.InvalidImsEndpoint,
                        _testCfg.ImsClientId,
                        _testCfg.InvalidImsClientSecret,
                        _testCfg.PrimaryUserName,
                        _testCfg.PrimaryUserPassword,
                        _testCfg.ImsScopes),
                RetryPolicy = new DefaultRetryPolicy()
            };
            var erRestClient = new RestClient(restClientOptions);
            scenarioContext.Set<IRestClient>(erRestClient, "InvalidIMSUser");
        }

        [When(@"the IMS User posts ""(.*)"" new entity where ""(.*)"" is ""(.*)"" without the ""(.*)"" property using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        [When(@"the IMS User posts ""(.*)"" new entities where ""(.*)"" is ""(.*)"" without the ""(.*)"" property using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task ThenTheIMSUserPostsNewEntityWithoutAnyFieldInAnERCollectionTimeSAsync(int numOfEntities, string asyncHeader, string asyncValue, string missingField, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfEntities; i++)
            {
                Entity newEntity = null;

                if (missingField.Equals("entityId"))
                    newEntity = new Entity(id: null, entityType: BrickEntityTypes.FloorBrickEntityType, entityName: "test entity");

                if (missingField.Equals("entityType"))
                    newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: null, entityName: "test entity");

                if (missingField.Equals("entityName"))
                    newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: BrickEntityTypes.FloorBrickEntityType, entityName: null);

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

            Log.Information($"Attempting to post new entity/entities with missing field in ER collection {erCollectionId}");
            for (int j = 0; j < numOfTimes; j++)
            {
                RestClientResponse<PlatformItemResponse> postEntityResponseMsg = null;
                //try
                //{
                postEntityResponseMsg = await ERService.PostEntities(version, getentities, headers);
                //}
                //catch (RestClientException ex)
                //{
                //    scenarioContext.AddRestClientExceptionErrors(ex);
                //}
                if (asyncValue == "true")
                    postEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to post entity in er collection {erCollectionId}");
                else
                {
                    postEntityResponseMsg.Response.StatusCode.ToString().Should().Be("207", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to post entity in er collection {erCollectionId}");
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" Unauthorized error code for the post\(s\)")]
        [Then(@"validate the IMS User gets ""(.*)"" Internal server error code for the post\(s\)")]
        [Then(@"validate the IMS User gets ""(.*)"" Bad Request error code for the post\(s\)")]
        [Then(@"validate the IMS User gets ""(.*)"" Request Entity Too Large error code for the post\(s\)")]
        public void ThenValidateTheIMSUserErrorCodeForThePost(HttpStatusCode errorCode)
        {
            var getAllResponseErrors = scenarioContext.AllRestClientExceptionErrors().ToList();
            for (int i = 0; i < getAllResponseErrors.Count; i++)
            {
                getAllResponseErrors[i].Response.StatusCode.Should().Be(errorCode, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to post a new entity");
            }

        }

        [Then(@"validate the IMS User gets ""(.*)"" Not Found error")]
        public void ThenValidateTheIMSUSerGetsNotFoundError(HttpStatusCode respCode)
        {
            var getAllResponse = scenarioContext.AllHttpResponses().ToList();
            for (int i = 0; i < getAllResponse.Count; i++)
            {
                getAllResponse[i].Response.StatusCode.Should().Be(respCode, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to get new entity");
            }
        }

        [Then(@"validate IMS User gets ""(.*)"" entities from the ER Collection")]
        public async Task ThenValidateIMSUserCannotGetTheEntityFromTheERCollection(int numOfentities)
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
            };
                Log.Information($"Attempting to get all entities from ER Collection");
                var getAllEntitiesResponseMsg = await ERService.GetAllEntities(headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities from er collection");

                var getAllEntitiesResponseData = JObject.Parse(getAllEntitiesResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("paging");
                getAllEntitiesResponseData.SelectToken("totalCount").ToString().Should().Be("0");
            }
        }

        [When(@"the IMS User posts ""(.*)"" new entity where ""(.*)"" is ""(.*)"" using V(.*) without an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewEntityIsWithoutTheERCollectionTimes(int numOfEntities, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfEntities; i++)
            {
                var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: BrickEntityTypes.FloorBrickEntityType, entityName: "test entity");
                scenarioContext.AddEntity(newEntity);
            }
            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(asyncHeader, asyncValue)
            };
            var getentities = scenarioContext.CreatedEntities().ToList();
            Log.Information($"Attempting to post new entity/entities without an ER collection");
            for (int i = 0; i < numOfTimes; i++)
            {
                RestClientResponse<PlatformItemResponse> postEntityResponseMsg = null;
                try
                {
                    postEntityResponseMsg = await ERService.PostEntities(version, getentities, headers);
                }
                catch (RestClientException ex)
                {
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to post a new entity without an er collection");
                }
            }
        }

        [Then(@"validate IMS User cannot get the entity without ER Collection")]
        public async Task ThenValidateIMSUserCannotGetTheEntityWithoutERCollectionAsync()
        {
            restClientUser = "PrimaryIMSUser";
            Log.Information($"Attempting to get all entities without ER Collection");
            try
            {
                var getAllEntitiesResponseMsg = await ERService.GetAllEntities();
            }
            catch (RestClientException ex)
            {
                ex.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to get all entities without er collection");
            }
        }

        [When(@"the IMS User gets a specific entity by entity Id that doesn't exist from the ER Collection")]
        public async Task WhenTheIMSUserGetsASpecificEntityByEntityIdThatDoesnTExistFromTheERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
            };
                var wrongEntityId = $"wrongTestId-{Guid.NewGuid()}";
                // var wrongEntityId = this.scenarioContext.Get<dynamic>("CollectionsResponse");
                Log.Information($"Attempting to get specific entity by entity Id {wrongEntityId} from ER Collection {erCollectionIdList[i]}");
                var getEntityByIdResponseMsg = await ERService.GetEntityById(wrongEntityId, headers);
                scenarioContext.AddHttpResponse(getEntityByIdResponseMsg);
            }
        }

        [Then(@"validate IMS User cannot delete the entity without the ER Collection")]
        public async Task ThenIMSUserCannotDeleteTheEntityWithoutTheERCollectionAsync()
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            for (int i = 0; i < getNewEntities.Count; i++)
            {
                Log.Information($"Attempting to delete entities without ER Collection");
                RestClientResponse<PlatformItemResponse> deleteAEntityResponseMsg = null;
                try
                {
                    deleteAEntityResponseMsg = await ERService.DeleteEntity(getNewEntities[i].Id);
                }
                catch (RestClientException ex)
                {
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be not able to delete entity {getNewEntities[i].Id} without ER collection");
                }
            }
        }

        [When(@"the IMS User posts empty list ""(.*)"" new entities where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        [When(@"the IMS User posts ""(.*)"" new entities in bulk where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewEntitiesWhereIsInAnERCollectionTimeS(int numOfEntities, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfEntities; i++)
            {
                var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: BrickEntityTypes.FloorBrickEntityType, entityName: "test entity");
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


            Log.Information($"Attempting to post new entity/entities in ER collection {erCollectionId}");
            for (int i = 0; i < numOfTimes; i++)
            {
                RestClientResponse<PlatformItemResponse> postEntityResponseMsg = null;
                try
                {
                    postEntityResponseMsg = await ERService.PostEntities(version, getentities, headers);
                }
                catch (RestClientException ex)
                {
                    scenarioContext.AddRestClientExceptionErrors(ex);
                }
            }
        }

        [Then(@"validate IMS User gets ""(.*)"" Forbidden error code for get all entities request")]
        public async Task ThenValidateIMSUserGetsForbiddenErrorCodeForGetAllEntitiesRequestAsync(int p0)
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
            };
                Log.Information($"Attempting to get all entities from ER Collection");
                try
                {
                    var getAllEntitiesResponseMsg = await ERService.GetAllEntities(headers);
                }
                catch (RestClientException ex)
                {
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to get all entities from er collection");
                }
            }
        }

        [Then(@"validate IMS User gets ""(.*)"" Forbidden error code for delete entities request")]
        public async Task ThenValidateIMSUserGetsForbiddenErrorCodeForDeleteEntitiesRequestAsync(HttpStatusCode errorCode)
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
                    RestClientResponse<PlatformItemResponse> deleteAEntityResponseMsg = null;

                    try
                    {
                        deleteAEntityResponseMsg = await ERService.DeleteEntity(getNewEntities[j].Id, headers);
                    }
                    catch (RestClientException ex)
                    {
                        ex.Response.StatusCode.Should().Be(errorCode, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be not able to delete entity {getNewEntities[j].Id} from er collection {erCollectionIdList[i]}");
                        break;
                    }
                }
            }
        }

        [When(@"the IMS User updates the entities where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" without the ""(.*)"" property using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserPartiallyUpdatesThePostedEntitiesWhereIsAndIsWithoutThePropertyUsingVInTheERCollection(string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string propertyMissing, string version)
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

                List<Entity> partialUpdateEntitiesList = new List<Entity>();
                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to Partially update entities from ER Collection {erCollectionIdList[i]}");

                    Entity newEntity = null;

                    if (propertyMissing.Equals("entityId"))
                        newEntity = new Entity(id: null, entityType: getNewEntities[j].EntityType, entityName: getNewEntities[j].EntityName);

                    //if (propertyMissing.Equals("entityType"))
                    //    newEntity = new Entity(id: getNewEntities[j].Id, entityType: null, entityName: getNewEntities[j].EntityName);
                    partialUpdateEntitiesList.Add(newEntity);
                }

                //   RestClientResponse<PlatformItemResponse> postEntityResponseMsg = null;
                var partialUpdateEntityResponseMsg = await ERService.PostEntities(version, partialUpdateEntitiesList, headers);
                scenarioContext.AddPartialUpdateResponses(partialUpdateEntityResponseMsg);


                if (asyncValue == "true")
                    partialUpdateEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to partial update entity in er collection {erCollectionIdList[i]}");
                else
                {
                    partialUpdateEntityResponseMsg.Response.StatusCode.ToString().Should().Be("207", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to partial update entity in er collection {erCollectionIdList[i]}");
                }


                //try
                //{
                //    postEntityResponseMsg = await ERService.PostEntities(version, partialUpdateEntitiesList, headers);
                //}
                //catch (RestClientException ex)
                //{
                //    scenarioContext.AddRestClientExceptionErrors(ex);
                //}
            }
        }

        [When(@"the IMS User posts ""(.*)"" new entity where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection ""(.*)"" time\(s\) with Invalid Schema")]
        public async Task WhenTheIMSUserPostsNewEntityWhereIsAndIsUsingVInTheERCollectionTimeSWithInvalidSchema(int numOfEntities, string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfEntities; i++)
            {
                var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: "InvalidEntitySchema", entityName: "test entity");
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
                var partialUpdateEntityResponseMsg = await ERService.PostEntities(version, getentities, headers);
                scenarioContext.AddPartialUpdateResponses(partialUpdateEntityResponseMsg);


                if (asyncValue == "true")
                    partialUpdateEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to partial update entity in er collection {erCollectionId}");
                else
                {
                    partialUpdateEntityResponseMsg.Response.StatusCode.ToString().Should().Be("207", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to partial update entity in er collection {erCollectionId}");
                }
            }
        }

        [When(@"the IMS User updates the entities where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" with invalid property name ""(.*)"" and value ""(.*)"" in the ER Collection using V(.*)")]
        public async Task WhenTheIMSUserPartiallyUpdatesThePostedEntitiesWhereIsAndIsWithInvalidPropertyNameAndValueInTheERCollectionUsingV(string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string invalidPropertyName, string invalidPropertyValue, string version)
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

                List<Entity> partialUpdateEntitiesList = new List<Entity>();
                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to Partially update entities from ER Collection {erCollectionIdList[i]}");

                    Entity newEntity = new Entity(id: getNewEntities[j].Id, entityType: getNewEntities[j].EntityType, entityName: getNewEntities[j].EntityName)
                    {
                        InvalidProperty = invalidPropertyValue
                    };

                    partialUpdateEntitiesList.Add(newEntity);
                }
                var partialUpdateEntityResponseMsg = await ERService.PostEntities(version, partialUpdateEntitiesList, headers);
                scenarioContext.AddPartialUpdateResponses(partialUpdateEntityResponseMsg);


                if (asyncValue == "true")
                    partialUpdateEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to partial update entity in er collection {erCollectionIdList[i]}");
                else
                {
                    partialUpdateEntityResponseMsg.Response.StatusCode.ToString().Should().Be("207", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to partial update entity in er collection {erCollectionIdList[i]}");
                }

                //RestClientResponse<PlatformItemResponse> postEntityResponseMsg = null;
                //try
                //{
                //    postEntityResponseMsg = await ERService.PostEntities(version, partialUpdateEntitiesList, headers);
                //}
                //catch (RestClientException ex)
                //{
                //    scenarioContext.AddRestClientExceptionErrors(ex);
                //}
            }
        }

        [When(@"the IMS User change the entity type for the posted entity where ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserChangeTheEntityTypeForThePostedEntityWhereIsUsingVInTheERCollectionTimeS(string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";

            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };

                List<Entity> partialUpdateEntitiesList = new List<Entity>();
                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to update entities from ER Collection {erCollectionIdList[i]}");

                    Entity newEntity = new Entity(id: getNewEntities[j].Id, entityType: BrickEntityTypes.LocationBrickEntityType, entityName: getNewEntities[j].EntityName);
                    partialUpdateEntitiesList.Add(newEntity);
                }

                RestClientResponse<PlatformItemResponse> postEntityResponseMsg = null;
                try
                {
                    postEntityResponseMsg = await ERService.PostEntities(version, partialUpdateEntitiesList, headers);
                }
                catch (RestClientException ex)
                {
                    scenarioContext.AddRestClientExceptionErrors(ex);
                }
            }
        }

        [When(@"the IMS User updates entity type for the entity where ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserPartiallyUpdatesTheEntityTypeWhereIsAndIsUsingVInTheERCollection(string asyncHeader, string asyncValue, string version)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };

                List<Entity> partialUpdateEntitiesList = new List<Entity>();
                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to update entities from ER Collection {erCollectionIdList[i]}");
                    Entity newEntity = new Entity(id: getNewEntities[i].Id, entityType: BrickEntityTypes.BuildingBrickEntityType, entityName: getNewEntities[i].EntityName);
                    partialUpdateEntitiesList.Add(newEntity);
                }

                var partialUpdateEntityResponseMsg = await ERService.PostEntities(version, partialUpdateEntitiesList, headers);
                scenarioContext.AddPartialUpdateResponses(partialUpdateEntityResponseMsg);


                if (asyncValue == "true")
                    partialUpdateEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to partial update entity in er collection {erCollectionIdList[i]}");
                else
                {
                    partialUpdateEntityResponseMsg.Response.StatusCode.ToString().Should().Be("207", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to partial update entity in er collection {erCollectionIdList[i]}");
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" responses message ""(.*)"" while updating entity")]
        public void ThenValidateTheIMSUserGetsResponsesWhileUpdatingEntity(string error, string message)
        {
            var getAllResponses = scenarioContext.AllPartialUpdateResponses().ToList();
            for (int i = 0; i < getAllResponses.Count; i++)
            {
                getAllResponses[i].Response.StatusCode.ToString().Should().Be("207", $"user with {_testCfg.ImsScopes} scopes should not be able to update entities");
                var response = getAllResponses[i].Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                JObject o = JObject.Parse(response);

                o.SelectToken("data.responses.failure").Should().NotBeEmpty();
                var responseMessage = (string)(o.SelectToken("data.responses.failure").First["message"]);
                // Assert.AreEqual(message, responseMessage);
                Assert.IsTrue(responseMessage.Contains(message));
            }
        }

        [When(@"the IMS User posts ""(.*)"" new entity where ""(.*)"" is ""(.*)"" With Invalid Token using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewEntityWhereIsWithInvalidTokenUsingVInAnERCollectionTimeS(int numOfEntities, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            GenerateTokenForInvalidIMSUser();
            restClientUser = "InvalidIMSUser";
            string erCollectionId = this.scenarioContext.Get<dynamic>("CollectionsResponse");
            //scenarioContext.AddERCollections(erCollectionId);
            for (int i = 0; i < numOfEntities; i++)
            {
                var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: BrickEntityTypes.FloorBrickEntityType, entityName: "test entity");
                scenarioContext.AddEntity(newEntity);
            }
           
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionId),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };
                var getentities = scenarioContext.CreatedEntities().ToList();
            Log.Information($"Attempting to post new entity/entities without an ER collection");
            for (int i = 0; i < numOfTimes; i++)
            {
                RestClientResponse<PlatformItemResponse> postEntityResponseMsg = null;
                try
                {
                    postEntityResponseMsg = await ERService.PostEntities(version, getentities, headers);
                }
                catch (RestClientException ex)
                {
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to post a new entity without an er collection");
                }
            }
        }
    }
}
  
