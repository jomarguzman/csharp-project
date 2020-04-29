/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Jci.Be.Data.Apollo.Core.Models.Schema;
using Newtonsoft.Json;

namespace Apollo.ERService.SpecflowTests.Models
{
    public class Relationship : RelationshipBase
    {
        [JsonConstructor]
        public Relationship(string id, string relationshipType, string relationshipName, string sourceEntityId, string destinationEntityId)
           : base(id, relationshipType, relationshipName, sourceEntityId, destinationEntityId) { }

        [JsonProperty("sourceERCollectionId")]
        public string SourceERCollectionId { get; set; }

        [JsonProperty("destinationERCollectionId")]
        public string DestinationERCollectionId { get; set; }

        [JsonProperty("invalidProperty")]
        public string InvalidProperty { get; set; }
    }
}
