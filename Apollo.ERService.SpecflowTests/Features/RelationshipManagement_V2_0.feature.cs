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
    public partial class RelationshipManagementV2_0Feature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "RelationshipManagement_V2_0.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Relationship Management V2.0", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Relationship Management V2.0")))
            {
                global::Apollo.ERService.SpecflowTests.Features.RelationshipManagementV2_0Feature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously in an ER Collection, Get All Relati" +
            "onships from the ER Collection, and Delete Relationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyInAnERCollectionGetAllRelationshipsFromTheERCollectionAndDeleteRelationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously in an ER Collection, Get All Relati" +
                    "onships from the ER Collection, and Delete Relationships and Entities", null, ((string[])(null)));
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
                    "\"false\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 18
 testRunner.Then("validate the IMS User gets all relationships from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 19
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 20
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 21
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 22
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously in an ER Collection, Get A Specific" +
            " Relationship By Relationship Id from the ER Collection, and Delete Relationship" +
            "s and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyInAnERCollectionGetASpecificRelationshipByRelationshipIdFromTheERCollectionAndDeleteRelationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously in an ER Collection, Get A Specific" +
                    " Relationship By Relationship Id from the ER Collection, and Delete Relationship" +
                    "s and Entities", null, ((string[])(null)));
#line 24
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 25
    testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 27
 testRunner.Then("validate the IMS User gets a specific relationship by relationship Id from the ER" +
                    " Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 28
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 29
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 30
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously in an ER Collection, Get All Versio" +
            "ns Of A Specific Relationship By Relationship Id from the ER Collection, and Del" +
            "ete Relationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyInAnERCollectionGetAllVersionsOfASpecificRelationshipByRelationshipIdFromTheERCollectionAndDeleteRelationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously in an ER Collection, Get All Versio" +
                    "ns Of A Specific Relationship By Relationship Id from the ER Collection, and Del" +
                    "ete Relationships and Entities", null, ((string[])(null)));
#line 33
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 34
    testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 35
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 in the ER Collection \"2\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 36
 testRunner.Then("validate the IMS User gets all \"2\" versions of specific relationship by relations" +
                    "hip Id from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 37
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 38
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 40
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Synchronously in an ER Collection, Get All Versio" +
            "ns Of A Specific Relationship By Relationship Id using Page Size and Continuatio" +
            "n Token from the ER Collection, and Delete Relationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        public virtual void PostEntitiesAndRelationshipsSynchronouslyInAnERCollectionGetAllVersionsOfASpecificRelationshipByRelationshipIdUsingPageSizeAndContinuationTokenFromTheERCollectionAndDeleteRelationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Synchronously in an ER Collection, Get All Versio" +
                    "ns Of A Specific Relationship By Relationship Id using Page Size and Continuatio" +
                    "n Token from the ER Collection, and Delete Relationships and Entities", null, ((string[])(null)));
