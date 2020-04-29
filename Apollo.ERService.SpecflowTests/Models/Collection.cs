using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.ERService.SpecflowTests.Models
{
    public class Collection

    {
       
        public Collection(string collectionName)
        {
            this.collectionName = collectionName;
        }

        
        public object collectionName { get; }
               
    }

    public class CollectionItem
    {


        [Description("Unique identifier of this specific request")]
        [JsonProperty("id")]
        public string Id { get; set; }
        [Description("The Collection Id of the request")]
        [JsonProperty("collectionId")]
        public string CollectionId { get; set; }
        [Description("The Collection Name of the request")]
        [JsonProperty("collectionName")]
        public string CollectionName { get; set; }
        [Description("The Valut Id of the request")]
        [JsonProperty("vaultId")]
        public string VaultId { get; set; }
        [Description("The Vault Name of the request")]
        [JsonProperty("vaultName")]
        public string VaultName { get; set; }
        [Description("The Account Id of the request")]
        [JsonProperty("accountId")]
        public string AccountId { get; set; }
        [Description("The Account Name of the request")]
        [JsonProperty("accountName")]
        public string AccountName { get; set; }
    }
}
