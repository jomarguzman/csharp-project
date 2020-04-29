/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Apollo.ERService.SpecflowTests.Models
{
    public class PlatformItemResponse
    {
        [JsonProperty("status")]
        public PlatformResponseStatus Status { get; set; }

        [JsonProperty("transactionId", NullValueHandling = NullValueHandling.Ignore)]
        public string TransactionId { get; set; }

        [JsonProperty("data")]
        public JObject Data { get; set; }

        [JsonProperty("paging", NullValueHandling = NullValueHandling.Ignore)]
        public PlatformResponsePaging Paging { get; set; }

        public PlatformItemResponse(string code = null, object data = null, string message = "")
        {
            Status = new PlatformResponseStatus(code, message);
            Data = data == null ? new JObject() : JObject.FromObject(data);
        }
    }

    public class PlatformCollectionResponse
    {
        [JsonProperty("status")]
        public PlatformResponseStatus Status { get; set; }

        [JsonProperty("transactionId", NullValueHandling = NullValueHandling.Ignore)]
        public string TransactionId { get; set; }

        [JsonProperty("data")]
        public IEnumerable<JObject> Data { get; set; }

        [JsonProperty("paging", NullValueHandling = NullValueHandling.Ignore)]
        public PlatformResponsePaging Paging { get; set; }

        public PlatformCollectionResponse(string code = null, IEnumerable<object> data = null, string message = "")
        {
            Status = new PlatformResponseStatus(code, message);
            Data = data == null ? new List<JObject>() : data.Select(x => JObject.FromObject(x));
        }
    }
    public class PlatformResponseStatus
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        public PlatformResponseStatus(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public class PlatformResponsePaging
    {
        [JsonProperty("totalCount", NullValueHandling = NullValueHandling.Include)]
        public int TotalCount { get; set; }

        [JsonProperty("continuationToken", NullValueHandling = NullValueHandling.Include)]
        public string ContinuationToken { get; set; }
    }
}
