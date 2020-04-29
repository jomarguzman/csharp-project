/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Clients;
using Apollo.ERService.SpecflowTests.Extensions;
using Apollo.ERService.SpecflowTests.Models;
using Apollo.ERService.SpecflowTests.TestSettings;
using FluentAssertions;
using Jci.Be.Data.Apollo.Core.Constants;
using Jci.Be.Data.Apollo.Core.Models.Schema;
using Jci.Be.Data.Identity.HttpClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Apollo.ERService.SpecflowTests.Steps
{
    [Binding]
    public class ImportFromFilesManagement
    {
        private readonly ITestConfiguration _testCfg;
        private readonly ScenarioContext scenarioContext;
        private static string restClientUser;

        public ImportFromFilesManagement(ITestConfiguration testConfiguration, ScenarioContext scenarioContext)
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
        private FileServiceClient FileService
        {
            get
            {
                var fileServiceClient = new FileServiceClient();
                return fileServiceClient;
            }
        }

        [Then(@"validate the IMS User gets all entities including ""(.*)"" uploaded file entities from the ER Collection")]
        public async Task ThenValidateTheIMSUserGetsAllEntitiesIncludingUploadedFileEntitiesFromTheERCollection(int numOfFiles)
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
                Log.Information($"Attempting to get all entities from ER Collection {erCollectionIdList[i]}");

                var getAllEntitiesResponseMsg = await ERService.GetAllEntities(headers);
                getAllEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to get all entities from er collection {erCollectionIdList[i]}");

                var getAllEntities = getAllEntitiesResponseMsg.Result.Data.Select(s => s.ToObject<Entity>());

                foreach (var entity in getNewEntities)
                {
                    getAllEntities.Should().Contain(x => x.Id == entity.Id);
                }
                getAllEntities.Count().Should().Be(getNewEntities.Count() + numOfFiles);
            }
        }


        [When(@"the IMS User ""(.*)"" uploads ""(.*)"", ""(.*)"" files")]
        public void WhenTheIMSUserUploadsFiles(string userName, string file1, string file2)
        {
            var filesToUpload = new List<string>
            {
                file1,
                file2
            };
           // var ercollectionId = "testER-" + Guid.NewGuid().ToString();
            string ercollectionId = this.scenarioContext.Get<dynamic>("CollectionsResponse");
            scenarioContext.AddERCollections(ercollectionId);
            var token = FileService.GetIMSAccessToken(userName);
            var fileEntityIdList = new List<string>();
            for (int i = 0; i <= filesToUpload.Count - 1; i++)
            {
                Log.Information($"Attempting to upload file synchronously in ER collection in Blob storage{ercollectionId}");
                var fileUploadResponseMessage = FileService.UploadFile(token, filesToUpload[i], ercollectionId);
                fileUploadResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post file er collection {ercollectionId}");

                var response = fileUploadResponseMessage.Content;
                JObject o = JObject.Parse(response);
                var entityId = (string)o.SelectToken("data.entityId");
                fileEntityIdList.Add(entityId);
            }
            scenarioContext.Set(fileEntityIdList, "FileEntityIdList");
        }

        [When(@"the IMS User posts ""(.*)"" new entities importing from CSV file where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection")]
        [When(@"the IMS User posts ""(.*)"" new entities importing from Json file where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in an ER Collection")]
        public async Task WhenTheIMSUserPostsNewEntitiesImportingFromCSVFileWhereIsAndIsUsingVInAnERCollection(int noOfEntities, string fileNameHeader, string fileName, string asyncHeader, string asyncValue, string version)
        {
            restClientUser = "PrimaryIMSUser";
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int j = 0; j < erCollectionIdList.Count; j++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[j]),
                    new KeyValuePair<string, string>(asyncHeader, asyncValue),
                    new KeyValuePair<string, string>(fileNameHeader, fileName)
                };
                List<Entity> entities = new List<Entity>();

                Log.Information($"Attempting to post new entity/entities importing from file in ER collection {erCollectionIdList[j]}");
                var postEntitiesResponseMsg = await ERService.PostEntities(version, entities, headers);
                if (asyncValue == "true")
                    postEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionIdList[j]}");
                else
                {
                    if (version == "2.0")
                    {
                        postEntitiesResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionIdList[j]}");
                        var response = postEntitiesResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        JObject o = JObject.Parse(response);
                        var entityId = (string)o.SelectToken("data.responses.success").First["id"];
                        var entityId1 = (string)o.SelectToken("data.responses.success").Last["id"];

                        //add entities to scenario context
                        var entityList = FileService.GetEntitiesFromJsonFile();
                        foreach (var entity in entityList)
                        {
                            scenarioContext.AddEntity(entity);
                        }

                        if (fileName.Contains("csv"))
                        {
                            List<string> entityValue = ReadCSVFile(fileName, 3, "id");
                            //Assert.AreEqual(entityValue[0], entityId);
                            //Assert.AreEqual(entityValue[1], entityId1);
                            Assert.IsTrue(entityValue.Contains(entityId));
                            Assert.IsTrue(entityValue.Contains(entityId1));
                        }
                        else if (fileName.Contains("json"))
                        {
                            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Files\" + fileName);
                            string filesContent = File.ReadAllText(path);
                            //var listOfEntities = JsonConvert.DeserializeObject<IEnumerable<EntityBase>>(filesContent);
                            //var expectedEntityId1 = listOfEntities.First().Id;
                            //var expectedEntityId2 = listOfEntities.Last().Id;
                            //Assert.AreEqual(expectedEntityId1, entityId);
                            //Assert.AreEqual(expectedEntityId2, entityId1);
                            Assert.IsTrue(filesContent.Contains(entityId));
                            Assert.IsTrue(filesContent.Contains(entityId1));
                        }
                    }
                    else
                        postEntitiesResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Created, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new entity in er collection {erCollectionIdList[j]}");
                }
            }
        }

        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) importing from CSV file where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection")]
        [When(@"the IMS User posts ""(.*)"" new relationship\(s\) importing from Json file where ""(.*)"" is ""(.*)"" and ""(.*)"" is ""(.*)"" using V(.*) in the ER Collection")]
        public async Task WhenTheIMSUserPostsNewRelationshipSUsingImportFileWhereIsAndIsUsingVInAnERCollectionTimeS(int numOfRelationships, string fileNameHeader, string fileName, string asyncHeader, string asyncValue, string version)
        {
            restClientUser = "PrimaryIMSUser";

            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            for (int i = 0; i < numOfRelationships; i++)
            {
                for (int j = 0; j < erCollectionIdList.Count; j++)
                {
                    var headers = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[j]),
                        new KeyValuePair<string, string>(asyncHeader, asyncValue),
                        new KeyValuePair<string, string>(fileNameHeader, fileName)
                    };
                    List<Relationship> relationships = new List<Relationship>();
                    Log.Information($"Attempting to post new Relationship/Relationships between Entities synchronously in ER collection {erCollectionIdList[j]}");
                    var postRelationshipResponseMsg = await ERService.PostRelationship(version, relationships, headers);
                    if (asyncValue.Equals("true"))
                    {
                        postRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Accepted, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new Relationship in er collection {erCollectionIdList[j]}");
                        Thread.Sleep(50000);
                    }
                    else
                    {
                        if (version == "2.0")
                        {
                            postRelationshipResponseMsg.Response.StatusCode.ToString().Should().Be("OK", $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new relationship in er collection {erCollectionIdList[j]}");
                            var response = postRelationshipResponseMsg.Response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            JObject o = JObject.Parse(response);
                            var relationshipId = (string)o.SelectToken("data.responses.success").First["id"];

                            //add relationships to scenario context
                            var listOfRelationships = FileService.GetRelationshipsFromJsonFile();
                            foreach (var relationship in listOfRelationships)
                            {
                                scenarioContext.AddRelationship(relationship);
                            }

                            if (fileName.Contains("csv"))
                            {
                                List<string> relationshipValue = ReadCSVFile(fileName, 3, "id");
                                Assert.AreEqual(relationshipValue[0], relationshipId);
                            }
                            else if (fileName.Contains("json"))
                            {
                                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Files\" + fileName);
                                string filesContent = File.ReadAllText(path);
                                var relationshipList = JsonConvert.DeserializeObject<IEnumerable<RelationshipBase>>(filesContent);
                                var expectedRelationshipId = relationshipList.First().Id;
                                Assert.AreEqual(expectedRelationshipId, relationshipId);
                            }
                        }
                        else
                            postRelationshipResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.Created, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to post a new Relationship in er collection {erCollectionIdList[j]}");
                    }
                }
            }
        }

        private List<string> ReadCSVFile(string fileName, int rowPoint, string columnName)
        {
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Files\" + fileName);

            DataTable dt = new DataTable();
            DataRow row;
            string[] csvRows = File.ReadAllLines(filePath);
            string[] csvColumn = csvRows[0].Split(',');
            for (int count = 0; count < csvColumn.Length; count++)
            {
                dt.Columns.Add(csvColumn[count].ToString().Trim());
            }

            List<string> datalist = new List<string>();

            for (int count = rowPoint; count < csvRows.Length; count++)
            {
                if (!string.IsNullOrEmpty(csvRows[count]))
                {
                    string[] csvValues = csvRows[count].Split(',');
                    row = dt.NewRow();

                    string[] columnNames = dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();

                    for (int f = 0; f < columnNames.Length; f++)
                    {
                        if (columnNames[f].ToString().Equals(columnName))
                        {
                            datalist.Add(csvValues[f]);
                        }
                    }
                }
            }
            return datalist;
        }

        [Then(@"IMS User deletes the entities including the uploaded file entities from the ER Collection")]
        public async Task ThenIMSUserDeletesTheEntitiesIncludingTheUploadedFileEntitiesFromTheERCollection()
        {
            restClientUser = "PrimaryIMSUser";
            var getNewEntities = scenarioContext.CreatedEntities().ToList();
            var erCollectionIdList = scenarioContext.CreatedERCollections().ToList();
            var fileEntityIdList = scenarioContext.Get<List<string>>("FileEntityIdList");

            for (int i = 0; i < erCollectionIdList.Count; i++)
            {
                var headers = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(PlatformHeaders.ErCollectionId, erCollectionIdList[i])
                };
                for (int j = 0; j < getNewEntities.Count; j++)
                {
                    Log.Information($"Attempting to delete entities from ER Collection {erCollectionIdList[i]}");

                    var deleteAEntityResponseMsg = await ERService.DeleteEntity(getNewEntities[j].Id, headers);
                    deleteAEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to delete entity {getNewEntities[j].Id} from er collection {erCollectionIdList[i]}");
                }
                for (int k = 0; k < fileEntityIdList.Count; k++)
                {
                    Log.Information($"Attempting to delete file entities from ER Collection {erCollectionIdList[i]}");

                    var deleteAEntityResponseMsg = await ERService.DeleteEntity(fileEntityIdList[k], headers);
                    deleteAEntityResponseMsg.Response.StatusCode.Should().Be(HttpStatusCode.OK, $"{restClientUser} user with {_testCfg.ImsScopes} scopes should be able to delete entity {fileEntityIdList[k]} from er collection {erCollectionIdList[i]}");
                }
            }
        }
    }
}
