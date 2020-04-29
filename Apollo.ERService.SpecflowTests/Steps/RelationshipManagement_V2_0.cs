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
    public class RelationshipManagement_V2_0
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public RelationshipManagement_V2_0(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewRelationshipSBetweenTheEntitiesWhereIsUsingVInTheERCollectionTimeS(int numOfRelationships, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";
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
            for (int k = 0; k < erCollectionIdList.Count; k++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[k]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };
                var getRelationships = scenarioContext.CreatedRelationships().ToList();

                Log.Information($"Attempting to post new Relationship/Relationships between Entities synchronously in ER collection {erCollectionIdList[k]}");
                for (int j = 0; j < numOfTimes; j++)
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

        [Then(@"validate the IMS User gets all relationships from the ER Collection")]
        [Then(@"validate the IMS User gets all relationships from the destination ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllRelationshipsFromTheERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                foreach (var relationship in getNewRelationships)
                {
                    relationship.SourceERCollectionId = erCollectionIdList[i];
                    relationship.DestinationERCollectionId = erCollectionIdList[i];
                }
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get all relationships from ER Collection {erCollectionIdList[i]}");
                var getAllRelationshipsResponseMsg = await ERService.GetAllRelationships(headers);
                getAllRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all relationships from er collection {erCollectionIdList[i]}");
                var getAllRelationships = getAllRelationshipsResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());

                getAllRelationships.Should().BeEquivalentTo(getNewRelationships);
                getAllRelationships.Count().Should().Be(getNewRelationships.Count());
            }
        }

        [Then(@"IMS User deletes the relationship\(s\) from the ER Collection")]
        [Then(@"IMS User deletes the relationship\(s\) from source & destination ER Collections")]
        public async Task ThenIMSUserDeletesTheRelationshipSFromTheERCollectionAsync()
        {
            restClientUser = "PrimaryIMSUser";
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
                    Thread.Sleep(3000);
                    var deleteARelationshipResponseMsg = await ERService.DeleteRelationship(getNewRelationships[j].Id, headers);
                    deleteARelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to delete relationship {getNewRelationships[j].Id} from er collection {erCollectionIdList[i]}");
                }
            }
        }

        [Then(@"validate IMS User deletes the relationship\(s\) from the ER Collection")]
        [Then(@"validate IMS User deletes the relationship\(s\) from source & destination the ER Collections")]
        public async Task ThenValidateIMSUserDeletesTheRelationshipSFromTheERCollectionAsync()
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

                var getAllRelationshipsResponseMsg = await ERService.GetAllRelationships(headers);
                getAllRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all relationships from er collection {erCollectionIdList[i]}");

                var getAllRelationships = getAllRelationshipsResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                getAllRelationships.Count().Should().Be(0);
            }
        }

        [Then(@"validate the IMS User gets a specific relationship by relationship Id from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsASpecificRelationshipByRelationshipIdFromTheERCollectionAsync()
        {
            restClientUser = "PrimaryIMSUser";

            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                foreach (var relationship in getNewRelationships)
                {
                    relationship.SourceERCollectionId = erCollectionIdList[i];
                    relationship.DestinationERCollectionId = erCollectionIdList[i];
                }

                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                for (int j = 0; j < getNewRelationships.Count; j++)
                {
                    var relationshipId = getNewRelationships[j].Id;
                    Log.Information($"Attempting to get specific Relationship by Relationship Id {relationshipId} from ER Collection {erCollectionIdList[i]}");

                    var getRelationshipByIdResponseMsg = await ERService.GetRelationshipById(relationshipId, headers);
                    getRelationshipByIdResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get specific relationship with id {relationshipId} from er collection {erCollectionIdList[i]}");
                    getNewRelationships[j].Should().BeEquivalentTo(getRelationshipByIdResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>()).ToList().FirstOrDefault());
                    var getRelationshipByIdResponseData = JObject.Parse(getRelationshipByIdResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data");
                    getRelationshipByIdResponseData[0]["id"].ToString().Should().Be(relationshipId);
                }
            }
        }

        [Then(@"validate the IMS User gets all ""(.*)"" versions of specific relationship by relationship Id from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllVersionsOfSpecificRelationshipByRelationshipIdFromTheERCollectionAsync(int versionCount)
        {
            restClientUser = "PrimaryIMSUser";

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
                    var relationshipId = getNewRelationships[j].Id;
                    Log.Information($"Attempting to get all versions of relationship by relationship Id {relationshipId} from ER Collection {erCollectionIdList[i]}");

                    var getAllVersionsOfRelationshipByIdResponseMsg = await ERService.GelAllVersionsOfRelationshipById(relationshipId, headers);
                    getAllVersionsOfRelationshipByIdResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of the relationship with id {relationshipId} from er collection {erCollectionIdList[i]}");

                    var getAllVersionsOfRelationshipByID = getAllVersionsOfRelationshipByIdResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                    var responseVersionCount = getAllVersionsOfRelationshipByID.Count();
                    //removed 1 to versionCount as after bug fix the response does not contain original relationship in addition to versioned relationships
                    responseVersionCount.Should().Be(versionCount); 
                    var response = JObject.Parse(getAllVersionsOfRelationshipByIdResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data");
                    for (int k = 0; k < responseVersionCount; k++)
                    {
                        var id = response[k]["id"];
                        id.ToString().Should().Contain(relationshipId);
                    }
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" version of specific relationship by relationship Id and ""(.*)"" when page size is ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsVersionOfSpecificRelationshipByRelationshipIdAndWhenPageSizeIs(int versionCount, string continuationToken, string pageSize)
        {
            restClientUser = "PrimaryIMSUser";

            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>("PageSize", pageSize)
                };
                for (int j = 0; j < getNewRelationships.Count; j++)
                {
                    var relationshipId = getNewRelationships[j].Id;
                    Log.Information($"Attempting to get all versions of relationship by relationship Id {relationshipId} from ER Collection {erCollectionIdList[i]}");

                    var getAllVersionsOfRelationshipByIdResponseMsg = await ERService.GelAllVersionsOfRelationshipById(relationshipId, headers);
                    getAllVersionsOfRelationshipByIdResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of the relationship with id {relationshipId} from er collection {erCollectionIdList[i]}");

                    var getAllVersionsOfRelationshipByID = getAllVersionsOfRelationshipByIdResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                    var responseVersionCount = getAllVersionsOfRelationshipByID.Count();

                    var response = getAllVersionsOfRelationshipByIdResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    continuationToken = (string)o.SelectToken("paging.continuationToken");
                    scenarioContext.Set(continuationToken, "PageContinuationToken");
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" versions of a specific relationship by relationship Id using the ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsVersionsOfASpecificRelationshipByRelationshipIdUsingThe(int versionCount, string continuationToken)
        {
            restClientUser = "PrimaryIMSUser";

            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var pageContinuationToken = scenarioContext.Get<string>("PageContinuationToken");

                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                new KeyValuePair<string, string>(continuationToken, pageContinuationToken)
            };

                for (int j = 0; j < getNewRelationships.Count; j++)
                {
                    var relationshipId = getNewRelationships[j].Id;
                    Log.Information($"Attempting to get all versions of relationship by relationship Id {relationshipId} from ER Collection {erCollectionIdList[i]}");

                    var getAllVersionsOfRelationshipByIdResponseMsg = await ERService.GelAllVersionsOfRelationshipById(relationshipId, headers);
                    getAllVersionsOfRelationshipByIdResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of the relationship with id {relationshipId} from er collection {erCollectionIdList[i]}");

                    var getAllVersionsOfRelationshipByID = getAllVersionsOfRelationshipByIdResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                    var responseVersionCount = getAllVersionsOfRelationshipByID.Count();
                    var response = getAllVersionsOfRelationshipByIdResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    continuationToken = (string)o.SelectToken("paging.continuationToken");
                    Assert.AreEqual(continuationToken, null);
                }
            }
        }

        [When(@"the IMS User updates the relationship\(s\) where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserPartiallyUpdatesThePostedRelationshipsWhereIsAndIsInTheERCollection(string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string version)
        {

            restClientUser = "PrimaryIMSUser";

            var getCreatedRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue),
                    new KeyValuePair<string, string>(allowPartialUpdateHeader, allowPartialUpdateValue)
                };

                List<Relationship> partialUpdateRelationshipsList = new List<Relationship>();
                for (int j = 0; j < getCreatedRelationships.Count; j++)
                {
                    Log.Information($"Attempting to Partially update relationship from ER Collection {erCollectionIdList[i]}");
                    var newRelationship = new Relationship(id: getCreatedRelationships[j].Id, relationshipType: RelationshipTypes.BrickRelationshipHasPartType, relationshipName: "test relationship updated", sourceEntityId: getCreatedRelationships[j].SourceEntityId, destinationEntityId: getCreatedRelationships[j].DestinationEntityId);
                    partialUpdateRelationshipsList.Add(newRelationship);
                }

                var partialUpdateRelationshipsResponseMsg = await ERService.PostRelationship(version, partialUpdateRelationshipsList, headers);

                if (asyncValue == "true")
                    partialUpdateRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update relationship in er collection {erCollectionIdList[i]}");
                else
                {
                    partialUpdateRelationshipsResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update relationship in er collection {erCollectionIdList[i]}");
                    var response = partialUpdateRelationshipsResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    var relationshipId = (string)(o.SelectToken("data.responses.success").First["id"]);
                    Assert.AreEqual(getCreatedRelationships[i].Id, relationshipId);
                }
            }
        }

        [Then(@"validate the IMS User updates the relationship\(s\)")]
        public async Task ThenValidateTheIMSUserUpdatesThePostedRelationships()
        {
            restClientUser = "PrimaryIMSUser";
            var getCreatedRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {

                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
            };

                Log.Information($"Attempting to get all relationships from ER Collection {erCollectionIdList[i]}");
                var getAllRelationshipsResponseMsg = await ERService.GetAllRelationships(headers);
                getAllRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all relationships from er collection {erCollectionIdList[i]}");
                var getAllRelationships = getAllRelationshipsResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                getAllRelationships.Count().Should().Be(getCreatedRelationships.Count());
                for (int j = 0; j < getCreatedRelationships.Count; j++)
                {
                    getAllRelationships.ToList()[j].Should().NotBe(getCreatedRelationships[j]);
                }

            }
        }

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewRelationshipSBetweenTheEntitiesWhereIsAndIsUsingVInAnERCollectionTimeS(int numOfRelationships, string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string version, int numOfTimes)

        {
            restClientUser = "PrimaryIMSUser";
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
            for (int k = 0; k < erCollectionIdList.Count; k++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[k]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue),
                    new KeyValuePair<string, string>(allowPartialUpdateHeader, allowPartialUpdateValue)
                };
                var getRelationships = scenarioContext.CreatedRelationships().ToList();

                Log.Information($"Attempting to post new Relationship/Relationships between Entities synchronously in ER collection {erCollectionIdList[k]}");
                for (int j = 0; j < numOfTimes; j++)
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

        [When(@"the IMS User updates the relationship ID where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserUpdatesTheRelationshipIDWhereIsAndIsUsingVInTheERCollection(string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string version)
        {
            restClientUser = "PrimaryIMSUser";

            var getCreatedRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue),
                    new KeyValuePair<string, string>(allowPartialUpdateHeader, allowPartialUpdateValue)
                };

                List<Relationship> partialUpdateRelationshipsList = new List<Relationship>();
                for (int j = 0; j < getCreatedRelationships.Count; j++)
                {

                    Log.Information($"Attempting to Partially update relationship from ER Collection {erCollectionIdList[i]}");

                    var newRelationship = new Relationship(id: getCreatedRelationships[j].Id, relationshipType: null, relationshipName: null, sourceEntityId: null, destinationEntityId: null);
                    partialUpdateRelationshipsList.Add(newRelationship);
                }

                var partialUpdateRelationshipsResponseMsg = await ERService.PostRelationship(version, partialUpdateRelationshipsList, headers);

                if (asyncValue == "true")
                    partialUpdateRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update relationship in er collection {erCollectionIdList[i]}");
                else
                {
                    partialUpdateRelationshipsResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update relationship in er collection {erCollectionIdList[i]}");
                    var response = partialUpdateRelationshipsResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    var relationshipId = (string)(o.SelectToken("data.responses.success").First["id"]);
                    Assert.AreEqual(getCreatedRelationships[i].Id, relationshipId);
                }
            }
        }


        [Then(@"validate the IMS User gets all relationships from source ER Collection\(s\)")]
        public async Task ThenValidateTheIMSUserGetsAllRelationshipsAcrossERCollectionsFromTheThERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionRelationshipsPairs = scenarioContext.AllERCollectionToRelationshipsPairs().ToList();

            for (int k = 0; k < erCollectionRelationshipsPairs.Count; k++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionRelationshipsPairs[k].Key)
                };

                Log.Information($"Attempting to get all relationships from ER Collection {erCollectionRelationshipsPairs[k].Key}");
                var getAllRelationshipsResponseMsg = await ERService.GetAllRelationships(headers);
                getAllRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all relationships from er collection {erCollectionRelationshipsPairs[k].Key}");
                var getAllRelationships = getAllRelationshipsResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                getAllRelationships.Should().BeEquivalentTo(erCollectionRelationshipsPairs[k].Value);
                getAllRelationships.Count().Should().Be(erCollectionRelationshipsPairs[k].Value.Count());
            }
        }


        [Then(@"IMS User deletes the relationship\(s\) from source ER Collection\(s\)")]
        public async Task ThenIMSUserDeletesTheRelationshipSAcrossAllTheERCollections()
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionRelationshipsPairs = scenarioContext.AllERCollectionToRelationshipsPairs().ToList();
            for (int k = 0; k < erCollectionRelationshipsPairs.Count; k++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionRelationshipsPairs[k].Key)
                };

                var getNewRelationships = erCollectionRelationshipsPairs[k].Value.ToList();
                for (int j = 0; j < getNewRelationships.Count; j++)
                {
                    Log.Information($"Attempting to delete relationship from ER Collection {erCollectionRelationshipsPairs[k].Key}");
                    var deleteARelationshipResponseMsg = await ERService.DeleteRelationship(getNewRelationships[j].Id, headers);
                    deleteARelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to delete relationship {getNewRelationships[j].Id} from er collection {erCollectionRelationshipsPairs[k].Key}");
                }
            }
        }

        [Then(@"validate IMS User deletes the relationship\(s\) from source ER Collection\(s\)")]
        public async Task ThenValidateIMSUserDeletesTheRelationshipSAcrossAllTheERCollections()
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionRelationshipsPairs = scenarioContext.AllERCollectionToRelationshipsPairs().ToList();

            for (int k = 0; k < erCollectionRelationshipsPairs.Count; k++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionRelationshipsPairs[k].Key)
            };
                Log.Information($"Attempting to get all relationships from ER Collection {erCollectionRelationshipsPairs[k].Key}");
                var getAllRelationshipsResponseMsg = await ERService.GetAllRelationships(headers);
                getAllRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all relationships from er collection {erCollectionRelationshipsPairs[k].Key}");
                var getAllRelationships = getAllRelationshipsResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                getAllRelationships.Count().Should().Be(0);
            }
        }

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities across the ERCollections where ""(.*)"" is ""(.*)"" using V(.*)")]
        public async Task WhenTheIMSUserPostsNewRelationshipSBetweenTheEntitiesBelongingToDifferentERCollectionsWhereIsUsingVInThERCollectionTimeS(int numOfRelationships, string asyncHeader, string asyncValue, string version)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfRelationships; i++)
            {
                var erCollectionEntitiesPairList = scenarioContext.AllERCollectionToEntitiesPairs().ToList();

                var sourceERCollectionId = erCollectionEntitiesPairList[i].Key;
                var NewSourceEntities = erCollectionEntitiesPairList[i].Value;

                var destinationERCollectionId = erCollectionEntitiesPairList[i + 1].Key;
                var NewDestinationEntities = erCollectionEntitiesPairList[i + 1].Value;

                for (int j = 0; j < NewSourceEntities.Count; j++)
                {
                    List<Relationship> relationshipsToCreate = new List<Relationship>();
                    var newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: RelationshipTypes.BrickRelationshipHasPartType, relationshipName: "test relationship", sourceEntityId: NewSourceEntities[j].Id, destinationEntityId: NewDestinationEntities[j].Id)
                    {
                        SourceERCollectionId = sourceERCollectionId,
                        DestinationERCollectionId = destinationERCollectionId
                    };

                    relationshipsToCreate.Add(newRelationship);
                    scenarioContext.AddERCollectionToRelationshipsPair(new KeyValuePair<string, List<Relationship>>(sourceERCollectionId, relationshipsToCreate));
                }
            }

            //Post relationships across ER Collections
            var erCollectionRelationshipsPairs = scenarioContext.AllERCollectionToRelationshipsPairs().ToList();
            for (int i = 0; i < erCollectionRelationshipsPairs.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionRelationshipsPairs[i].Key),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };
                var getRelationships = erCollectionRelationshipsPairs[i].Value;

                Log.Information($"Attempting to post new Relationship(s) between Entities in ER collection {erCollectionRelationshipsPairs[i].Key}");
                for (int j = 0; j < numOfRelationships; j++)
                {
                    var postRelationshipResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);
                    if (asyncValue.Equals("true"))
                    {
                        postRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new Relationship in er collection {erCollectionRelationshipsPairs[i].Key}");
                        Thread.Sleep(50000);
                    }
                    else
                    {
                        postRelationshipResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new relationship in er collection {erCollectionRelationshipsPairs[i].Key}");
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

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) of type\(s\) '(.*)' between the entities where ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection using ER API")]
        public async Task WhenTheIMSUserPostsNewRelationshipSOfTypeSBetweenTheEntitiesWhereIsUsingVInTheERCollectionUsingERAPI(int numOfRelationships, List<String> relationshipTypes, string asyncHeader, string asyncValue, string version)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfRelationships; i++)
            {
                int index = i;
                var NewEntities = scenarioContext.CreatedEntities().ToList();
                while (NewEntities.Count <= index + 1)
                    index = index + 1 - NewEntities.Count;

                var sourceEntityId = NewEntities[index].Id;
                var destinationEntityId = NewEntities[index + 1].Id;
                var newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: relationshipTypes[i], relationshipName: "test relationship", sourceEntityId: sourceEntityId, destinationEntityId: destinationEntityId);
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

                var postRelationshipResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);
                if (asyncValue.Equals("true"))
                {
                    postRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new Relationship in er collection {erCollectionIdList[k]}");
                    Thread.Sleep(50000);
                }
                else
                {
                    postRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new relationship in er collection {erCollectionIdList[k]}");
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
}
