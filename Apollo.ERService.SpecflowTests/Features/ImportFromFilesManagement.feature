#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: Import From Files Management

Background: 
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities and Relationships Synchronously Importing From CSV File in an ER Collection using V2.0, Get All Relationships from the ER Collection, and Delete Relationships and Entities
   When the IMS User "apollo.er.testuser1" uploads "PostEntities.csv", "PostRelationship.csv" files   
		And the IMS User posts "2" new entities importing from CSV file where "fileName" is "PostEntities.csv" and "async" is "false" using V2.0 in an ER Collection
		And the IMS User posts "1" new relationship(s) importing from CSV file where "fileName" is "PostRelationship.csv" and "async" is "false" using V2.0 in the ER Collection
	Then validate the IMS User gets all relationships from the ER Collection
		And validate the IMS User gets all entities including "2" uploaded file entities from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities including the uploaded file entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously Importing From Json File in an ER Collection using V2.0, Get All Relationships from the ER Collection, and Delete Relationships and Entities
    When the IMS User "apollo.er.testuser1" uploads "PostEntities.json", "PostRelationship.json" files
		And the IMS User posts "2" new entities importing from Json file where "fileName" is "PostEntities.json" and "async" is "false" using V2.0 in an ER Collection
		And the IMS User posts "1" new relationship(s) importing from Json file where "fileName" is "PostRelationship.json" and "async" is "false" using V2.0 in the ER Collection
	Then validate the IMS User gets all relationships from the ER Collection
		And validate the IMS User gets all entities including "2" uploaded file entities from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities including the uploaded file entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection
