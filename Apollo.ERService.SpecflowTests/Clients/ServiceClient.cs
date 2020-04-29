/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Jci.Be.Data.Identity.HttpClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apollo.ERService.SpecflowTests.Clients
{
    public class ServiceClient : IRestClient
    {
        private IRestClient _restClient;
        public ServiceClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public Task<RestClientResponse<R>> HttpDelete<R>(string route, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return _restClient.HttpDelete<R>(route, parameters, headers);
        }

        public Task<RestClientResponse<R>> HttpGet<R>(string route, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return _restClient.HttpGet<R>(route, parameters, headers);
        }

        public Task<RestClientResponse<R>> HttpPatch<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            throw new NotImplementedException();
        }

        public Task<RestClientResponse<R>> HttpPost<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return _restClient.HttpPost<T, R>(route, body, parameters, headers);
        }

        public Task<RestClientResponse<R>> HttpPut<T, R>(string route, T body, IEnumerable<KeyValuePair<string, string>> parameters = null, IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            return _restClient.HttpPut<T, R>(route, body, parameters, headers);
        }
    }
}