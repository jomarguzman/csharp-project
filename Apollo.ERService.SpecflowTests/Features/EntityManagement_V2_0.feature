#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: Entity Management V2.0

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities Synchronously in an ER Collection, Get All Entities from the ER Collection, and Delete Entities
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities from the ER Collection
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Synchronously in an ER Collection, Get A Specific Entity By Entity Id from the ER Collection, and Delete Entities
    When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
    Then validate the IMS User gets a specific entity by entity Id from the ER Collection
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Synchronously in an ER Collection, Get All Versions Of A Specific Entity By Entity Id from the ER Collection, and Delete Entities
    When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "2" time(s)
	Then validate the IMS User gets all "2" versions of a specific entity by entity Id from the ER Collection
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Synchronously in an ER Collection, Get All Versions Of A Specific Entity By Entity Id using Page Size and Continuation Token from the ER Collection, and Delete Entities
    When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "2" time(s)
	Then validate the IMS User gets "1" versions of a specific entity by entity Id and "continuationToken" when page size is "1" 
		And validate the IMS User gets "1" versions of a specific entity by entity Id using the "continuationToken"
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Asynchronously in an ER Collection, Get All Versions Of A Specific Entity By Entity Id from the ER Collection, and Delete Entities
    When the IMS User posts "1" new entity where "async" is "true" using V2.0 in an ER Collection "2" time(s)
	Then validate the IMS User gets all "2" versions of a specific entity by entity Id from the ER Collection
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Synchronously in an ER Collection, Get Multiple Entities by Ids from the ER Collection, and Delete Entities
    When the IMS User posts "3" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities by Ids from the ER Collection
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Asynchronously in an ER Collection, Get All Entities from the ER Collection, and Delete Entities
    When the IMS User posts "5" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities from the ER Collection
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection

@BulkPost
Scenario: Post Entities in Bulk Asynchronously in an ER Collection, Get All Entities from the ER Collection, and Delete Entities
	When the IMS User posts "50" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities from the ER Collection
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection

#Commented the scenario as now we are using System Generated ERCollection
#Scenario: Post Entities Synchronously With Special Characters in an ER Collection, Get All Entities from the ER Collection, and Delete Entities
#	When the IMS User posts "1" new entity where "async" is "false" having Special Characters using V2.0 in an ER Collection "1" time(s)
#	Then validate the IMS User gets all entities from the ER Collection
#		And IMS User deletes the entities from the ER Collection
#		And validate IMS User deletes the entities from the ER Collection


@PartialUpdate
Scenario: Post Entities Synchronously in an ER Collection using V2.0, Partially Update the Entities Synchronously, and Delete Entities 	
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	And the IMS User updates the entities where "async" is "false" and "allowPartialUpdate" is "true" using V2.0 in the ER Collection
	Then validate the IMS User updates the posted entities 
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection

@PartialUpdate
Scenario: Post Entities Asynchronously in an ER Collection using V2.0, Partially Update the Entities Asynchronously, and Delete Entities 
	When the IMS User posts "1" new entity where "async" is "true" using V2.0 in an ER Collection "1" time(s)
		And the IMS User updates the entities where "async" is "true" and "allowPartialUpdate" is "true" using V2.0 in the ER Collection
	Then validate the IMS User updates the posted entities
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection

@PartialUpdate
Scenario: Post Entities Synchronously in an ER Collection using V2.0 with allowPartialUpdate,Get All Entities from the ER Collection, and Delete Entities 	
	When the IMS User posts "1" new entity where "async" is "false" and "allowPartialUpdate" is "true" using V2.0 in the ER Collection "1" time(s)
	Then validate the IMS User gets all entities from the ER Collection
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection

	@PartialUpdate
Scenario: Post Entities Synchronously in an ER Collection using V2.0, Partially Update the Entity ID Synchronously, and Delete Entities 	
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User updates the entity ID where "async" is "false" and "allowPartialUpdate" is "true" using V2.0 in the ER Collection
	Then validate the IMS User updates the posted entities 
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection

@GetFile
Scenario: Upload Json File in an ER Collection, Get file by file name and Get File by Id  
	When the IMS User "apollo.er.testuser1" uploads "PostEntities.json", "PostRelationship.json" files
	Then validate the IMS user gets files by filename "PostEntities.json"
		And validate the IMS user gets file by Id
