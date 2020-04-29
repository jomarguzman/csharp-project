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
    public class ERPermissionsManagement
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public ERPermissionsManagement(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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

        private ACLServiceClient ACLService
        {
            get
            {
                if (!scenarioContext.TryGetValue(restClientUser, out IRestClient user))
                {
                    restClientUser = "PrimaryIMSUser";
                }
                var aclServiceClient = new ACLServiceClient(scenarioContext.Get<IRestClient>(restClientUser));
                return aclServiceClient;
            }
        }

        private void GenerateTokenForSecondaryIMSUser()
        {
            var restClientOptions = new RestClientOptions
            {
                BaseEndpoint = _testCfg.ERApiEndpoint,
                TokenProvider = new ResourceOwnerFlow(
                        _testCfg.ImsEndpoint,
                        _testCfg.ImsClientId,
                        _testCfg.ImsClientSecret,
                        _testCfg.SecondaryUsername,
                        _testCfg.SecondaryPassword,
                        _testCfg.ImsScopes),
                RetryPolicy = new DefaultRetryPolicy()
            };
            var erRestClient = new RestClient(restClientOptions);
            scenarioContext.Set<IRestClient>(erRestClient, "SecondaryIMSUser");
        }

        private void SetUpPrimaryUserACLClient()
        {
            var restClientOptions = new RestClientOptions
            {
                BaseEndpoint = _testCfg.ACLEndpoint,
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
            scenarioContext.Set<IRestClient>(erRestClient, "PrimaryACLUser");
        }

        [When(@"the IMS User sets ""(.*)"" permissions for Secondary User ""(.*)""")]
        public async Task WhenTheIMSUserSetsPermissionsForTheSecondaryUserAsync(string permissions, string secondaryUserName)
        {
            //make sure the secondary username given in specflow step matches with app.config value
            secondaryUserName.Should().BeEquivalentTo(_testCfg.SecondaryUsername, "given secondary username doesn't match with app settings secondary username. Please correct!");

            SetUpPrimaryUserACLClient();
            restClientUser = "PrimaryACLUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var postSecondaryAclERCollectionResp = await ACLService.PostSecondaryPermissions(erCollectionIdList[i], secondaryUserName, permissions);
                postSecondaryAclERCollectionResp.Response.StatusCode.Should().Be(HttpStatusCode.Created, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to set {permissions} permissions to secondary user for er collection {erCollectionIdList[i]}");
            }
            Thread.Sleep(300000); //sleep for 5 minutes as ACL memory cache gets updated every 5 minutes 
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions can post ""(.*)"" new entity where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        [Then(@"validate the Secondary User with ""(.*)"" permissions can post ""(.*)"" new entities where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheSecondaryUserWithCanPostNewEntityWhereIsInAnERCollectionTimeS(string permission, int numOfEntities, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            GenerateTokenForSecondaryIMSUser();
            restClientUser = "SecondaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                for (int j = 0; j < numOfEntities; j++)
                {
                    var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: EntityTypes.EquipmentBrickEntityType, entityName: "test entity");
                    scenarioContext.AddEntity(newEntity);
                }
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };
                var getentities = scenarioContext.CreatedEntities().ToList();

                Log.Information($"Attempting to post new entity/entities in ER collection {erCollectionIdList[i]}");
                for (int k = 0; k < numOfTimes; k++)
                {
                    var postEntitiesResponseMsg = await ERService.PostEntities(version, getentities, headers);
                    if (asyncValue == "true")
                    {
                        postEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionIdList[i]}");
                        Thread.Sleep(50000);
                    }
                    else
                    {
                        postEntitiesResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionIdList[i]}");
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
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions can get all entities in an ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithCanGetAllEntitiesInAnERCollectionAsync(string permission)
        {
            restClientUser = "SecondaryIMSUser";
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
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scope should be able to get all entities from er collection {erCollectionIdList[i]}");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Should().Equals(getNewEntities);
                getAllEntities.Count().Equals(getNewEntities.Count());

            }
        }

        [Then(@"the Secondary User with ""(.*)"" permissions can delete the entity from the ER Collection")]
        [Then(@"the Secondary User with ""(.*)"" permissions can delete the entities from the ER Collection")]
        public async Task ThenTheSecondaryUserWithPermissionsCanDeleteTheEntityFromTheERCollection(string permission)
        {
            restClientUser = "SecondaryIMSUser";

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
                    deleteAEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scope should be able to delete entity {getNewEntities[j].Id} from er collection {erCollectionIdList[i]}");
                }
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions can delete the entity from the ER Collection")]
        [Then(@"validate the Secondary User with ""(.*)"" permissions can delete the entities from the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCanDeleteTheEntityFromTheERCollection(string permission)
        {
            restClientUser = "SecondaryIMSUser";

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
                    Log.Information($"Attempting to get all entities from ER Collection {erCollectionIdList[i]}");

                    var getAllEntitiesResponseMsg = await ERService.GetAllEntities(headers);
                    getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scope should be able to get all entities from er collection {erCollectionIdList[i]}");

                    var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                    getAllEntities.Count().Equals(0);
                }
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot post ""(.*)"" new entity where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot post ""(.*)"" new entities where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheSecondaryUserWithPermissionsCannotPostsNewEntityWhereIsInAnERCollectionTimeS(string permission, int numOfEntities, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            GenerateTokenForSecondaryIMSUser();
            restClientUser = "SecondaryIMSUser";

            var getPrimaryCreatedEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {

                for (int j = 0; j < numOfEntities; j++)
                {
                    var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: EntityTypes.EquipmentBrickEntityType, entityName: "test entity");
                    scenarioContext.AddEntity(newEntity);
                }

                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };

                var getentities = scenarioContext.CreatedEntities().ToList();

                Log.Information($"Attempting to post new entity/entities synchronously in ER collection {erCollectionIdList[i]}");
                for (int k = 0; k < numOfTimes; k++)
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

                for (int l = 0; l < getentities.Count; l++)
                {
                    scenarioContext.RemoveEntity(scenarioContext.CreatedEntities().Find(entity => !getPrimaryCreatedEntities.Contains(entity)));
                }
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions gets ""(.*)"" Forbidden error code")]
        public void ThenValidateTheSecondaryUserWithPermissionsGetsForbiddenErrorCode(string permission, HttpStatusCode errorCode)
        {
            var getAllResponseErrors = scenarioContext.AllRestClientExceptionErrors().ToList();
            for (int i = 0; i < getAllResponseErrors.Count; i++)
            {
                getAllResponseErrors[i].Response.StatusCode.Should().Be(errorCode, $"{restClientUser} user with {permission} scope should not be able to post a new entity");
            }

        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot delete the entity from the ER Collection")]
        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot delete the entities from the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermisionsCannotDeleteTheEntityFromTheERCollectionAsync(string permission)
        {
            restClientUser = "SecondaryIMSUser";

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
                        ex.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden, $"{restClientUser} user with {permission} scope should be not able to delete entity {getNewEntities[j].Id} from er collection {erCollectionIdList[i]}");
                    }
                }
            }
        }

        [Then(@"the IMS User deletes ""(.*)"" permissions from the Secondary User")]
        public async Task ThenTheIMSUserCanDeleteTheSecondaryUserWithPermissionsAsync(string permissions)
        {
            restClientUser = "PrimaryACLUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var username = _testCfg.SecondaryUsername;

                var deleteSecondaryAclERCollectionResp = await ACLService.DeleteSecondaryPermissions(erCollectionIdList[i], username, permissions);

                deleteSecondaryAclERCollectionResp.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to remove {permissions} permissions of secondary user for ER collection {erCollectionIdList[i]}");
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot get all entities in an ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCannotGetAllEntitiesInAnERCollectionAsync(string permissions)
        {
            restClientUser = "SecondaryIMSUser";

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
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden, $"{restClientUser} user with {permissions} scope should not be able to get all entities from er collection");
                }
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions can post ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheSecondaryUserWithPermissionsCanPostNewRelationshipSBetweenTheEntitiesWhereIsInTheERCollectionTimeSAsync(string permissions, int numOfRelationships, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            restClientUser = "SecondaryIMSUser";
            for (int j = 0; j < numOfRelationships; j++)
            {
                int index = j;
                var NewEntities = scenarioContext.CreatedEntities().ToList();
                while (NewEntities.Count <= index + 1)
                    index = index + 1 - NewEntities.Count;

                var sourceEntityId = NewEntities[index].Id;
                var destinationEntityId = NewEntities[index + 1].Id;
                var newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: RelationshipTypes.BrickRelationshipHasPartType, relationshipName: "test relationship", sourceEntityId: sourceEntityId, destinationEntityId: destinationEntityId);
                scenarioContext.AddRelationship(newRelationship);
            }

            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int k = 0; k < erCollectionIdList.Count; k++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[k]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };
                var getRelationships = scenarioContext.CreatedRelationships().ToList();

                Log.Information($"Attempting to post new Relationship/Relationships between Entities synchronously in ER collection {erCollectionIdList[k]}");
                for (int i = 0; i < numOfTimes; i++)
                {

                    var postRelationshipResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);
                    if (asyncValue.Equals("true"))
                    {
                        postRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new Relationship in er collection {erCollectionIdList[k]}");
                        Thread.Sleep(50000);
                    }
                    else
                    {
                        postRelationshipResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new relationship in er collection {erCollectionIdList[k]}");
                        var response = postRelationshipResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        JObject jObjResponse = JObject.Parse(response);
                        var successRelationshipIDList = from relationship in jObjResponse["data"]["responses"]["success"]
                                                        select (string)relationship["id"];
                        var getRelationshipIDList = from relationship in getRelationships
                                                    select relationship.Id;

                        getRelationshipIDList.Should().BeEquivalentTo(successRelationshipIDList);
                        getRelationshipIDList.Count().Should().Be(successRelationshipIDList.Count());
                    }
                }
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot post ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheSecondaryUserWithPermissionsCannotPostNewRelationshipSBetweenTheEntitiesWhereIsInTheERCollectionTimeSAsync(string permissions, int numOfRelationships, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            restClientUser = "SecondaryIMSUser";
            var getPrimaryCreatedRelationships = scenarioContext.CreatedRelationships().ToList();
            for (int i = 0; i < numOfRelationships; i++)
            {
                int index = i;
                var NewEntities = scenarioContext.CreatedEntities().ToList();
                while (NewEntities.Count <= index + 1)
                    index = index + 1 - NewEntities.Count;

                var sourceEntityId = NewEntities[index].Id;
                var destinationEntityId = NewEntities[index + 1].Id;
                var newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: RelationshipTypes.BrickRelationshipHasPartType, relationshipName: "test relationship", sourceEntityId: sourceEntityId, destinationEntityId: destinationEntityId);
                scenarioContext.AddRelationship(newRelationship);
            }

            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };
                var getRelationships = scenarioContext.CreatedRelationships().ToList();

                Log.Information($"Attempting to post new Relationship/Relationships between Entities synchronously in ER collection {erCollectionIdList[i]}");
                for (int j = 0; j < numOfTimes; j++)
                {
                    RestClientResponse<PlatformItemResponse> postRelationshipsResponseMsg;
                    try
                    {
                        postRelationshipsResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);
                    }
                    catch (RestClientException ex)
                    {
                        scenarioContext.AddRestClientExceptionErrors(ex);
                    }
                }
                for (int k = 0; k < getRelationships.Count; k++)
                {
                    scenarioContext.RemoveRelationship(scenarioContext.CreatedRelationships().Find(relationship => !getPrimaryCreatedRelationships.Contains(relationship)));
                }
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions can get all relationships from the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCanGetAllRelationshipsFromTheERCollectionAsync(string permission)
        {
            restClientUser = "SecondaryIMSUser";
            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get all relationships from ER Collection {erCollectionIdList[i]}");
                var getAllRelationshipsResponseMsg = await ERService.GetAllRelationships(headers);
                getAllRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scope should be able to get all relationships from er collection {erCollectionIdList[i]}");

                var getAllRelationships = getAllRelationshipsResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                var count = getAllRelationships.Count();
                getAllRelationships.Should().Equals(getNewRelationships);
                getAllRelationships.Count().Should().Equals(getNewRelationships.Count());
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot get all relationships from the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCannotGetAllRelationshipsFromTheERCollectionAsync(string permission)
        {
            restClientUser = "SecondaryIMSUser";

            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
            };

                Log.Information($"Attempting to get all relationships from ER Collection {erCollectionIdList[i]}");
                try
                {
                    var getAllRelationshipsResponseMsg = await ERService.GetAllRelationships(headers);
                }
                catch (RestClientException ex)
                {
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden, $"{restClientUser} user with {permission} scope should not be able to get all relationships from er collection {erCollectionIdList[i]}");
                }
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" Permissions cannot delete the relationships from the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCannotDeleteTheRelationshipsFromTheERCollectionAsync(string permission)
        {
            restClientUser = "SecondaryIMSUser";

            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                for (int j = 0; j < getNewRelationships.Count; j++)
                {
                    Log.Information($"Attempting to delete relationship from ER Collection {erCollectionIdList[i]}");
                    try
                    {
                        var deleteARelationshipResponseMsg = await ERService.DeleteRelationship(getNewRelationships[j].Id, headers);
                    }
                    catch (RestClientException ex)
                    {
                        ex.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden, $"{restClientUser} user with {permission} scope should not be able to delete relationships from er collection {erCollectionIdList[i]}");
                    }
                }
            }
        }

        [Then(@"the Secondary User with ""(.*)"" permissions can delete the relationship\(s\) from the ER Collection")]
        public async Task ThenTheSecondaryUserWithPermissionsCanDeleteTheRelationshipSFromTheERCollectionAsync(string permission)
        {
            restClientUser = "SecondaryIMSUser";
            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                for (int j = 0; j < getNewRelationships.Count; j++)
                {
                    Log.Information($"Attempting to delete relationship from ER Collection {erCollectionIdList[i]}");

                    var deleteARelationshipResponseMsg = await ERService.DeleteRelationship(getNewRelationships[j].Id, headers);
                    deleteARelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scope should be able to delete relationship {getNewRelationships[j].Id} from er collection {erCollectionIdList[i]}");
                }
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions can delete the relationship\(s\) from the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCanDeleteTheRelationshipSFromTheERCollectionAsync(string permission)
        {
            restClientUser = "SecondaryIMSUser";

            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get all relationships from ER Collection {erCollectionIdList[i]}");

                var getAllRelationshipsResponseMsg = await ERService.GetAllRelationships(headers);
                getAllRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scope should be able to get all relationships from er collection {erCollectionIdList[i]}");

                var getAllRelationships = getAllRelationshipsResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                getAllRelationships.Count().Equals(0);
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions gets stats by ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsGetsStatsByERCollection(string permission)
        {
            GenerateTokenForSecondaryIMSUser();
            restClientUser = "SecondaryIMSUser";
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
                for (int j = 0; j < getNewEntities.Count(); j++)
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

        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot get stats by ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionscannotgetStatsByERCollection(string permission)
        {
            GenerateTokenForSecondaryIMSUser();
            restClientUser = "SecondaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                Log.Information($"Attempting to get stats by ER Collection {erCollectionIdList[i]}");


                try
                {
                    var getStatsByerCollectionResponseMsg = await ERService.GetStatsByERCollection(erCollectionIdList[i]);
                }
                catch (RestClientException ex)
                {
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden, $"{restClientUser} user with {permission} scope should not be able to get stats by er collection {erCollectionIdList[i]}");
                }
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions gets the list of Assets within the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsGetsTheListOfAssetsWithinTheERCollection(string permission)

        {
            GenerateTokenForSecondaryIMSUser();
            restClientUser = "SecondaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };
                Log.Information($"Attempting to get the list of Assets from ER Collection {erCollectionIdList[i]}");

                var getAllAssetsResponseMsg = await ERService.GetAllAssets(headers);
                getAllAssetsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scopes should be able to get the list of Assets from er collection {erCollectionIdList[i]}");

                var getExpectedEntities = scenarioContext.CreatedEntities().ToList().FindAll(s => s.EntityType.Equals(EntityTypes.EquipmentBrickEntityType));
                var getAllEntities = getAllAssetsResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Should().BeEquivalentTo(getExpectedEntities);
                getAllEntities.Count().Should().Be(getExpectedEntities.Count());
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions gets the list of Spaces within the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsGetsTheListOfSpacesWithinTheERCollection(string permission)
        {
            restClientUser = "SecondaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };
                Log.Information($"Attempting to get list of Spaces from ER Collection {erCollectionIdList[i]}");

                var getAllEntitiesResponseMsg = await ERService.GetAllSpaces(headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scopes should be able to get list of Spaces from er collection {erCollectionIdList[i]}");

                var getExpectedEntities = scenarioContext.CreatedEntities().ToList().FindAll(s => s.EntityType.Equals(EntityTypes.BuildingBrickEntityType));
                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Should().BeEquivalentTo(getExpectedEntities);
                getAllEntities.Count().Should().Be(getExpectedEntities.Count());
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot get the list of Assets within the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCannotGetTheListOfAssetsWithinTheERCollection(string permission)
        {
            GenerateTokenForSecondaryIMSUser();
            restClientUser = "SecondaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get the list of Assets from ER Collection {erCollectionIdList[i]}");

                try
                {
                    var getAllAssetsResponseMsg = await ERService.GetAllAssets(headers);
                }

                catch (RestClientException ex)
                {
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden, $"{restClientUser} user with {permission} scope should not be able to get the list of Assets within er collection {erCollectionIdList[i]}");
                }
            }

        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot get the list of Spaces within the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCannotGetTheListOfSpacesWithinTheERCollection(string permission)
        {
            restClientUser = "SecondaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get list of Spaces from ER Collection {erCollectionIdList[i]}");

                try
                {
                    var getAllEntitiesResponseMsg = await ERService.GetAllSpaces(headers);
                }
                catch (RestClientException ex)
                {
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden, $"{restClientUser} user with {permission} scope should not be able to get the list of Spaces within er collection {erCollectionIdList[i]}");
                }
            }
        }



        [Then(@"validate the Secondary User with ""(.*)"" permissions gets all entities by specific label from the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsGetsAllEntitiesBySpecificLabelFromTheERCollection(string permission)
        {

            GenerateTokenForSecondaryIMSUser();
            restClientUser = "SecondaryIMSUser";
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
                getEntityByLabelResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scopes should be able to get all entities by specific label from er collection {erCollectionIdList[i]}");

                var getAllEntities = getEntityByLabelResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Should().BeEquivalentTo(getNewEntities.FindAll(s => s.Labels.Contains(label)));
                getAllEntities.Count().Should().Be(getNewEntities.FindAll(s => s.Labels.Contains(label)).Count());
            }
        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions cannot get all entities by specific label from the ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCannotGetAllEntitiesBySpecificLabelFromTheERCollection(string permission)
        {
            GenerateTokenForSecondaryIMSUser();
            restClientUser = "SecondaryIMSUser";
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

                try
                {
                    var getEntityByLabelResponseMsg = await ERService.GetEntitiesByLabel(label, headers: headers);
                }
                catch (RestClientException ex)
                {
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden, $"{restClientUser} user with {permission} scope should not be able to get all entities by specific label from er collection {erCollectionIdList[i]}");
                }
            }
        }

        [Then(@"the Secondary User with ""(.*)"" permissions Cannot copies entities and relationships from this ER Collection to another ER Collection")]
        public async Task ThenTheSecondaryUserWithPermissionsCannotCopiesEntitiesAndRelationshipsFromThisERCollectionToAnotherERCollection(string permission)

        {
            GenerateTokenForSecondaryIMSUser();
            restClientUser = "SecondaryIMSUser";
            var sourceERCollectionId = "";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                sourceERCollectionId = erCollectionIdList[i];
            }
            var destinationERCollectionId = "destination-" + Guid.NewGuid().ToString();

            var copyErCollectionsList = new List<CopyERCollections>{
                new CopyERCollections{
                    SourceERCollectionID = sourceERCollectionId,
                    DestinationERCollectionID = destinationERCollectionId
                }
            };
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                Log.Information($"Attempting to copy entities and relationships from source ER collection {sourceERCollectionId} to destination ER collection{destinationERCollectionId}");
                try
                {
                    var postEntitiesResponseMsg = await ERService.CopyERCollections(copyErCollectionsList);
                }
                catch (RestClientException ex)
                {
                    ex.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden, $"{restClientUser} user with {permission} scope should not be able to copy all entities and relationship from er collection {erCollectionIdList[i]}");
                }
            }

        }

        [Then(@"the Secondary User with ""(.*)"" permissions Can copies entities and relationships from this ER Collection to another ER Collection")]
        public async Task ThenTheSecondaryUserWithPermissionsCanCopiesEntitiesAndRelationshipsFromThisERCollectionToAnotherERCollection(string permission)
        {
            GenerateTokenForSecondaryIMSUser();
            restClientUser = "SecondaryIMSUser";
            var sourceERCollectionId = "";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                sourceERCollectionId = erCollectionIdList[i];
            }
            string destinationERCollectionId = "destination-" + Guid.NewGuid().ToString();
            this.scenarioContext.Set<dynamic>(destinationERCollectionId, "desercollectionid");

            var copyErCollectionsList = new List<CopyERCollections>{
                new CopyERCollections{
                    SourceERCollectionID = sourceERCollectionId,
                    DestinationERCollectionID = destinationERCollectionId
                }
            };

            Log.Information($"Attempting to copy entities and relationships from source ER collection {sourceERCollectionId} to destination ER collection{destinationERCollectionId}");
            var postEntitiesResponseMsg = await ERService.CopyERCollections(copyErCollectionsList);
            var response = JObject.Parse(postEntitiesResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data");
            postEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes be able to copy all entities and relationship from {sourceERCollectionId} er collection");


        }

        [Then(@"validate the Secondary User with ""(.*)"" permissions can get all relationships from the destination ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCanGetAllRelationshipsFromTheDestinationERCollection(string permission)
        {
            restClientUser = "SecondaryIMSUser";
            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            string destinationERCollectionId = this.scenarioContext.Get<dynamic>("desercollectionid");
            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ercollectionid", destinationERCollectionId)
            };

            Log.Information($"Attempting to get all relationships from ER Collection {destinationERCollectionId}");
            var getAllRelationshipsResponseMsg = await ERService.GetAllRelationships(headers);
            getAllRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scope should be able to get all relationships from er collection {destinationERCollectionId}");

            var getAllRelationships = getAllRelationshipsResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
            var count = getAllRelationships.Count();
            getAllRelationships.Should().BeEquivalentTo(getNewRelationships);
            getAllRelationships.Count().Should().Be(getNewRelationships.Count());

        }


        [Then(@"validate the Secondary User with ""(.*)"" permissions can get all entities from the destination ER Collection")]
        public async Task ThenValidateTheSecondaryUserWithPermissionsCanGetAllEntitiesFromTheDestinationERCollection(string permission)
        {
            restClientUser = "SecondaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();

            string destinationERCollectionId = this.scenarioContext.Get<dynamic>("desercollectionid");

            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ercollectionid", destinationERCollectionId)
            };
            Log.Information($"Attempting to get all entities from ER Collection {destinationERCollectionId}");

            var getAllEntitiesResponseMsg = await ERService.GetAllEntities(headers);
            getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {permission} scope should be able to get all entities from er collection {destinationERCollectionId}");

            var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
            getAllEntities.Should().BeEquivalentTo(getNewEntities);
            getAllEntities.Count().Should().Be(getNewEntities.Count());

        }
    }


}





