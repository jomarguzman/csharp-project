
using Apollo.ERService.SpecflowTests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollo.ERService.SpecflowTests.Constants
{
	class GeoBoundariesConstants
	{
		public static List<GeoBoundary> GeoBoundaryList = new List<GeoBoundary>()
		{
			new GeoBoundary()
			{
				Type = "FeatureCollection",
                Features = new List<BoundaryFeature>()
                {
                    new BoundaryFeature()
                    {
                        Type = "Feature",
                        Properties = new Properties(),
                        Geometry = new BoundaryGeometry()
                        {
                            Type = "Polygon",
                            Coordinates = new List<List<List<float>>>()
                            {
                                new List<List<float>>()
                                {
                                    new List<float>()
                                    {
                                        -109.05f,
                                        41f
                                    },
                                    new List<float>()
                                    {
                                        -102.06f,
                                        40.99f
                                    },
                                    new List<float>()
                                    {
                                        -102.03f,
                                        36.99f
                                    },
                                    new List<float>()
                                    {
                                        -109.04f,
                                        36.99f
                                    },
                                    new List<float>()
                                    {
                                        -109.05f,
                                        41f
                                    }
                                }
                            }
                        }
                    }
                }
            },

            new GeoBoundary()
            {
                Type = "FeatureCollection",
                Features = new List<BoundaryFeature>()
                {
                    new BoundaryFeature()
                    {
                        Type = "Feature",
                        Properties = new Properties(),
                        Geometry = new BoundaryGeometry()
                        {
                            Type = "Polygon",
                            Coordinates = new List<List<List<float>>>()
                            {
                                new List<List<float>>()
                                {
                                    new List<float>()
                                    {
                                        31.8f,
                                        -5f
                                    },
                                    new List<float>()
                                    {
                                        32f,
                                        -5f
                                    },
                                    new List<float>()
                                    {
                                        32f,
                                        -4.7f
                                    },
                                    new List<float>()
                                    {
                                        31.8f,
                                        -4.7f
                                    },
                                    new List<float>()
                                    {
                                        31.8f,
                                        -5f
                                    }
                                }
                            }
                        }
                    }
                }
            },

            new GeoBoundary()
            {
                Type = "FeatureCollection",
                Features = new List<BoundaryFeature>()
                {
                    new BoundaryFeature()
                    {
                        Type = "Feature",
                        Properties = new Properties(),
                        Geometry = new BoundaryGeometry()
                        {
                            Type = "Polygon",
                            Coordinates = new List<List<List<float>>>()
                            {
                                new List<List<float>>()
                                {
                                    new List<float>()
                                    {
                                        31.8f,
                                        -5f
                                    },
                                    new List<float>()
                                    {
                                        32f,
                                        -5f
                                    },
                                    new List<float>()
                                    {
                                        32f,
                                        -4.7f
                                    },
                                    new List<float>()
                                    {
                                        31.8f,
                                        -4.7f
                                    },
                                    new List<float>()
                                    {
                                        31.8f,
                                        -5f
                                    }
                                }
                            }
                        }
                    }

                }
            }
        };
    }
}
