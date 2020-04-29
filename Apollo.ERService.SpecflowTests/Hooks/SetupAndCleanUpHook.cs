/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using BoDi;
using System.Linq;
using System.Net;
using TechTalk.SpecFlow;
using Serilog;
using System;
using Apollo.ERService.SpecflowTests.TestSettings;
using Apollo.ERService.SpecflowTests.Extensions;

namespace Apollo.ERService.SpecflowTests.Hooks
{
    [Binding]
    public class SetupAndCleanUpHook
    {
        private static ITestConfiguration _testCfg;
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext scenarioContext;

        public SetupAndCleanUpHook(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            this.scenarioContext = scenarioContext ?? throw new ArgumentNullException("ScenarioContext is null");
        }


        [BeforeTestRun]
        public static void BaseSetup()
        {
            if (_testCfg == null)
            {
                _testCfg = new AppSettingsCfg();
            }

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            Log.Logger = new LoggerConfiguration().CreateLogger();
            Log.Information("Test is configured to point to {0}, {1}", _testCfg.ERApiEndpoint, _testCfg.ImsEndpoint);
        }

        [BeforeScenario(Order = 100)]
        public void BeforeScenario()
        {
            _objectContainer.RegisterInstanceAs(_testCfg);
        }

        [AfterScenario]
        public void AfterScenarioCleanUp()
        {
            RelationshipCleanUp();
            EntityCleanUp();
            ErCollectionCleanUp();

            ERCollectionToRelationshipsPairsCleanUp();
            ERCollectionToEntitiesPairsCleanUp();
            PartialUpdateResponsesCleanUp();

            HttpResponsesCleanUp();
            RestClientExceptionErrorsCleanUp();
        }

        private void EntityCleanUp()
        {
            var entitiesToDelete = scenarioContext.CreatedEntities().ToList();
            for (int i = 0; i < entitiesToDelete.Count; i++)
            {
                Log.Information("Attempting to delete entity {0} as IMS User {1}", entitiesToDelete[i].Id, _testCfg.PrimaryUserName);
                scenarioContext.RemoveEntity(scenarioContext.CreatedEntities().Find(entity => entity.Id == entitiesToDelete[i].Id));
            }
        }
        private void RelationshipCleanUp()
        {
            var relationshipsToDelete = scenarioContext.CreatedRelationships().ToList();
            for (int i = 0; i < relationshipsToDelete.Count; i++)
            {
                Log.Information("Attempting to delete relationship {0} as IMS User {1}", relationshipsToDelete[i].Id, _testCfg.PrimaryUserName);
                scenarioContext.RemoveRelationship(scenarioContext.CreatedRelationships().Find(relationship => relationship.Id == relationshipsToDelete[i].Id));
            }
        }
        private void ErCollectionCleanUp()
        {
            var erCollectionsToDelete = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < erCollectionsToDelete.Count; i++)
            {
                Log.Information("Attempting to delete ER Collections {0} as IMS User {1}", erCollectionsToDelete[i], _testCfg.PrimaryUserName);
                scenarioContext.RemoveERCollections(erCollectionsToDelete[i]);
            }
        }
        private void ERCollectionToRelationshipsPairsCleanUp()
        {
            var pairsToDelete = scenarioContext.AllERCollectionToRelationshipsPairs().ToList();
            for (int i = 0; i < pairsToDelete.Count; i++)
            {
                Log.Information("Attempting to delete ER Collection to Relationships pairs from scenario context");
                scenarioContext.RemoveERCollectionToRelationshipsPair(pairsToDelete[i]);
            }
        }
        private void ERCollectionToEntitiesPairsCleanUp()
        {
            var pairsToDelete = scenarioContext.AllERCollectionToEntitiesPairs().ToList();
            for (int i = 0; i < pairsToDelete.Count; i++)
            {
                Log.Information("Attempting to delete ER Collection to Entities pairs from scenario context");
                scenarioContext.RemoveERCollectionToEntitiesPair(pairsToDelete[i]);
            }
        }
        private void PartialUpdateResponsesCleanUp()
        {
            var partialUpdateResponsesToDelete = scenarioContext.AllPartialUpdateResponses().ToList();
            for (int i = 0; i < partialUpdateResponsesToDelete.Count; i++)
            {
                Log.Information("Attempting to delete partial update responses from scenario context");
                scenarioContext.RemovePartialUpdateResponses(partialUpdateResponsesToDelete[i]);
            }
        }
        private void HttpResponsesCleanUp()
        {
            var httpResponsesToDelete = scenarioContext.AllHttpResponses().ToList();
            for (int i = 0; i < httpResponsesToDelete.Count; i++)
            {
                Log.Information("Attempting to delete rest client exception errors from scenario context");
                scenarioContext.RemoveHttpResponse(httpResponsesToDelete[i]);
            }
        }
        private void RestClientExceptionErrorsCleanUp()
        {
            var restClientExceptionErrorsToDelete = scenarioContext.AllRestClientExceptionErrors().ToList();
            for (int i = 0; i < restClientExceptionErrorsToDelete.Count; i++)
            {
                Log.Information("Attempting to delete rest client exception errors from scenario context");
                scenarioContext.RemoveRestClientExceptionErrors(restClientExceptionErrorsToDelete[i]);
            }
        }
    }
}
