/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Models;
using Jci.Be.Data.Identity.HttpClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apollo.ERService.SpecflowTests.Clients
{
    class GraphServiceClient : ServiceClient
    {
        private string entitiesBaseRoute, relationshipBaseRoute;
        public GraphServiceClient(IRestClient restClient) : base(restClient)
        {
            SetRoutes();
        }
        private void SetRoutes()
        {
            entitiesBaseRoute = $"/graph/entities";
            relationshipBaseRoute = $"/graph/relationships";
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetAllEntities(IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var getAllEntitiesResponse = await HttpGet<PlatformCollectionResponse>(entitiesBaseRoute, headers: headers);
            return getAllEntitiesResponse;
        }

        public async Task<RestClientResponse<PlatformCollectionResponse>> GetAllRelationship(IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            var getAllRelationshipResponse = await HttpGet<PlatformCollectionResponse>(relationshipBaseRoute, headers: headers);
            return getAllRelationshipResponse;
        }
    }
}
