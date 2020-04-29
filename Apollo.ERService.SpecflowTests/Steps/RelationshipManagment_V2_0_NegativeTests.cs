/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Clients;
using Apollo.ERService.SpecflowTests.TestSettings;
using System;
using TechTalk.SpecFlow;
using Jci.Be.Data.Identity.HttpClient;
using Apollo.ERService.SpecflowTests.Models;
using Jci.Be.Data.Apollo.Core.Constants;
using Apollo.ERService.SpecflowTests.Extensions;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Apollo.ERService.SpecflowTests.Constants;
using Jci.Be.Data.Identity.HttpClient.TokenProvider;
using Jci.Be.Data.Identity.HttpClient.Policies;

namespace Apollo.ERService.SpecflowTests.Steps
{
    [Binding]
    public class RelationshipManagment_V2_0_NegativeTests
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public RelationshipManagment_V2_0_NegativeTests(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" without ""(.*)"" property using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserPostsNewRelationshipSBetweenTheEntitiesWhereIsWithoutPropertyUsingVInTheERCollectionTimeS(int numOfRelationship, string asyncHeader, string asyncValue, string missingField, string version)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfRelationship; i++)
            {
                int index = i;
                var newEntities = scenarioContext.CreatedEntities().ToList();
                while (newEntities.Count <= index + 1)
                    index = index + 1 - newEntities.Count;
                var sourceEntityId = newEntities[index].Id;
                var destinationEntityId = newEntities[index + 1].Id;
                Relationship newRelationship = null;
                {
                    if (missingField.Equals("relationshipType"))
                        newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: null, relationshipName: "test relationship", sourceEntityId: sourceEntityId, destinationEntityId: destinationEntityId);

                    if (missingField.Equals("sourceEntityId"))
                        newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: BrickRelationshipTypes.BrickEquipmentIsLocatedInLocation, relationshipName: "test relationship", sourceEntityId: null, destinationEntityId: destinationEntityId);

                    if (missingField.Equals("destinationEntityId"))
                        newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: BrickRelationshipTypes.BrickEquipmentIsLocatedInLocation, relationshipName: "test relationship", sourceEntityId: sourceEntityId, destinationEntityId: null);

