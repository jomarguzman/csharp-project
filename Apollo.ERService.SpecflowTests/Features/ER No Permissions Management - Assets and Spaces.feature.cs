﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Apollo.ERService.SpecflowTests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class ERNoPermissionsManagement_AssetsAndSpacesFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "ER No Permissions Management - Assets and Spaces.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ER No Permissions Management - Assets and Spaces", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "ER No Permissions Management - Assets and Spaces")))
            {
                global::Apollo.ERService.SpecflowTests.Features.ERNoPermissionsManagement_AssetsAndSpacesFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 12
#line 13
 testRunner.Given("an IMS User \"apollo.er.testuser1\" with an access token", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 14
 testRunner.When("the IMS User posts Collection and response status Code as OK", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities Synchronously in an ER Collection by Primary User, Secondary User W" +
            "ith No Permissions Cannot Get list of Assets and Spaces within the ER Collection" +
            " and Delete Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ER No Permissions Management - Assets and Spaces")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("AssetsAndSpaces")]
        public virtual void PostEntitiesSynchronouslyInAnERCollectionByPrimaryUserSecondaryUserWithNoPermissionsCannotGetListOfAssetsAndSpacesWithinTheERCollectionAndDeleteEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities Synchronously in an ER Collection by Primary User, Secondary User W" +
                    "ith No Permissions Cannot Get list of Assets and Spaces within the ER Collection" +
                    " and Delete Entities", null, new string[] {
                        "AssetsAndSpaces"});
#line 17
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 12
this.FeatureBackground();
#line 18
 testRunner.When("the IMS User posts \"2\" new entities of types \"Brick__Equipment,Brick__Building\" w" +
                    "here \"async\" is \"false\" using V2.0 in an ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 19
 testRunner.Then("validate the Secondary User with \"No\" permissions cannot get the list of Assets w" +
                    "ithin the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 20
  testRunner.And("validate the Secondary User with \"No\" permissions cannot get the list of Spaces w" +
                    "ithin the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("validate the Secondary User with \"No\" permissions gets \"403\" Forbidden error code" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
