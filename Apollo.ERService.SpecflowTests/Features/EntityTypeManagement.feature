#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: EntityType Management

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities Synchronously in an ER Collection, Get All Entities of Specific Entity Type from the ER Collection, and Delete Entities
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities of specific entity type from the ER Collection
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Synchronously in an ER Collection, Get All Versions Of A Specific Entity By Entity Type from the ER Collection, and Delete Entities
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "2" time(s)
	Then validate the IMS User gets all "2" versions of a specific entity by entity type from the ER Collection
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Synchronously in an ER Collection, Get All Versions Of A Specific Entity By Entity Type using Page Size and Continuation Token from the ER Collection, and Delete Entities
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "2" time(s)
	Then validate the IMS User gets "1" versions of a specific entity by entity type and "continuationToken" when page size is "1" 
		And validate the IMS User gets "1" versions of a specific entity by entity type using the "continuationToken"
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Asynchronously in an ER Collection, Get All Versions Of A Specific Entity By Entity Type from the ER Collection, and Delete Entities
	When the IMS User posts "1" new entity where "async" is "true" using V2.0 in an ER Collection "2" time(s)
	Then validate the IMS User gets all "2" versions of a specific entity by entity type from the ER Collection
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Synchronously in an ER Collection, Get Entities of Specific Entity Type using Page Size and Continuation Token from the ER Collection, and Delete Entities
    When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets "1" entity and "continuationToken" when page size is "1" 
		And validate the IMS User gets "1" entity using the "continuationToken"
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Synchronously in an ER Collection, Get All Entities Of A Specific Type Including SystemData, and Delete Entities
    When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities of a specific type with system data when includeSystemData is "true"
	    And IMS User deletes the entity from the ER Collection
	    And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entities Asynchronously in an ER Collection, Get All Entities from the ER Collection, and Delete Entities
	When the IMS User posts "5" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities from the ER Collection
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection


