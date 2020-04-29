/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Jci.Be.Data.AccessValidation.Domain;
using Jci.Be.Data.Identity.HttpClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apollo.ERService.SpecflowTests.Clients
{
    public class ACLServiceClient : ServiceClient
    {
        private string aclBaseRoute;

        public ACLServiceClient(IRestClient restClient) : base(restClient)
        { }

        private void SetRoutes(string erCollectionId, string username, string permissions)
        {
            aclBaseRoute = $"/ercollection/{erCollectionId}/permissions/sub:user:{username}::{permissions}";
        }

        public async Task<RestClientResponse<AccessControlList>> PostSecondaryPermissions(string erCollectionId, string username, string permissions, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            SetRoutes(erCollectionId, username, permissions);
            var postSecondaryPermissionResponse = await HttpPost<AccessControlList, AccessControlList>(aclBaseRoute, null, headers: headers);
            return postSecondaryPermissionResponse;
        }

        public async Task<RestClientResponse<AccessControlList>> DeleteSecondaryPermissions(string erCollectionId, string username, string permissions, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            SetRoutes(erCollectionId, username, permissions);
            var deleteSecondaryPermissionResponse = await HttpDelete<AccessControlList>(aclBaseRoute, headers: headers);
            return deleteSecondaryPermissionResponse;
        }
    }
}
