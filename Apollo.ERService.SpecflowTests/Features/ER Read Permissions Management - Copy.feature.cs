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
    public partial class ERReadPermissionsManagement_CopyFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "ER Read Permissions Management - Copy.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ER Read Permissions Management - Copy", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "ER Read Permissions Management - Copy")))
            {
                global::Apollo.ERService.SpecflowTests.Features.ERReadPermissionsManagement_CopyFeature.FeatureSetup(null);
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
#line 13
#line 14
 testRunner.Given("an IMS User \"apollo.er.testuser1\" with an access token", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
 testRunner.When("the IMS User posts Collection and response status Code as OK", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute(@"Post Entities and Relationships Synchronously in an ER Collection using V2.0 by Primary User, Primary User Sets Secondary User With ReadOnly Permissions, Secondary User Can Copy Entities and Relationships from Source ER Collection to Destination ER Colletion and Delete Relationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "ER Read Permissions Management - Copy")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute()]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyInAnERCollectionUsingV2_0ByPrimaryUserPrimaryUserSetsSecondaryUserWithReadOnlyPermissionsSecondaryUserCanCopyEntitiesAndRelationshipsFromSourceERCollectionToDestinationERColletionAndDeleteRelationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo(@"Post Entities and Relationships Synchronously in an ER Collection using V2.0 by Primary User, Primary User Sets Secondary User With ReadOnly Permissions, Secondary User Can Copy Entities and Relationships from Source ER Collection to Destination ER Colletion and Delete Relationships and Entities", null, new string[] {
                        "ignore"});
#line 17
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 13
this.FeatureBackground();
#line 18
 testRunner.When("the IMS User posts \"2\" new entity where \"async\" is \"false\" using V2.0 in an ER Co" +
                    "llection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 19
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
  testRunner.And("the IMS User sets \"readonly\" permissions for Secondary User \"apollo.er.testuser2\"" +
                    "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
 testRunner.Then("the Secondary User with \"readonly\" permissions Can copies entities and relationsh" +
                    "ips from this ER Collection to another ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
   testRunner.And("validate the Secondary User with \"readonly\" permissions can get all relationships" +
                    " from the destination ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 23
  testRunner.And("validate the Secondary User with \"readonly\" permissions can get all entities from" +
                    " the destination ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 24
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 25
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
