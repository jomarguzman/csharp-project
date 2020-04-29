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
    public partial class RelationshipManagmentV2_0_NegativeTestsFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "RelationshipManagment_V2_0-NegativeTests.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Relationship Managment V2.0 - NegativeTests", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Relationship Managment V2.0 - NegativeTests")))
            {
                global::Apollo.ERService.SpecflowTests.Features.RelationshipManagmentV2_0_NegativeTestsFeature.FeatureSetup(null);
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
#line 11
#line 12
 testRunner.Given("an IMS User \"apollo.er.testuser1\" with an access token", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 13
 testRunner.When("the IMS User posts Collection and response status Code as OK", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously Without Required Field Relationship" +
            " Id and validate response message Relationship id is missing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyWithoutRequiredFieldRelationshipIdAndValidateResponseMessageRelationshipIdIsMissing()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously Without Required Field Relationship" +
                    " Id and validate response message Relationship id is missing", null, ((string[])(null)));
#line 15
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 16
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 17
     testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" without \"relationshipId\" property using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
 testRunner.Then("validate the IMS User gets \"failure\" responses message \"Relationship id is missin" +
                    "g\" while updating relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 19
  testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously Without Required Field Relationship" +
            " Type and validate response message Relationship Type is missing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyWithoutRequiredFieldRelationshipTypeAndValidateResponseMessageRelationshipTypeIsMissing()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously Without Required Field Relationship" +
                    " Type and validate response message Relationship Type is missing", null, ((string[])(null)));
#line 23
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 24
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 25
     testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" without \"relationshipType\" property using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 26
 testRunner.Then("validate the IMS User gets \"failure\" responses message \"Relationship type is miss" +
                    "ing\" while updating relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 27
  testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 28
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously Without Required Field Source Entit" +
            "y ID and validate response message sourceEntityId:PropertyRequired")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyWithoutRequiredFieldSourceEntityIDAndValidateResponseMessageSourceEntityIdPropertyRequired()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously Without Required Field Source Entit" +
                    "y ID and validate response message sourceEntityId:PropertyRequired", null, ((string[])(null)));
#line 31
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 32
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 33
     testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" without \"sourceEntityId\" property using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
 testRunner.Then("validate the IMS User gets \"failure\" responses message \"sourceEntityId:PropertyRe" +
                    "quired\" while updating relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 35
  testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously Without Required Field Destination " +
            "Entity ID and validate response message destinationEntityId:PropertyRequired")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyWithoutRequiredFieldDestinationEntityIDAndValidateResponseMessageDestinationEntityIdPropertyRequired()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously Without Required Field Destination " +
                    "Entity ID and validate response message destinationEntityId:PropertyRequired", null, ((string[])(null)));
#line 39
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 40
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 41
     testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" without \"destinationEntityId\" property using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 42
 testRunner.Then("validate the IMS User gets \"failure\" responses message \"destinationEntityId:Prope" +
                    "rtyRequired\" while updating relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 43
  testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 44
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously Without Required Field Relationship" +
            " Name and validate response message relationshipName:PropertyRequired")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyWithoutRequiredFieldRelationshipNameAndValidateResponseMessageRelationshipNamePropertyRequired()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously Without Required Field Relationship" +
                    " Name and validate response message relationshipName:PropertyRequired", null, ((string[])(null)));
#line 47
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 48
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 49
     testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" without \"relationshipName\" property using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
 testRunner.Then("validate the IMS User gets \"failure\" responses message \"relationshipName:Property" +
                    "Required\" while updating relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 51
  testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Post Relationships Synchronously With Invalid SourceEntityId an" +
            "d DestinationEntityId in an ER Collection")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void PostEntitiesAndPostRelationshipsSynchronouslyWithInvalidSourceEntityIdAndDestinationEntityIdInAnERCollection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Post Relationships Synchronously With Invalid SourceEntityId an" +
                    "d DestinationEntityId in an ER Collection", null, ((string[])(null)));
#line 55
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 56
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 57
     testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" with invalid sourceEntityId using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" with invalid destinationEntityId using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
 testRunner.Then("validate the IMS User gets \"412\" precondition failed error code for the post(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 60
  testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 61
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
  testRunner.And("validate IMS User cannot delete the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 63
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously With An Empty List in an ER Collect" +
            "ion")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyWithAnEmptyListInAnERCollection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously With An Empty List in an ER Collect" +
                    "ion", null, ((string[])(null)));
#line 65
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 66
 testRunner.When("the IMS User posts \"0\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 67
 testRunner.Then("validate the IMS User gets \"400\" Bad Request error code for the post(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 68
  testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 69
  testRunner.And("validate IMS User cannot delete the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously With Invalid Token in an ER Collect" +
            "ion")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyWithInvalidTokenInAnERCollection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously With Invalid Token in an ER Collect" +
                    "ion", null, ((string[])(null)));
#line 71
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 72
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 73
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" With Invalid Token using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
 testRunner.Then("validate the IMS User gets \"401\" Unauthorized error code for the post(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 75
  testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities Async and Relationships Sync in Bulk in an ER Collection")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("BulkPost")]
        public virtual void PostEntitiesAsyncAndRelationshipsSyncInBulkInAnERCollection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities Async and Relationships Sync in Bulk in an ER Collection", null, new string[] {
                        "BulkPost"});
