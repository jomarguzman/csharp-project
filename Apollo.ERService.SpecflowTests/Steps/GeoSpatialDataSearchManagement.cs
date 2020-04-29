/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Clients;
using Apollo.ERService.SpecflowTests.TestSettings;
using Jci.Be.Data.Identity.HttpClient;
using System;
using TechTalk.SpecFlow;

namespace Apollo.ERService.SpecflowTests.Steps
{
    [Binding]
    public class GeoSpatialDataSearchManagement
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public GeoSpatialDataSearchManagement(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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
        public void WhenTheIMSUserPostsNewEntitiesWithGeoBoundaryAndGeoLocationWhereIsUsingVInAnERCollectionTimeS(int numOfEntities, string asyncHeader, string asyncValue, string version, int numOfTimes)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"validate the IMS User gets all entities whose GeoLocation matches exactly with the Query Point ""(.*)"" from the ER Collection")]
        public void ThenValidateTheIMSUserGetsAllEntitiesWhoseGeoLocationMatchesExactlyWithTheQueryPointFromTheERCollection(string pointCoordinates)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"validate the IMS User gets all entities whose GeoBoundary contain Query Point ""(.*)"" from the ER Collection")]
        public void ThenValidateTheIMSUserGetsAllEntitiesWhoseGeoBoundaryContainQueryPointFromTheERCollection(string pointCoordinates)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"validate the IMS User gets all entities whose GeoLocation\(Point\) is contained within the Query Polygon ""(.*)"" from the ER Collection")]
        public void ThenValidateTheIMSUserGetsAllEntitiesWhoseGeoLocationPointIsContainedWithinTheQueryPolygonFromTheERCollection(string polygonCoordinates)
        {
            ScenarioContext.Current.Pending();
        }

    }
}
