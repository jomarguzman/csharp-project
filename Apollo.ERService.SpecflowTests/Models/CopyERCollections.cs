/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Newtonsoft.Json;

namespace Apollo.ERService.SpecflowTests.Models
{
    public class CopyERCollections
    {
        [JsonProperty("originalERCollectionID")]
        public string SourceERCollectionID { get; set; }
        [JsonProperty("newERCollectionID")]
        public string DestinationERCollectionID { get; set; }
    }
}
