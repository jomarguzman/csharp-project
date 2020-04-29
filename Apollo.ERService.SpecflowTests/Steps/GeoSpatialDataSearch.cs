/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Clients;
using Apollo.ERService.SpecflowTests.Constants;
using Apollo.ERService.SpecflowTests.Extensions;
using Apollo.ERService.SpecflowTests.Models;
using Apollo.ERService.SpecflowTests.TestSettings;
using FluentAssertions;
using Jci.Be.Data.Apollo.Core.Constants;
using Jci.Be.Data.Identity.HttpClient;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Apollo.ERService.SpecflowTests.Steps
{
    [Binding]
    public class GeoSpatialDataSearch
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public GeoSpatialDataSearch(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext ?? throw new ArgumentNullException("ScenarioContext is null");
            _testCfg = testConfiguration ?? throw new Exception("Something went wrong with Test Setup. The setup provides a parallel safe version of test configuration settings.");
        }

        private ERServiceClient ERService
        {
            get
            {
                if (!scenarioContext.TryGetValue(restClientUser, out IRestClient user))
                {
                    restClientUser = "PrimaryIMSUser";
                }
                var erServiceClient = new ERServiceClient(scenarioContext.Get<IRestClient>(restClientUser));
                return erServiceClient;
            }
        }

        [When(@"the IMS User posts ""(.*)"" new entities with GeoBoundary and GeoLocation where ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection ""(.*)"" time\(s\)")]
        public async Task WhenTheIMSUserPostsNewEntitiesWithGeoBoundaryAndGeoLocationWhereIsUsingVInAnERCollectionTimeS(int numOfEntities, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            restClientUser = "PrimaryIMSUser";
            List<GeoBoundary> GeoBoundaries = GeoBoundariesConstants.GeoBoundaryList;
            List<GeoLocation> GeoLocations = GeoLocationsConstants.GeoLocationList;

            for (int i = 0; i < numOfEntities; i++)
            {
                GeoBoundary boundary; GeoLocation location;
                var index = i;
                while (GeoBoundaries.Count <= index)
                    index = index - GeoBoundaries.Count;

                boundary = GeoBoundaries[index];

                index = i;

                while (GeoLocations.Count <= index)
                    index = index - GeoLocations.Count;

                location = GeoLocations[i];
                
                var newEntity = new Entity(id: $"test-{Guid.NewGuid()}", entityType: EntityTypes.EquipmentBrickEntityType, entityName: "test entity")
                {
                    GeoBoundary = boundary,
                    GeoLocation = location
                };

                scenarioContext.AddEntity(newEntity);
            }
            // var erCollectionId = "test-" + Guid.NewGuid().ToString();
            string erCollectionId = this.scenarioContext.Get<dynamic>("CollectionsResponse");
            scenarioContext.AddERCollections(erCollectionId);
            var headers = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionId),
                new KeyValuePair<string, string>(asyncHeader, asyncValue)
            };
            var getentities = scenarioContext.CreatedEntities().ToList();

            Log.Information($"Attempting to post new entity/entities synchronously in ER collection {erCollectionId}");
            for (int i = 0; i < numOfTimes; i++)
            {
                var postEntitiesResponseMsg = await ERService.PostEntities(version, getentities, headers);
                if (asyncValue == "true")
                {
                    postEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
                    Thread.Sleep(10000);
                }
                else
                {
                    postEntitiesResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionId}");
                    var response = postEntitiesResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    JObject jObjResponse = JObject.Parse(response);
                    var successEntityIDList = from entity in jObjResponse["data"]["responses"]["success"]
                                              select (string)entity["id"];
                    var getEntityIDList = from entity in getentities
                                          select entity.Id;

                    getEntityIDList.Should().BeEquivalentTo(successEntityIDList);
                    getEntityIDList.Count().Should().Be(successEntityIDList.Count());
                }
            }
        }

        [Then(@"validate the IMS User gets all entities whose GeoLocation matches exactly with the Query Point ""(.*)"" from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllEntitiesWhoseGeoLocationMatchesExactlyWithTheQueryPointFromTheERCollection(string queryPoint)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get all entities Whose GeoLocation Matches Exactly With The QueryPoint {queryPoint} from ER Collection {erCollectionIdList[i]}");

                var getAllEntitiesResponseMsg = await ERService.GetEntitiesWhoseGeoLocationMatchesExactlyWithTheQueryPoint(queryPoint, headers: headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities of specific entity type from er collection {erCollectionIdList[i]}");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());

                // Fetching expected result list
                string[] queryPointArray = queryPoint.Replace("(", "").Replace(")", "").Split(new char[] { ',' });

                var point = (from queryPoints in queryPointArray select float.Parse(queryPoints)).ToList();

                var expectedEntities = getNewEntities.FindAll(s => s.GeoLocation.Features.FirstOrDefault().Geometry.Coordinates.Except(point).Count().Equals(0));

                getAllEntities.Count().Should().Be(expectedEntities.Count());
                getAllEntities.Should().BeEquivalentTo(expectedEntities);
            }
        }

        [Then(@"validate the IMS User gets all entities whose GeoBoundary contain Query Point ""(.*)"" from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllEntitiesWhoseGeoBoundaryContainQueryPointFromTheERCollection(string queryPoint)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get all entities Whose GeoLocation Matches Exactly With The QueryPoint {queryPoint} from ER Collection {erCollectionIdList[i]}");

                var getAllEntitiesResponseMsg = await ERService.GetEntitiesWhoseGeoBoundaryContainQueryPoint(queryPoint, headers: headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities of specific entity type from er collection {erCollectionIdList[i]}");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());

                // Fetching expected result list
                string[] queryPointArray = queryPoint.Replace("(", "").Replace(")", "").Split(new char[] { ',' });

                var point = (from queryPoints in queryPointArray select float.Parse(queryPoints)).ToList();


                List<Entity> expectedEntities = new List<Entity>();

                foreach(Entity entity in getNewEntities)
                {
                    List<Point> polygonPoints = GetPolygonPointsFromGeometry(entity.GeoBoundary.Features.FirstOrDefault().Geometry.Coordinates.FirstOrDefault());

                    if (PointInPolygon(polygonPoints, point.FirstOrDefault(), point.LastOrDefault()))
                        expectedEntities.Add(entity);
                };

                getAllEntities.Count().Should().Be(expectedEntities.Count());
                getAllEntities.Should().BeEquivalentTo(expectedEntities);
                
            }
        }

        [Then(@"validate the IMS User gets all entities whose GeoLocation\(Point\) is contained within the Query Polygon ""(.*)"" from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllEntitiesWhoseGeoLocationPointIsContainedWithinTheQueryPolygonFromTheERCollection(string queryPolygon)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get all entities Whose GeoLocation Matches Exactly With The QueryPoint {queryPolygon} from ER Collection {erCollectionIdList[i]}");

                var getAllEntitiesResponseMsg = await ERService.GetEntitiesWhoseGeoLocationPointIsContainedWithinTheQueryPolygon(queryPolygon, headers: headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities of specific entity type from er collection {erCollectionIdList[i]}");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());

                // Fetching expected result list
                List<List<float>> polygon = new List<List<float>>();

                string[] queryPolygonArray = queryPolygon.Split(new string[] { ")," }, StringSplitOptions.None);
                for (int j = 0; j < queryPolygonArray.Length; j++)
                {
                    var queryPointArray = queryPolygonArray[j].Replace("(", "").Replace(")", "").Split(new char[] { ',' });
                    polygon.Add((from queryPoint in queryPointArray
                                    select float.Parse(queryPoint)).ToList());               
                }

                List<Entity> expectedEntities = new List<Entity>();

                foreach (Entity entity in getNewEntities)
                {
                    var locationPoint = entity.GeoLocation.Features.FirstOrDefault().Geometry.Coordinates;
                    List<Point> polygonPoints = GetPolygonPointsFromGeometry(polygon);

                    if (PointInPolygon(polygonPoints, locationPoint.FirstOrDefault(), locationPoint.LastOrDefault()))
                        expectedEntities.Add(entity);

                }

                getAllEntities.Count().Should().Be(expectedEntities.Count());
                getAllEntities.Should().BeEquivalentTo(expectedEntities);
            }
        }

        [Then(@"validate the IMS User does not get all entities whose GeoBoundary contain Invalid Query Point ""(.*)"" from the ER Collection")]
        public async Task ThenValidateTheIMSUserDoesNotGetAllEntitiesWhoseGeoBoundaryContainInvalidQueryPointFromTheERCollection(string queryPoint)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get all entities Whose GeoLocation contain Invalid Query Point {queryPoint} from ER Collection {erCollectionIdList[i]}");

                var getAllEntitiesResponseMsg = await ERService.GetEntitiesWhoseGeoBoundaryContainQueryPoint(queryPoint, headers: headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities of specific entity type from er collection {erCollectionIdList[i]}");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());

                getAllEntities.Count().Should().Be(0);


            }
        }

        [Then(@"validate the IMS User gets all entities whose GeoLocation does not match exactly with the Query Point ""(.*)"" from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllEntitiesWhoseGeoLocationDoesNotMatchExactlyWithTheQueryPointFromTheERCollection(string queryPoint)
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };

                Log.Information($"Attempting to get all entities Whose GeoLocation Matches Exactly With The QueryPoint {queryPoint} from ER Collection {erCollectionIdList[i]}");

                var getAllEntitiesResponseMsg = await ERService.GetEntitiesWhoseGeoLocationMatchesExactlyWithTheQueryPoint(queryPoint, headers: headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities of specific entity type from er collection {erCollectionIdList[i]}");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());
                getAllEntities.Count().Should().Be(0);
            }
        }

        // Return True if the point is in the polygon.
        List<float> m_lstCrossPoints = new List<float>();
        public bool PointInPolygon(List<Point> Points, float X, float Y)
        {
            // Get the angle between the point and the first and last vertices.
            int max_point = Points.Count - 1;
            float total_angle = GetAngle(
                Points[max_point].X, Points[max_point].Y,
                X, Y,
                Points[0].X, Points[0].Y);

            m_lstCrossPoints.Clear();
            // Add the angles from the point to each other pair of vertices.
            for (int i = 0; i < max_point; i++)
            {
                total_angle += GetAngle(
                    Points[i].X, Points[i].Y,
                    X, Y,
                    Points[i + 1].X, Points[i + 1].Y);
            }

            float total_CrossPoints = 0;
            if (m_lstCrossPoints.Count > 0)
            {
                for (int nCount = 0; nCount < m_lstCrossPoints.Count; nCount++)
                {
                    total_CrossPoints = total_CrossPoints + m_lstCrossPoints[nCount];
                }
            }

            if ((total_CrossPoints < 0) || (Math.Abs(total_angle) > 0.000001))
                return true;
            else
                return false;
            // The total angle should be 2 * PI or -2 * PI
            //return (Math.Abs(total_angle) > 0.000001);
        }

        // Return the angle ABC. Return a value between PI and -PI.
        public float GetAngle(float Ax, float Ay,
            float Bx, float By, float Cx, float Cy)
        {
            // Get the vectors' coordinates.
            float BAx = Ax - Bx;
            float BAy = Ay - By;
            float BCx = Cx - Bx;
            float BCy = Cy - By;

            // Get the dot product.
            float dot_product = BAx * BCx + BAy * BCy;

            // Get the cross product.
            float cross_product = BAx * BCy - BAy * BCx;
            m_lstCrossPoints.Add(cross_product);

            // Calculate the angle.
            return (float)Math.Atan2(cross_product, dot_product);
        }

        private List<Point> GetPolygonPointsFromGeometry(List<List<float>> polygon)
        {
            List<Point> Points = new List<Point>();
            foreach (List<float> polygonPoint in polygon)
            {
                Point p = new Point()
                {
                    X = polygonPoint.FirstOrDefault(),
                    Y = polygonPoint.LastOrDefault()
                };

                Points.Add(p);
            }

            return Points;
        }
    }

    public class Point
    {
        public float X, Y;
    }
}        

