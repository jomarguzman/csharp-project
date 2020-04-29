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
    public class RelationshipTypeManagement
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser, pageContinuationToken;

        public RelationshipTypeManagement(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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

        [Then(@"validate the IMS User gets all relationships of specific relationship type from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllRelationshipsOfSpecificRelationshipTypeFromTheERCollection()
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
                    Log.Information($"Attempting to get all relationship from ER Collection {erCollectionIdList[i]}");
                    var relationshipType = getNewRelationships[j].RelationshipType.ToString();
                    var getAllRelationshipResponseMsg = await ERService.GetAllRelshipsByRelshipType(relationshipType, headers: headers);
                    getAllRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all Relation with Relation Tpe from er collection {erCollectionIdList[i]}");

                    var getAllEntities = getAllRelationshipResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                    getAllEntities.Should().BeEquivalentTo(getNewRelationships);
                    getAllEntities.Count().Should().Be(getNewRelationships.Count());
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" relationship\(s\) and ""(.*)"" when page size is ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsRelationshipSAndWhenPageSizeIs(int numOfRelationships, string continuationToken, string pageSize)
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
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>("PageSize", pageSize)
                };
                Log.Information($"Attempting to get specific relationship by relationship type from the ER Collection {erCollectionIdList[i]}");
                var relationshipType = getNewRelationships[0].RelationshipType.ToString();
                var getAllRelationshipResponseMsg = await ERService.GetAllRelshipsByRelshipType(relationshipType, headers);
                getAllRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all relationships with relationship type from er collection {erCollectionIdList[i]}");
                var response = getAllRelationshipResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                JObject o = JObject.Parse(response);
                continuationToken = (string)o.SelectToken("paging.continuationToken");
                pageContinuationToken = continuationToken;
                var getAllRelationships = getAllRelationshipResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>()).ToList();
                foreach (var relship in getAllRelationships)
                {
                    getNewRelationships.Should().Contain(r => r.Id == relship.Id);
                }
                getAllRelationships.Count().Should().Be(numOfRelationships);
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" relationship\(s\) using the ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsRelationshipSUsingThe(int numOfRelationships, string continuationToken)
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
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(continuationToken, pageContinuationToken)
                };

                for (int j = 0; j < getNewRelationships.Count; j++)
                {
                    Log.Information($"Attempting to get specific relationship by relationship type from the ER Collection {erCollectionIdList[i]}");
                    var relationshipType = getNewRelationships[j].RelationshipType.ToString();
                    var getAllRelationshipResponseMsg = await ERService.GetAllRelshipsByRelshipType(relationshipType, headers);
                    getAllRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities with Entity Tpe from er collection {erCollectionIdList[i]}");
                    var getAllRelationships = getAllRelationshipResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>()).ToList();
                    foreach (var relship in getAllRelationships)
                    {
                        getNewRelationships.Should().Contain(r => r.Id == relship.Id);
                    }
                    getAllRelationships.Count().Should().Be(numOfRelationships);
                }
            }
        }

        [Then(@"validate the IMS User gets all ""(.*)"" versions of specific relationship by relationship Type from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllVersionsOfSpecificRelationshipByRelationshipTypeFromTheERCollection(int versionCount)
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
                    Log.Information($"Attempting to get all relationship from ER Collection {erCollectionIdList[i]}");
                    var relationshipId = getNewRelationships[j].Id;
                    var relationshipType = getNewRelationships[j].RelationshipType.ToString();
                    var getAllVersionsOfRelshipsByRelshipTypeResponseMsg = await ERService.GetAllVersionsOfRelshipsByRelshipType(relationshipId, relationshipType, headers: headers);
                    getAllVersionsOfRelshipsByRelshipTypeResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of the relationship with id {relationshipId} from er collection {erCollectionIdList[i]}");

                    var getAllVersionsOfRelationshipByType = getAllVersionsOfRelshipsByRelshipTypeResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                    var responseVersionCount = getAllVersionsOfRelationshipByType.Count();
                    //removed 1 to versionCount as after Bug fix the response does not contain orginal relationship in addition to versioned relationships
                    responseVersionCount.Should().Be(versionCount);
                    var response = JObject.Parse(getAllVersionsOfRelshipsByRelshipTypeResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("data");
                    for (int k = 0; k < responseVersionCount; k++)
                    {
                        var id = response[k]["id"];
                        id.ToString().Should().Contain(relationshipId);
                    }
                }
            }

        }

        [Then(@"validate the IMS User gets ""(.*)"" version of specific relationship by relationship type and ""(.*)"" when page size is ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsVersionOfSpecificRelationshipByRelationshipTypeAndWhenPageSizeIs(int versionCount, string continuationToken, string pageSize)
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
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>("PageSize", pageSize)
                };
                for (int j = 0; j < getNewRelationships.Count; j++)
                {
                    Log.Information($"Attempting to get all relationship from ER Collection {erCollectionIdList[i]}");
                    var relationshipId = getNewRelationships[j].Id;
                    var relationshipType = getNewRelationships[j].RelationshipType.ToString();
                    var getAllVersionsOfRelshipsByRelshipTypeResponseMsg = await ERService.GetAllVersionsOfRelshipsByRelshipType(relationshipId, relationshipType, headers: headers);
                    getAllVersionsOfRelshipsByRelshipTypeResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of the relationship with id {relationshipId} from er collection {erCollectionIdList[i]}");

                    var getAllVersionsOfRelationshipByType = getAllVersionsOfRelshipsByRelshipTypeResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                    var responseVersionCount = getAllVersionsOfRelationshipByType.Count();
                    responseVersionCount.Should().Be(versionCount);
                    var response = getAllVersionsOfRelshipsByRelshipTypeResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    continuationToken = (string)o.SelectToken("paging.continuationToken");
                    scenarioContext.Set(continuationToken, "PageContinuationToken");
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" versions of a specific relationship by relationship type using the ""(.*)""")]
        public async Task ThenValidateTheIMSUserGetsVersionsOfASpecificRelationshipByRelationshipTypeUsingThe(int versionCount, string continuationToken)
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

                var pageContinuationToken = scenarioContext.Get<string>("PageContinuationToken");

                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                new KeyValuePair<string, string>(continuationToken, pageContinuationToken)
            };
                for (int j = 0; j < getNewRelationships.Count; j++)
                {
                    Log.Information($"Attempting to get all relationship from ER Collection {erCollectionIdList[i]}");
                    var relationshipId = getNewRelationships[j].Id;
                    var relationshipType = getNewRelationships[j].RelationshipType.ToString();
                    var getAllVersionsOfRelshipsByRelshipTypeResponseMsg = await ERService.GetAllVersionsOfRelshipsByRelshipType(relationshipId, relationshipType, headers: headers);
                    getAllVersionsOfRelshipsByRelshipTypeResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all versions of the relationship with id {relationshipId} from er collection {erCollectionIdList[i]}");

                    var getAllVersionsOfRelationshipByType = getAllVersionsOfRelshipsByRelshipTypeResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                    var responseVersionCount = getAllVersionsOfRelationshipByType.Count();
                    responseVersionCount.Should().Be(versionCount);
                    var response = getAllVersionsOfRelshipsByRelshipTypeResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject o = JObject.Parse(response);
                    continuationToken = (string)o.SelectToken("paging.continuationToken");
                    Assert.AreEqual(continuationToken, null);
                }
            }
        }

    }
}