                    if (missingField.Equals("relationshipName"))
                        newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: BrickRelationshipTypes.BrickEquipmentIsLocatedInLocation, relationshipName: null, sourceEntityId: sourceEntityId, destinationEntityId: destinationEntityId);

                    if (missingField.Equals("relationshipId"))
                        newRelationship = new Relationship(id: null, relationshipType: BrickRelationshipTypes.BrickEquipmentIsLocatedInLocation, relationshipName: "test relationship", sourceEntityId: sourceEntityId, destinationEntityId: destinationEntityId);
                }
                scenarioContext.AddRelationship(newRelationship);
            }
            //var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            string erCollectionId = this.scenarioContext.Get<dynamic>("CollectionsResponse");
            scenarioContext.AddERCollections(erCollectionId);
            
                   var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionId),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };
                var getRelationships = scenarioContext.CreatedRelationships().ToList();

                Log.Information($"Attempting to post new Relationship/Relationships between Entities synchronously in ER collection {erCollectionId}");

                {
                RestClientResponse<PlatformItemResponse> postRelationshipResponseMsg = null;

                //try
                //{
                postRelationshipResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);
                if (asyncValue == "true")
                    postRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update relationship in er collection {erCollectionId}");
                else
                    scenarioContext.AddPartialUpdateResponses(postRelationshipResponseMsg);
            }
                //}
                //catch (RestClientException ex)
                //{
                //    scenarioContext.AddRestClientExceptionErrors(ex);
                //}
            
        }
        [Then(@"validate IMS User cannot get the relationship\(s\) from the ER Collection")]
        public async Task ThenValidateIMSUserCannotGetTheRelationshipSFromTheERCollection()
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
                var getAllRelationshipResponseMsg = await ERService.GetAllRelationships(headers);
                getAllRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all relationship from er collection");
                var getRelationshipByIdResponseData = JObject.Parse(getAllRelationshipResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult()).SelectToken("paging");
                var getAllRelationship = getAllRelationshipResponseMsg.Result.Data.Select(s => s.ToObject<Relationship>());
                getAllRelationship.Count().Should().Be(0);
            }
        }

        [Then(@"validate IMS User cannot delete the relationship\(s\) from the ER Collection")]
        public async Task ThenValidateIMSUserCannotDeleteTheRelationshipSFromTheERCollection()
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
                    RestClientResponse<PlatformItemResponse> deleteRelationshipResponseMsg = null;
                    try
                    {
                        deleteRelationshipResponseMsg = await ERService.DeleteRelationship(getNewRelationships[j].Id, headers);
                        deleteRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.NotFound, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be not able to delete relationship {getNewRelationships[j].Id} from er collection {erCollectionIdList[i]}");
                    }
                    catch (RestClientException ex)
                    {
                        ex.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be not able to delete relationship {getNewRelationships[j].Id} from er collection {erCollectionIdList[i]}");
                    }
                }
            }
        }

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" with invalid sourceEntityId using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserPostsNewRelationshipSBetweenTheEntitiesWhereIsWithInvalidSourceEntityIdUsingVInTheERCollectionTimeS(int numOfRelationship, string asyncHeader, string asyncValue, string version)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfRelationship; i++)
            {
                int index = i;
                var NewEntities = scenarioContext.CreatedEntities().ToList();
                while (NewEntities.Count <= index + 1)
                    index = index + 1 - NewEntities.Count;

                var sourceEntityId = NewEntities[index].Id;
                var destinationEntityId = NewEntities[index + 1].Id;
                Relationship newRelationship = null;
                newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: BrickRelationshipTypes.BrickEquipmentIsLocatedInLocation, relationshipName: "test relationship", sourceEntityId: $"test-{sourceEntityId}", destinationEntityId: destinationEntityId);
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
                RestClientResponse<PlatformItemResponse> postRelationshipResponseMsg = null;
                try
                {
                    postRelationshipResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);
                }
                catch (RestClientException ex)
                {
                    scenarioContext.AddRestClientExceptionErrors(ex);
                }
            }
        }

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" with invalid destinationEntityId using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserPostsNewRelationshipSBetweenTheEntitiesWhereIsWithInvalidDestinationEntityIdUsingVInTheERCollectionTimeS(int numOfRelationship, string asyncHeader, string asyncValue, string version)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfRelationship; i++)
            {
                int index = i;
                var NewEntities = scenarioContext.CreatedEntities().ToList();
                while (NewEntities.Count <= index + 1)
                    index = index + 1 - NewEntities.Count;

                var sourceEntityId = NewEntities[index].Id;
                var destinationEntityId = NewEntities[index + 1].Id;
                Relationship newRelationship = null;
                newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: BrickRelationshipTypes.BrickEquipmentIsLocatedInLocation, relationshipName: "test relationship", sourceEntityId: sourceEntityId, destinationEntityId: $"test-{destinationEntityId}");
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
                RestClientResponse<PlatformItemResponse> postRelationshipResponseMsg = null;
                try
                {
                    postRelationshipResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);
                }
                catch (RestClientException ex)
                {
                    scenarioContext.AddRestClientExceptionErrors(ex);
                }
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" precondition failed error code for the post\(s\)")]
        public void ThenValidateTheIMSUserGetPreconditionFailedErrorCodeForThePostS(HttpStatusCode errorCode)
        {
            var getAllResponseErrors = scenarioContext.AllRestClientExceptionErrors().ToList();
            for (int i = 0; i < getAllResponseErrors.Count; i++)
            {
                getAllResponseErrors[i].Response.StatusCode.Should().Be(errorCode, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to post a new relationship without required value");
            }
        }

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" using V(.*) without the ER Collection")]
        public async Task WhenTheIMSUserPostsNewRelationshipSBetweenTheEntitiesWhereIsUsingVWithoutTheERCollectionTimeS(int numOfRelationship, string asyncHeader, string asyncValue, string version)
        {
            restClientUser = "PrimaryIMSUser";
            for (int i = 0; i < numOfRelationship; i++)
            {
                int index = i;
                var NewEntities = scenarioContext.CreatedEntities().ToList();
                while (NewEntities.Count <= index + 1)
                    index = index + 1 - NewEntities.Count;

                var sourceEntityId = NewEntities[index].Id;
                var destinationEntityId = NewEntities[index + 1].Id;
                Relationship newRelationship = null;
                newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: BrickRelationshipTypes.BrickEquipmentIsLocatedInLocation, relationshipName: "test relationship", sourceEntityId: sourceEntityId, destinationEntityId: destinationEntityId);
                scenarioContext.AddRelationship(newRelationship);
            }
            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(asyncHeader, asyncValue)
            };
            var getRelationships = scenarioContext.CreatedRelationships().ToList();

            Log.Information($"Attempting to post new Relationship/Relationships between Entities synchronously witout ER collection");
            RestClientResponse<PlatformItemResponse> postRelationshipResponseMsg = null;
            try
            {
                postRelationshipResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);
            }
            catch (RestClientException ex)
            {
                scenarioContext.AddRestClientExceptionErrors(ex);
            }
        }

        [Then(@"validate the IMS User gets ""(.*)"" Payload too large error code for the post\(s\)")]
        public void ThenValidateTheIMSUserGetsPayloadTooLargeErrorCodeForThePostS(HttpStatusCode errorCode)
        {
            var getAllResponseErrors = scenarioContext.AllRestClientExceptionErrors().ToList();
            for (int i = 0; i < getAllResponseErrors.Count; i++)
            {
                getAllResponseErrors[i].Response.StatusCode.Should().Be(errorCode, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to post more than 100 relationships in one go");
            }
        }

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection")]
        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserPostsLargeCountNewRelationshipSBetweenTheEntitiesWhereIsUsingVInTheERCollectionTimeS(int numOfRelationships, string asyncHeader, string asyncValue, string version)
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
                var newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: BrickRelationshipTypes.BrickEquipmentIsLocatedInLocation, relationshipName: "test relationship", sourceEntityId: sourceEntityId, destinationEntityId: destinationEntityId);
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
                RestClientResponse<PlatformItemResponse> postRelationshipResponseMsg = null;
                try
                {
                    postRelationshipResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);
                }
                catch (RestClientException ex)
                {
                    scenarioContext.AddRestClientExceptionErrors(ex);
                }
            }
        }

        [When(@"IMS User gets a specific relationship by relationship Id that doesn't exist from the ER Collection")]
        public async Task WhenIMSUserGetsASpecificRelationshipByRelationshipIdThatDoesnTExistFromTheERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
            };
                var wrongRelationshipId = $"wrongTest-{Guid.NewGuid()}";
                Log.Information($"Attempting to get specific Relationship by Relationship Id {wrongRelationshipId} from ER Collection {erCollectionIdList[i]}");

                var getRelationshipByIdResponseMsg = await ERService.GetRelationshipById(wrongRelationshipId, headers);
                scenarioContext.AddHttpResponse(getRelationshipByIdResponseMsg);
            }
        }

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\) with Invalid Schema")]
        public async Task WhenTheIMSUserPostsNewRelationshipSBetweenTheEntitiesWhereIsAndIsUsingVInAnERCollectionTimeSWithInvalidSchema(int numOfRelationships, string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string version, int numOfTimes)

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
                var newRelationship = new Relationship(id: $"test-{Guid.NewGuid()}", relationshipType: "InvalidRelationshipSchema", relationshipName: "test relationship", sourceEntityId: sourceEntityId, destinationEntityId: destinationEntityId);
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

                    var partialUpdateRelationshipsResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);

                    if (asyncValue == "true")
                        partialUpdateRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update relationship in er collection {erCollectionIdList[j]}");
                    else
                        scenarioContext.AddPartialUpdateResponses(partialUpdateRelationshipsResponseMsg);
                }
            }

        }


        [When(@"the IMS User updates the relationships where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" without the ""(.*)"" property using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserPartiallyUpdatesThePostedRelationshipsWhereIsAndIsWithoutThePropertyUsingVInTheERCollection(string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string propertyMissing, string version)
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

                    Relationship newRelationship = null;

                    if (propertyMissing.Equals("relationshipId"))
                        newRelationship = new Relationship(id: null, relationshipType: getCreatedRelationships[j].RelationshipType, relationshipName: getCreatedRelationships[j].RelationshipName, sourceEntityId: getCreatedRelationships[j].SourceEntityId, destinationEntityId: getCreatedRelationships[j].DestinationEntityId);

                    //if (propertyMissing.Equals("relationshipType"))
                    //    newRelationship = new Relationship(id: getCreatedRelationships[j].Id, relationshipType: null, relationshipName: getCreatedRelationships[j].RelationshipName, sourceEntityId: getCreatedRelationships[j].SourceEntityId, destinationEntityId: getCreatedRelationships[j].DestinationEntityId);

                    partialUpdateRelationshipsList.Add(newRelationship);
                }

                var partialUpdateRelationshipsResponseMsg = await ERService.PostRelationship(version, partialUpdateRelationshipsList, headers);

                if (asyncValue == "true")
                    partialUpdateRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update relationship in er collection {erCollectionIdList[i]}");
                else
                    scenarioContext.AddPartialUpdateResponses(partialUpdateRelationshipsResponseMsg);
            }
        }

        [When(@"the IMS User updates the relationships where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" with invalid property name ""(.*)"" and value ""(.*)"" in the ER Collection using V(.*)")]
        public async Task WhenTheIMSUserPartiallyUpdatesThePostedRelationshipsWhereIsAndIsWithInvalidPropertyNameAndValueInTheERCollectionUsingV(string asyncHeader, string asyncValue, string allowPartialUpdateHeader, string allowPartialUpdateValue, string invalidPropertyName, string invalidPropertyValue, string version)
        {
            restClientUser = "PrimaryIMSUser";

            var getNewRelationships = scenarioContext.CreatedRelationships().ToList();
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
                for (int j = 0; j < getNewRelationships.Count; j++)
                {
                    Log.Information($"Attempting to Partially update relationship from ER Collection {erCollectionIdList[i]}");

                    Relationship newRelationship = new Relationship(id: getNewRelationships[j].Id, relationshipType: getNewRelationships[j].RelationshipType, relationshipName: getNewRelationships[j].RelationshipName, sourceEntityId: getNewRelationships[j].SourceEntityId, destinationEntityId: getNewRelationships[j].DestinationEntityId)
                    {
                        InvalidProperty = invalidPropertyValue
                    };

                    partialUpdateRelationshipsList.Add(newRelationship);
                }

                var partialUpdateRelationshipsResponseMsg = await ERService.PostRelationship(version, partialUpdateRelationshipsList, headers);

                if (asyncValue == "true")
                    partialUpdateRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update relationship in er collection {erCollectionIdList[i]}");
                else
                    scenarioContext.AddPartialUpdateResponses(partialUpdateRelationshipsResponseMsg);

                //RestClientResponse<PlatformItemResponse> postRelationshipResponseMsg = null;
                //try
                //{
                //    postRelationshipResponseMsg = await ERService.PostRelationship(version, partialUpdateRelationshipsList, headers);
                //}
                //catch (RestClientException ex)
                //{
                //    scenarioContext.AddRestClientExceptionErrors(ex);
                //}
            }
        }


        [Then(@"validate the IMS User gets ""(.*)"" responses message ""(.*)"" while updating relationship\(s\)")]
        public void ThenValidateTheIMSUserGetsResponsesWhileUpdatingRelationships(string errorCode, string message)
        {
            var getAllResponses = scenarioContext.AllPartialUpdateResponses().ToList();
            for (int i = 0; i < getAllResponses.Count; i++)
            {
                //getAllResponses[i].Response.StatusCode.ToString().Should().Be("207", $"user with {_testCfg.ImsScopes} scopes should not be able to partial update relationship");
                //var response = JObject.Parse(getAllResponses[i].Response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                //response.SelectToken("data.responses.failure").Should().NotBeEmpty();
                getAllResponses[i].Response.StatusCode.ToString().Should().Be("207", $"user with {_testCfg.ImsScopes} scopes should not be able to update entities");
                var response = getAllResponses[i].Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                JObject o = JObject.Parse(response);

                o.SelectToken("data.responses.failure").Should().NotBeEmpty();
                var responseMessage = (string)(o.SelectToken("data.responses.failure").First["message"]);
                //Assert.AreEqual(message, responseMessage);
                Assert.IsTrue(responseMessage.Contains(message));

            }

        }

        [When(@"the IMS User updates the relationship type for the posted relationship\(s\) where ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserChangeTheRelationshipTypeForThePostedRelationshipSBetweenTheEntitiesWhereIsUsingVInTheERCollectionTimeS(string asyncHeader, string asyncValue, string version)
        {

            restClientUser = "PrimaryIMSUser";

            var getCreatedRelationships = scenarioContext.CreatedRelationships().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue)
                };

                List<Relationship> partialUpdateRelationshipsList = new List<Relationship>();
                for (int j = 0; j < getCreatedRelationships.Count; j++)
                {
                    Log.Information($"Attempting to Partially update relationship from ER Collection {erCollectionIdList[i]}");

                    var newRelationship = new Relationship(id: getCreatedRelationships[j].Id, relationshipType: BrickRelationshipTypes.BrickEquipmentIsLocatedInLocation, relationshipName: getCreatedRelationships[j].RelationshipName, sourceEntityId: getCreatedRelationships[j].SourceEntityId, destinationEntityId: getCreatedRelationships[j].DestinationEntityId);

                    partialUpdateRelationshipsList.Add(newRelationship);
                }

                var partialUpdateRelationshipsResponseMsg = await ERService.PostRelationship(version, partialUpdateRelationshipsList, headers);

                if (asyncValue == "true")
                    partialUpdateRelationshipsResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to partial update relationship in er collection {erCollectionIdList[i]}");
                else
                    scenarioContext.AddPartialUpdateResponses(partialUpdateRelationshipsResponseMsg);
            }
        }

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) between the entities where ""(.*)"" is ""(.*)"" With Invalid Token using V(.*) in the ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewRelationshipSBetweenTheEntitiesWhereIsWithInvalidTokenUsingVInTheERCollectionTimeS(int numOfRelationships, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            GenerateTokenForInvalidIMSUser();
            restClientUser = "InvalidIMSUser";
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
                    try
                    {
                        var postRelationshipResponseMsg = await ERService.PostRelationship(version, getRelationships, headers);

                    }
                    catch (RestClientException ex)
                    {
                        ex.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should not be able to post a new Relationship without token");
                    }
                }
            }
        }
    }
}
