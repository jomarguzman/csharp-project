/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Models;
using Jci.Be.Data.Identity.HttpClient;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Apollo.ERService.SpecflowTests.Extensions
{
    public static class ScenarioContextExtensions
    {
        public static void AddVaultId(this ScenarioContext scenarioContext, string VaultId)
        {
            scenarioContext.CreatedVaultId().Add(VaultId);
        }

        public static List<string> CreatedVaultId(this ScenarioContext scenarioContext)
        {
            string key = "createdVaultId";
            if (!scenarioContext.Keys.Contains(key))
            {
                scenarioContext.Set(new List<string>(), key);
            }
            return scenarioContext.Get<List<string>>(key);
        }

        public static void AddAccountId(this ScenarioContext scenarioContext, string AccountId)
        {
            scenarioContext.CreatedAccountId().Add(AccountId);
        }

        public static List<string> CreatedAccountId(this ScenarioContext scenarioContext)
        {
            string key = "createdAccountId";
            if (!scenarioContext.Keys.Contains(key))
            {
                scenarioContext.Set(new List<string>(), key);
            }
            return scenarioContext.Get<List<string>>(key);
        }

        public static void AddEntity(this ScenarioContext scenarioContext, Entity entity)
        {
            scenarioContext.CreatedEntities().Add(entity);
        }
        public static void RemoveEntity(this ScenarioContext scenarioContext, Entity entity)
        {
            scenarioContext.CreatedEntities().Remove(entity);
        }
        public static List<Entity> CreatedEntities(this ScenarioContext scenarioContext)
        {
            string key = "createdErEntities";
            if (!scenarioContext.Keys.Contains(key))
            {
                scenarioContext.Set(new List<Entity>(), key);
            }
            return scenarioContext.Get<List<Entity>>(key);
        }

        public static void AddRelationship(this ScenarioContext scenarioContext, Relationship relationship)
        {
            scenarioContext.CreatedRelationships().Add(relationship);
        }
        public static void RemoveRelationship(this ScenarioContext scenarioContext, Relationship relationship)
        {
            scenarioContext.CreatedRelationships().Remove(relationship);
        }
        public static List<Relationship> CreatedRelationships(this ScenarioContext scenarioContext)
        {
            string key = "createdErRelationships";
            if (!scenarioContext.Keys.Contains(key))
            {
                scenarioContext.Set(new List<Relationship>(), key);
            }
            return scenarioContext.Get<List<Relationship>>(key);
        }

        public static void AddERCollections(this ScenarioContext scenarioContext, string erCollectionId)
        {
            scenarioContext.CreatedERCollections().Add(erCollectionId);
        }
        public static void RemoveERCollections(this ScenarioContext scenarioContext, string erCollectionId)
        {
            scenarioContext.CreatedERCollections().Remove(erCollectionId);
        }
        public static List<string> CreatedERCollections(this ScenarioContext scenarioContext)
        {
            string key = "createdErCollections";
            if (!scenarioContext.Keys.Contains(key))
            {
                scenarioContext.Set(new List<string>(), key);
            }
            return scenarioContext.Get<List<string>>(key);
        }

        public static void AddRestClientExceptionErrors(this ScenarioContext scenarioContext, RestClientException response)
        {
            scenarioContext.AllRestClientExceptionErrors().Add(response);
        }
        public static void RemoveRestClientExceptionErrors(this ScenarioContext scenarioContext, RestClientException response)
        {
            scenarioContext.AllRestClientExceptionErrors().Remove(response);
        }
        public static List<RestClientException> AllRestClientExceptionErrors(this ScenarioContext scenarioContext)
        {
            string key = "createdRestClientExceptionErrors";
            if (!scenarioContext.Keys.Contains(key))
            {
                scenarioContext.Set(new List<RestClientException>(), key);
            }
            return scenarioContext.Get<List<RestClientException>>(key);
        }

        public static void AddHttpResponse(this ScenarioContext scenarioContext, RestClientResponse<PlatformCollectionResponse> response)
        {
            scenarioContext.AllHttpResponses().Add(response);
        }
        public static void RemoveHttpResponse(this ScenarioContext scenarioContext, RestClientResponse<PlatformCollectionResponse> response)
        {
            scenarioContext.AllHttpResponses().Remove(response);
        }
        public static List<RestClientResponse<PlatformCollectionResponse>> AllHttpResponses(this ScenarioContext scenarioContext)
        {
            string key = "createdResponses";
            if (!scenarioContext.Keys.Contains(key))
            {
                scenarioContext.Set(new List<RestClientResponse<PlatformCollectionResponse>>(), key);
            }
            return scenarioContext.Get<List<RestClientResponse<PlatformCollectionResponse>>>(key);
        }

        public static void AddPartialUpdateResponses(this ScenarioContext scenarioContext, RestClientResponse<PlatformItemResponse> response)
        {
            scenarioContext.AllPartialUpdateResponses().Add(response);
        }
        public static void RemovePartialUpdateResponses(this ScenarioContext scenarioContext, RestClientResponse<PlatformItemResponse> response)
        {
            scenarioContext.AllPartialUpdateResponses().Remove(response);
        }
        public static List<RestClientResponse<PlatformItemResponse>> AllPartialUpdateResponses(this ScenarioContext scenarioContext)
        {
            string key = "allPartialUpdateResponses";
            if (!scenarioContext.Keys.Contains(key))
            {
                scenarioContext.Set(new List<RestClientResponse<PlatformItemResponse>>(), key);
            }
            return scenarioContext.Get<List<RestClientResponse<PlatformItemResponse>>>(key);
        }

        public static void AddERCollectionToEntitiesPair(this ScenarioContext scenarioContext, KeyValuePair<string, List<Entity>> ERCollectionEntitiesPair)
        {
            scenarioContext.AllERCollectionToEntitiesPairs().Add(ERCollectionEntitiesPair);
        }
        public static void RemoveERCollectionToEntitiesPair(this ScenarioContext scenarioContext, KeyValuePair<string, List<Entity>> ERCollectionEntitiesPair)
        {
            scenarioContext.AllERCollectionToEntitiesPairs().Remove(ERCollectionEntitiesPair);
        }

        public static List<KeyValuePair<string, List<Entity>>> AllERCollectionToEntitiesPairs(this ScenarioContext scenarioContext)
        {
            string key = "allERCollectionToEntitiesPairs";
            if (!scenarioContext.Keys.Contains(key))
            {
                scenarioContext.Set(new List<KeyValuePair<string, List<Entity>>>(), key);
            }
            return scenarioContext.Get<List<KeyValuePair<string, List<Entity>>>>(key);
        }

        public static void AddERCollectionToRelationshipsPair(this ScenarioContext scenarioContext, KeyValuePair<string, List<Relationship>> ERCollectionEntitiesPair)
        {
            scenarioContext.AllERCollectionToRelationshipsPairs().Add(ERCollectionEntitiesPair);
        }
        public static void RemoveERCollectionToRelationshipsPair(this ScenarioContext scenarioContext, KeyValuePair<string, List<Relationship>> ERCollectionEntitiesPair)
        {
            scenarioContext.AllERCollectionToRelationshipsPairs().Remove(ERCollectionEntitiesPair);
        }

        public static List<KeyValuePair<string, List<Relationship>>> AllERCollectionToRelationshipsPairs(this ScenarioContext scenarioContext)
        {
            string key = "allERCollectionToRelationshipsPairs";
            if (!scenarioContext.Keys.Contains(key))
            {
                scenarioContext.Set(new List<KeyValuePair<string, List<Relationship>>>(), key);
            }
            return scenarioContext.Get<List<KeyValuePair<string, List<Relationship>>>>(key);
        }
    }
}