#line 78
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 79
 testRunner.When("the IMS User posts \"68\" new entities where \"async\" is \"true\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 80
  testRunner.And("the IMS User posts \"102\" new relationship(s) between the entities where \"async\" i" +
                    "s \"false\" using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
 testRunner.Then("validate the IMS User gets \"413\" Payload too large error code for the post(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 82
  testRunner.And("validate the IMS User gets all entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
  testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Post Relationships Synchronously Without ER Collection")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void PostEntitiesAndPostRelationshipsSynchronouslyWithoutERCollection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Post Relationships Synchronously Without ER Collection", null, ((string[])(null)));
#line 85
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 86
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 87
     testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 without the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
 testRunner.Then("validate the IMS User gets \"400\" Bad Request error code for the post(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 89
    testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 91
  testRunner.And("validate IMS User cannot delete the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Get Relationship By Relationship Id that doesn\'t exist from an ER Collection")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void GetRelationshipByRelationshipIdThatDoesntExistFromAnERCollection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get Relationship By Relationship Id that doesn\'t exist from an ER Collection", null, ((string[])(null)));
#line 94
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 95
  testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 96
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
  testRunner.And("IMS User gets a specific relationship by relationship Id that doesn\'t exist from " +
                    "the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
 testRunner.Then("validate the IMS User gets \"404\" Not Found error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 99
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 102
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Reationships Synchronously in an ER Collection using V2.0 with allowPartialU" +
            "pdate, with Invalid Schema")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PartialUpdate")]
        public virtual void PostReationshipsSynchronouslyInAnERCollectionUsingV2_0WithAllowPartialUpdateWithInvalidSchema()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Reationships Synchronously in an ER Collection using V2.0 with allowPartialU" +
                    "pdate, with Invalid Schema", null, new string[] {
                        "PartialUpdate"});
#line 118
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 119
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 120
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" and \"allowPartialUpdate\" is \"true\" using V2.0 in an ER Collection \"1\" ti" +
                    "me(s) with Invalid Schema", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
 testRunner.Then("validate the IMS User gets \"failure\" responses message \"Failed-Invalid Schema\" wh" +
                    "ile updating relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 122
  testRunner.And("validate IMS User cannot get the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 124
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Partially Update the Relationships Synchronously With Invalid property Fileds and" +
            " validate response message NoAdditionalPropertiesAllowed")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PartialUpdate")]
        public virtual void PartiallyUpdateTheRelationshipsSynchronouslyWithInvalidPropertyFiledsAndValidateResponseMessageNoAdditionalPropertiesAllowed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Partially Update the Relationships Synchronously With Invalid property Fileds and" +
                    " validate response message NoAdditionalPropertiesAllowed", null, new string[] {
                        "PartialUpdate"});
#line 127
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 128
  testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 129
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
  testRunner.And("the IMS User updates the relationships where \"async\" is \"false\" and \"allowPartial" +
                    "Update\" is \"true\" with invalid property name \"InvalidProp\" and value \"Test-01\" i" +
                    "n the ER Collection using V2.0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
 testRunner.Then("validate the IMS User gets \"failure\" responses message \"NoAdditionalPropertiesAll" +
                    "owed\" while updating relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 132
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 133
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 135
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Partially Update the Relationships Synchronously Without the Required field Relat" +
            "ionship Id and validate response message Relationship id is missing")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PartialUpdate")]
        public virtual void PartiallyUpdateTheRelationshipsSynchronouslyWithoutTheRequiredFieldRelationshipIdAndValidateResponseMessageRelationshipIdIsMissing()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Partially Update the Relationships Synchronously Without the Required field Relat" +
                    "ionship Id and validate response message Relationship id is missing", null, new string[] {
                        "PartialUpdate"});
#line 138
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 139
  testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 140
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 141
  testRunner.And("the IMS User updates the relationships where \"async\" is \"false\" and \"allowPartial" +
                    "Update\" is \"true\" without the \"relationshipId\" property using V2.0 in the ER Col" +
                    "lection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 142
 testRunner.Then("validate the IMS User gets \"failure\" responses message \"Relationship id is missin" +
                    "g\" while updating relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 143
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 144
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 145
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 146
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Relationship Synchronously in an ER Collection using V2.0 and Updating Relat" +
            "ionship Type for this Relationship is Not Allowed")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Managment V2.0 - NegativeTests")]
        public virtual void PostRelationshipSynchronouslyInAnERCollectionUsingV2_0AndUpdatingRelationshipTypeForThisRelationshipIsNotAllowed()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Relationship Synchronously in an ER Collection using V2.0 and Updating Relat" +
                    "ionship Type for this Relationship is Not Allowed", null, ((string[])(null)));
#line 148
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 149
 testRunner.When("the IMS User posts \"2\" new entity where \"async\" is \"false\" using V2.0 in an ER Co" +
                    "llection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 150
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 151
  testRunner.And("the IMS User updates the relationship type for the posted relationship(s) where \"" +
                    "async\" is \"false\" using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 152
 testRunner.Then("validate the IMS User gets \"failure\" responses message \"Restricted property relat" +
                    "ionshipType is not allowed to change\" while updating relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 153
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 154
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 155
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 156
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
