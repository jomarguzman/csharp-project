/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Jci.Be.Data.Apollo.Core.Models.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Apollo.ERService.SpecflowTests.Models
{
    public class Entity : EntityBase
    {
        [JsonConstructor]
        public Entity(string id, string entityType, string entityName)
           : base(id, entityType, entityName) { }

        [JsonProperty("invalidProperty")]
        public string InvalidProperty { get; set; }

        [JsonProperty("labels")]
        public List<string> Labels { get; set; }

        [JsonProperty("geoBoundary", NullValueHandling = NullValueHandling.Ignore)]
        public GeoBoundary GeoBoundary { get; set; }

        [JsonProperty("geoLocation", NullValueHandling = NullValueHandling.Ignore)]
        public GeoLocation GeoLocation { get; set; }
    }
    
    public class GeoBoundary
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public List<BoundaryFeature> Features { get; set; }
    }

    public class GeoLocation
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("features", NullValueHandling = NullValueHandling.Ignore)]
        public List<LocationFeature> Features { get; set; }
    }

    public class LocationFeature
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public Properties Properties { get; set; }

        [JsonProperty("geometry", NullValueHandling = NullValueHandling.Ignore)]
        public LocationGeometry Geometry { get; set; }
    }

    public class BoundaryFeature
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public Properties Properties { get; set; }

        [JsonProperty("geometry", NullValueHandling = NullValueHandling.Ignore)]
        public BoundaryGeometry Geometry { get; set; }
    }

    public class LocationGeometry
    {

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("coordinates", NullValueHandling = NullValueHandling.Ignore)]
        public List<float> Coordinates { get; set; }

    }

    public class BoundaryGeometry
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("coordinates", NullValueHandling = NullValueHandling.Ignore)]
        public List<List<List<float>>> Coordinates { get; set; }
    }

    public class Properties
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
    }
}
