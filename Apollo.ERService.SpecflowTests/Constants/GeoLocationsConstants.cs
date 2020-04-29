using Apollo.ERService.SpecflowTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.ERService.SpecflowTests.Constants
{
    class GeoLocationsConstants
    {
        public static List<GeoLocation> GeoLocationList = new List<GeoLocation>()
        {
            new GeoLocation()
            {
                Type = "FeatureCollection",
                Features = new List<LocationFeature>()
                {
                    new LocationFeature()
                    {
                        Type = "Feature",
                        Properties = new Properties(),
                        Geometry = new LocationGeometry()
                        {
                            Type = "Point",
                            Coordinates = new List<float>()
                            {
                                -102.06f,
                                40.99f
                            }
                        }
                    }
                }
            },
            new GeoLocation()
            {
                Type = "FeatureCollection",
                Features = new List<LocationFeature>()
                {
                    new LocationFeature()
                    {
                        Type = "Feature",
                        Properties = new Properties(),
                        Geometry = new LocationGeometry()
                        {
                            Type = "Point",
                            Coordinates = new List<float>()
                            {
                                31.6f,
                                -4.7f
                            }
                        }
                    }
                }
            },
            new GeoLocation()
            {
                Type = "FeatureCollection",
                Features = new List<LocationFeature>()
                {
                    new LocationFeature()
                    {
                        Type = "Feature",
                        Properties = new Properties(),
                        Geometry = new LocationGeometry()
                        {
                            Type = "Point",
                            Coordinates = new List<float>()
                            {
                                31.9f,
                                -4.8f
                            }
                        }
                    }
                }
            }

        };
    }
}