#line 42
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 43
    testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 44
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 in the ER Collection \"2\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 45
 testRunner.Then("validate the IMS User gets \"1\" version of specific relationship by relationship I" +
                    "d and \"continuationToken\" when page size is \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 46
  testRunner.And("validate the IMS User gets \"1\" versions of a specific relationship by relationshi" +
                    "p Id using the \"continuationToken\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 48
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 49
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships ASynchronously in an ER Collection, Get All Versi" +
            "ons Of A Specific Relationship By Relationship Id from the ER Collection, and De" +
            "lete Relationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        public virtual void PostEntitiesAndRelationshipsASynchronouslyInAnERCollectionGetAllVersionsOfASpecificRelationshipByRelationshipIdFromTheERCollectionAndDeleteRelationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships ASynchronously in an ER Collection, Get All Versi" +
                    "ons Of A Specific Relationship By Relationship Id from the ER Collection, and De" +
                    "lete Relationships and Entities", null, ((string[])(null)));
#line 52
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 53
    testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"true\" using V2.0 in an ER C" +
                    "ollection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 54
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"true\" using V2.0 in the ER Collection \"2\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 55
 testRunner.Then("validate the IMS User gets all \"2\" versions of specific relationship by relations" +
                    "hip Id from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 56
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 59
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Asynchronously in an ER Collection, Get All Relat" +
            "ionships from the ER Collection, and Delete Relationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        public virtual void PostEntitiesAndRelationshipsAsynchronouslyInAnERCollectionGetAllRelationshipsFromTheERCollectionAndDeleteRelationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Asynchronously in an ER Collection, Get All Relat" +
                    "ionships from the ER Collection, and Delete Relationships and Entities", null, ((string[])(null)));
#line 61
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 62
    testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"true\" using V2.0 in an ER C" +
                    "ollection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 63
  testRunner.And("the IMS User posts \"5\" new relationship(s) between the entities where \"async\" is " +
                    "\"true\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
 testRunner.Then("validate the IMS User gets all relationships from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 65
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 68
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Reationships Synchronously in an ER Collection using V2.0, Partially Update " +
            "the Reationships Synchronously, and Delete Reationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PartialUpdate")]
        public virtual void PostReationshipsSynchronouslyInAnERCollectionUsingV2_0PartiallyUpdateTheReationshipsSynchronouslyAndDeleteReationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Reationships Synchronously in an ER Collection using V2.0, Partially Update " +
                    "the Reationships Synchronously, and Delete Reationships and Entities", null, new string[] {
                        "PartialUpdate"});
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
                    "\"false\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 74
     testRunner.And("the IMS User updates the relationship(s) where \"async\" is \"false\" and \"allowParti" +
                    "alUpdate\" is \"true\" using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
 testRunner.Then("validate the IMS User updates the relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 76
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Reationships Asynchronously in an ER Collection using V2.0, Partially Update" +
            " the Reationships Synchronously, and Delete Reationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PartialUpdate")]
        public virtual void PostReationshipsAsynchronouslyInAnERCollectionUsingV2_0PartiallyUpdateTheReationshipsSynchronouslyAndDeleteReationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Reationships Asynchronously in an ER Collection using V2.0, Partially Update" +
                    " the Reationships Synchronously, and Delete Reationships and Entities", null, new string[] {
                        "PartialUpdate"});
#line 82
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 83
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"true\" using V2.0 in an ER C" +
                    "ollection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 84
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"true\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
     testRunner.And("the IMS User updates the relationship(s) where \"async\" is \"true\" and \"allowPartia" +
                    "lUpdate\" is \"true\" using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 86
 testRunner.Then("validate the IMS User updates the relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 87
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 88
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 89
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 90
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Reationships Synchronously in an ER Collection using V2.0 with allowPartialU" +
            "pdate, Get All Relationships from the ER Collection, and Delete Reationships and" +
            " Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PartialUpdate")]
        public virtual void PostReationshipsSynchronouslyInAnERCollectionUsingV2_0WithAllowPartialUpdateGetAllRelationshipsFromTheERCollectionAndDeleteReationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Reationships Synchronously in an ER Collection using V2.0 with allowPartialU" +
                    "pdate, Get All Relationships from the ER Collection, and Delete Reationships and" +
                    " Entities", null, new string[] {
                        "PartialUpdate"});
#line 93
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 94
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 95
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" and \"allowPartialUpdate\" is \"true\" using V2.0 in an ER Collection \"1\" ti" +
                    "me(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 96
 testRunner.Then("validate the IMS User gets all relationships from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 97
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 98
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 100
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Reationships Synchronously in an ER Collection using V2.0, Partially Update " +
            "the relationship ID Synchronously, and Delete Reationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PartialUpdate")]
        public virtual void PostReationshipsSynchronouslyInAnERCollectionUsingV2_0PartiallyUpdateTheRelationshipIDSynchronouslyAndDeleteReationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Reationships Synchronously in an ER Collection using V2.0, Partially Update " +
                    "the relationship ID Synchronously, and Delete Reationships and Entities", null, new string[] {
                        "PartialUpdate"});
