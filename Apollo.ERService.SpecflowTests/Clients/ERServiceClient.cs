/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Models;
using Jci.Be.Data.Identity.HttpClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apollo.ERService.SpecflowTests.Clients
{
    public class ERServiceClient : ServiceClient
    {
        private string collectionsBaseRoute, entitiesBaseRoute, relationshipBaseRoute, erCollectionsBaseRoute, entitiesBasetypeRoute, relationshipBasetypeRoute;
        private string statusBaseRoute, refreshEntitySchemaBaseRoute, refreshRelationshipsSchemaBaseRoute;
        
        public ERServiceClient(IRestClient restClient) : base(restClient)
        {
            SetRoutes();
        }
        private void SetRoutes()
        {
            entitiesBaseRoute = $"/er/entities";
            relationshipBaseRoute = $"/er/relationships";
            entitiesBasetypeRoute = $"/er/entitytypes";
            relationshipBasetypeRoute = $"/er/relationshiptypes";
            erCollectionsBaseRoute = $"/er/ercollections";
            statusBaseRoute = $"/er/status";
            refreshEntitySchemaBaseRoute = $"/er/refreshentityschema";
            refreshRelationshipsSchemaBaseRoute = $"/er/refreshrelationshipschema";
            collectionsBaseRoute = $"collections";
        }

        public async Task<RestClientResponse<PlatformItemResponse>> PostCollections(Collection collections, List<KeyValuePair<string, string>> headers = null)
        {
            var postCollectionsResponse = await HttpPost<Collection, PlatformItemResponse>(collectionsBaseRoute, collections, headers: headers);
            return postCollectionsResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetAllEntities(IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var getAllEntitiesResponse = await HttpGet<PlatformCollectionResponse>(entitiesBaseRoute, headers: headers);
            return getAllEntitiesResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetAllEntitiesByEntityType(string entityType, bool includeSystemData = false, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var queryParameters = new List<KeyValuePair<string, string>>();
            if (includeSystemData)
            {
                queryParameters.Add(new KeyValuePair<string, string>("includeSystemData", includeSystemData.ToString()));
            }
            var route = $"{entitiesBasetypeRoute}/{entityType}/entities";
            var getAllEntitiesByEntityTypeResponse = await HttpGet<PlatformCollectionResponse>(route, parameters: queryParameters, headers: headers);
            return getAllEntitiesByEntityTypeResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetAllVersionsOfEntityByEntityType(string entityType, string entityId, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var route = $"{entitiesBasetypeRoute}/{entityType}/entities/{entityId}/versions";
            var getAllVersionsOfEntityByEntityTypeResponse = await HttpGet<PlatformCollectionResponse>(route, headers: headers);
            return getAllVersionsOfEntityByEntityTypeResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetEntityById(string entityID, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var route = $"{entitiesBaseRoute}/{entityID}";
            var getEntityByIdResponse = await HttpGet<PlatformCollectionResponse>(route, headers: headers);
            return getEntityByIdResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetEntitiesByMultipleIds(string entityIDs, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var route = $"{entitiesBaseRoute}({entityIDs})";
            var getMultipleEntitiesByIdResponse = await HttpGet<PlatformCollectionResponse>(route, headers: headers);
            return getMultipleEntitiesByIdResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GelAllVersionsOfEntityById(string entityID, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var route = $"{entitiesBaseRoute}/{entityID}/versions";
            var getAllVersionsOfEntitiesByIdResponse = await HttpGet<PlatformCollectionResponse>(route, headers: headers);
            return getAllVersionsOfEntitiesByIdResponse;
        }
        public async Task<RestClientResponse<PlatformCollectionResponse>> GetAllRelationships(IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var getAllRelationshipsResponse = await HttpGet<PlatformCollectionResponse>(relationshipBaseRoute, headers: headers);
            return getAllRelationshipsResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetRelationshipById(string relationshipId, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var route = $"{relationshipBaseRoute}/{relationshipId}";
            var getRelationshipByIdResponse = await HttpGet<PlatformCollectionResponse>(route, headers: headers);
            return getRelationshipByIdResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GelAllVersionsOfRelationshipById(string relationshipId, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var route = $"{relationshipBaseRoute}/{relationshipId}/versions";
            var getAllVersionOfRelationshipResponse = await HttpGet<PlatformCollectionResponse>(route, headers: headers);
            return getAllVersionOfRelationshipResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetAllVersionsOfRelshipsByRelshipType(string relationshipId, string relationshipType, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var route = $"{relationshipBasetypeRoute}/{relationshipType}/relationships/{relationshipId}/versions";
            var getAllRelshipByRelshpTypeResponse = await HttpGet<PlatformCollectionResponse>(route, headers: headers);
            return getAllRelshipByRelshpTypeResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetAllRelshipsByRelshipType(string relationshipType, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            //var getAllRelshipByRelshpTypeResponse = await HttpGet<PlatformCollectionResponse>($"relationshiptypes/{relationshipType}/relationships", headers: headers);
            //return getAllRelshipByRelshpTypeResponse;
            var route = $"{relationshipBasetypeRoute}/{relationshipType}/relationships";
            var getAllRelshipByRelshpTypeResponse = await HttpGet<PlatformCollectionResponse>(route, headers: headers);
            return getAllRelshipByRelshpTypeResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetStatsByERCollection(string erCollectionId, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var getStatsByERCollectionResponse = await HttpGet<PlatformCollectionResponse>($"{erCollectionsBaseRoute}/{erCollectionId}/stats", headers: headers);
            return getStatsByERCollectionResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetApiStatus()
        {
            var getApiStatus = await HttpGet<PlatformCollectionResponse>(statusBaseRoute);
            return getApiStatus;
        }

        public async Task<RestClientResponse<PlatformItemResponse>> PostEntities(string v, IEnumerable<Entity> entities, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var queryParameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("v", v)
            };
            var postEntitiesResponse = await HttpPost<IEnumerable<Entity>, PlatformItemResponse>(entitiesBaseRoute, entities, parameters: queryParameters, headers: headers);
            return postEntitiesResponse;
        }

        public async Task<RestClientResponse<PlatformItemResponse>> PostRelationship(string v, IEnumerable<Relationship> relationships, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var queryParameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("v", v)
            };
            var postRelationshipsResponse = await HttpPost<IEnumerable<Relationship>, PlatformItemResponse>(relationshipBaseRoute, relationships, parameters: queryParameters, headers: headers);
            return postRelationshipsResponse;
        }

        public async Task<RestClientResponse<PlatformItemResponse>> CopyERCollections(IEnumerable<CopyERCollections> body)
        {
            var route = $"{erCollectionsBaseRoute}";
            var getCopyERCollections = await HttpPost<IEnumerable<CopyERCollections>, PlatformItemResponse>(route, body);
            return getCopyERCollections;
        }

        public async Task<RestClientResponse<PlatformItemResponse>> DeleteEntity(string entityId, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var deleteEntityRoute = $"{entitiesBaseRoute}/{entityId}";
            var deleteEntityResponse = await HttpDelete<PlatformItemResponse>(deleteEntityRoute, headers: headers);
            return deleteEntityResponse;
        }

        public async Task<RestClientResponse<PlatformItemResponse>> DeleteRelationship(string relationshipId, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var deleteRelationshipRoute = $"{relationshipBaseRoute}/{relationshipId}";
            var delRelshipResponse = await HttpDelete<PlatformItemResponse>(deleteRelationshipRoute, headers: headers);
            return delRelshipResponse;
        }

        public async Task<RestClientResponse<PlatformItemResponse>> RefreshEntitySchema(string entityType, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var refreshEntitySchemaRoute = $"{refreshEntitySchemaBaseRoute}/{entityType}";
            var refreshEntitySchemaResponse = await HttpPut<IEnumerable<string>, PlatformItemResponse>(refreshEntitySchemaRoute, null, headers: headers);
            return refreshEntitySchemaResponse;
        }

        public async Task<RestClientResponse<PlatformItemResponse>> RefreshRelationshipSchema(string relationshipType, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var refreshRelationshipSchemaRoute = $"{refreshRelationshipsSchemaBaseRoute}/{relationshipType}";
            var refreshRelationshipSchemaResponse = await HttpPut<IEnumerable<string>, PlatformItemResponse>(refreshRelationshipSchemaRoute, null, headers: headers);
            return refreshRelationshipSchemaResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetEntitiesByLabel(string label, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var getEntitiesByLabelResponse = await HttpGet<PlatformCollectionResponse>($"{entitiesBaseRoute}?$filter=labels/any(d:d eq '{label}')", headers: headers);
            return getEntitiesByLabelResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetEntitiesByMultipleLabelsWithORCondition(List<string> labels, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var labelsQuery = $"{entitiesBaseRoute}?$filter=";
            foreach (var label in labels)
            {
                labelsQuery = labelsQuery + $"labels/any(d:d eq '{label}') or ";
            }
            //remove last 'or' condition
            labelsQuery = labelsQuery.Remove(labelsQuery.Length - 3);

            var getEntitiesByMultipleLabelsWithORConditionResponse = await HttpGet<PlatformCollectionResponse>($"{labelsQuery}", headers: headers);
            return getEntitiesByMultipleLabelsWithORConditionResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetEntitiesByMultipleLabelsWithANDCondition(List<string> labels, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var labelsQuery = $"{entitiesBaseRoute}?$filter=";
            foreach (var label in labels)
            {
                labelsQuery = labelsQuery + $"labels/any(d:d eq '{label}') and ";
            }
            //remove last 'and' condition
            labelsQuery = labelsQuery.Remove(labelsQuery.Length - 4);

            var getEntitiesByMultipleLabelsWithANDConditionResponse = await HttpGet<PlatformCollectionResponse>($"{labelsQuery}", headers: headers);
            return getEntitiesByMultipleLabelsWithANDConditionResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetEntitiesWhoseGeoLocationMatchesExactlyWithTheQueryPoint(string queryPoint, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var queryParameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("$filter", $"geolocation eq Point({queryPoint})")
            };
            var getAllEntitiesResponse = await HttpGet<PlatformCollectionResponse>(entitiesBaseRoute, parameters: queryParameters, headers: headers);
            return getAllEntitiesResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetEntitiesWhoseGeoBoundaryContainQueryPoint(string queryPoint, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var queryParameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("$filter", $"intersects(geoBoundary, Point({queryPoint}))")
            };
            var getAllEntitiesResponse = await HttpGet<PlatformCollectionResponse>(entitiesBaseRoute, parameters: queryParameters, headers: headers);
            return getAllEntitiesResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetEntitiesWhoseGeoLocationPointIsContainedWithinTheQueryPolygon(string queryPolygon, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var queryParameters = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("$filter", $"intersects(geoLocation, Polygon({queryPolygon}))")
            };
            var getAllEntitiesResponse = await HttpGet<PlatformCollectionResponse>(entitiesBaseRoute, parameters: queryParameters, headers: headers);
            return getAllEntitiesResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetFileByFileNameOrID(string fileNameOrID, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var getFileByFileNameResponse = await HttpGet<PlatformCollectionResponse>($"{entitiesBaseRoute}/filename/{fileNameOrID}", headers: headers);
            return getFileByFileNameResponse;
        }
       
        public async Task<RestClientResponse<PlatformCollectionResponse>> GetAllAssets(IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var queryParameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("$filter", $"rootEntityType eq 'Root__Asset'")
            };
            var getAllAssetsResponse = await HttpGet<PlatformCollectionResponse>(entitiesBaseRoute, parameters: queryParameters, headers: headers);
            return getAllAssetsResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetAllSpaces(IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var queryParameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("$filter", $"rootEntityType eq 'Root__Location'")
            };
            var getAllSpacesResponse = await HttpGet<PlatformCollectionResponse>(entitiesBaseRoute, parameters: queryParameters, headers: headers);
            return getAllSpacesResponse;
        }
    }
}