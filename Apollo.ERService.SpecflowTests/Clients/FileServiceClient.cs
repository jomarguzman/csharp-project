/*-------------------------------------------------------------------------------------

© Copyright 2018 Johnson Controls, Inc.
Use or Copying of all or any part of this program, except as
permitted by License Agreement, is prohibited.

-------------------------------------------------------------------------------------*/

using Apollo.ERService.SpecflowTests.Models;
using Apollo.ERService.SpecflowTests.TestSettings;
using Jci.Be.Data.Identity.HttpClient.TokenProvider;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Apollo.ERService.SpecflowTests.Clients
{
    public class FileServiceClient
    {
        ITestConfiguration _testCfg = new AppSettingsCfg();
        private string fileUploadBaseRoute;
        public FileServiceClient()
        {
            SetRoutes();
        }
        private void SetRoutes()
        {
            fileUploadBaseRoute = $"{_testCfg.FilesApiEndpoint}/upload";
        }

        public string GetIMSAccessToken(string userName)
        {
            ITokenProvider tokenProvider = new ResourceOwnerFlow(
               _testCfg.ImsEndpoint,
               _testCfg.ImsClientId,
               _testCfg.ImsClientSecret,
               userName,
               _testCfg.PrimaryUserPassword,
               _testCfg.ImsScopes
            );
            return tokenProvider.GetToken().GetAwaiter().GetResult().AccessToken;
        }

        public IRestResponse UploadFile(string token, string fileName, string erCollectionId)
        {
            var fileClient = new RestClient($"{fileUploadBaseRoute}/{fileName}");
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "multipart/related; boundary=foo_bar_baz");
            request.AddHeader("X-Metadata-Only", "false");
            request.AddHeader("If-None-Match", "*");
            request.AddHeader("ercollectionid", erCollectionId);
            request.AddParameter("undefined", MetaBody(fileName), ParameterType.RequestBody);
            Thread.Sleep(1000);
            IRestResponse response = fileClient.Execute(request);
            return response;
        }

        private string MetaBody(string fileName)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Files\" + fileName);
            var files = File.ReadAllBytes(path);
            var file = Convert.ToBase64String(files);
            var fileExtension = Path.GetExtension(fileName);
            string contentType = string.Empty;

            if (fileExtension == ".csv")
                contentType = "text/csv";
            if (fileExtension == ".json")
                contentType = "application/json";

            var fileBody = "--foo_bar_baz\r\nContent-Type: application/json; charset=UTF-8;\r\n\r\n{\r\n\"file_name\":\"" + fileName + "\",\r\n\"file_description\": \"Entity/Relationship Import file\",\r\n\"file_size\": 2179072,\r\n\"file_type\": \"" + contentType + "\",\r\n\"file_rating\": \"5\"\r\n}\r\n--foo_bar_baz\r\nContent-Type: " + contentType + "\r\n\r\n" + file + "\r\n--foo_bar_baz--\r\n";
            return fileBody;
        }

        public List<Entity> GetEntitiesFromJsonFile()
        {
            string fileName = "PostEntities.json";
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Files\" + fileName);
            string filesContent = File.ReadAllText(path);
            var listOfEntities = JsonConvert.DeserializeObject<List<Entity>>(filesContent);
            return listOfEntities;
        }
        public List<Relationship> GetRelationshipsFromJsonFile()
        {
            string fileName = "PostRelationship.json";
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Files\" + fileName);
            string filesContent = File.ReadAllText(path);
            var relationshipList = JsonConvert.DeserializeObject<List<Relationship>>(filesContent);
            return relationshipList;
        }
    }
}