#line 103
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 104
 testRunner.When("the IMS User posts \"2\" new entities where \"async\" is \"false\" using V2.0 in an ER " +
                    "Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 105
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities where \"async\" is " +
                    "\"false\" using V2.0 in the ER Collection \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 106
     testRunner.And("the IMS User updates the relationship ID where \"async\" is \"false\" and \"allowParti" +
                    "alUpdate\" is \"true\" using V2.0 in the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 107
 testRunner.Then("validate the IMS User updates the relationship(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 108
  testRunner.And("IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
  testRunner.And("IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
  testRunner.And("validate IMS User deletes the relationship(s) from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
  testRunner.And("validate IMS User deletes the entities from the ER Collection", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities Synchronously in different ER Collections and Reationships across t" +
            "he ER Collections using V2.0, Get All Relationships from Source ER Collection, a" +
            "nd Delete Relationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostRelationshipsSyncAcrossERCollections")]
        public virtual void PostEntitiesSynchronouslyInDifferentERCollectionsAndReationshipsAcrossTheERCollectionsUsingV2_0GetAllRelationshipsFromSourceERCollectionAndDeleteRelationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities Synchronously in different ER Collections and Reationships across t" +
                    "he ER Collections using V2.0, Get All Relationships from Source ER Collection, a" +
                    "nd Delete Relationships and Entities", null, new string[] {
                        "PostRelationshipsSyncAcrossERCollections"});
#line 114
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 115
 testRunner.When("the IMS User posts \"1\" new entity each in \"2\" ER Collections where \"async\" is \"fa" +
                    "lse\" using V2.0 \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 116
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities across the ERColl" +
                    "ections where \"async\" is \"false\" using V2.0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
 testRunner.Then("validate the IMS User gets all relationships from source ER Collection(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 118
  testRunner.And("IMS User deletes the relationship(s) from source ER Collection(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
  testRunner.And("IMS User deletes the entities across all the ER Collections", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 120
  testRunner.And("validate IMS User deletes the relationship(s) from source ER Collection(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
  testRunner.And("validate IMS User deletes the entities across all the ER Collections", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Post Entities and Relationships Asynchronously in different ER Collections using " +
            "V2.0, Post Relationships Synchronously Across ER Collections, Get All Relationsh" +
            "ips from Source ER Collection, and Delete Relationships and Entities")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Relationship Management V2.0")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("PostRelationshipsAsyncAcrossERCollections")]
        public virtual void PostEntitiesAndRelationshipsAsynchronouslyInDifferentERCollectionsUsingV2_0PostRelationshipsSynchronouslyAcrossERCollectionsGetAllRelationshipsFromSourceERCollectionAndDeleteRelationshipsAndEntities()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post Entities and Relationships Asynchronously in different ER Collections using " +
                    "V2.0, Post Relationships Synchronously Across ER Collections, Get All Relationsh" +
                    "ips from Source ER Collection, and Delete Relationships and Entities", null, new string[] {
                        "PostRelationshipsAsyncAcrossERCollections"});
#line 124
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 11
this.FeatureBackground();
#line 125
 testRunner.When("the IMS User posts \"1\" new entity each in \"2\" ER Collections where \"async\" is \"tr" +
                    "ue\" using V2.0 \"1\" time(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 126
  testRunner.And("the IMS User posts \"1\" new relationship(s) between the entities across the ERColl" +
                    "ections where \"async\" is \"true\" using V2.0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
 testRunner.Then("validate the IMS User gets all relationships from source ER Collection(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 128
  testRunner.And("IMS User deletes the relationship(s) from source ER Collection(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
  testRunner.And("IMS User deletes the entities across all the ER Collections", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 130
  testRunner.And("validate IMS User deletes the relationship(s) from source ER Collection(s)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
  testRunner.And("validate IMS User deletes the entities across all the ER Collections", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion

