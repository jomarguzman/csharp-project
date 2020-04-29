#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: Entity Management V2.0 - NegativeTests

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Negative Test - Post Entities Synchronously Without Required Fields in Entity Id and validate response message Entity id is missing
	When the IMS User posts "1" new entity where "async" is "false" without the "entityId" property using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets "failure" responses message "Entity id is missing" while updating entity	
		And validate IMS User gets "0" entities from the ER Collection

Scenario: Negative Test - Post Entities Synchronously Without Required Fields in Entity Type and validate response message Entity Type is missing
	When the IMS User posts "1" new entity where "async" is "false" without the "entityType" property using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets "failure" responses message "Entity type is missing" while updating entity
		And validate IMS User gets "0" entities from the ER Collection

Scenario: Negative Test - Post Entities Synchronously Without Required Fields in Entity Name and validate response message Entity Name PropertyRequired
	When the IMS User posts "1" new entity where "async" is "false" without the "entityName" property using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets "failure" responses message "entityName:PropertyRequired" while updating entity	
		And validate IMS User gets "0" entities from the ER Collection

Scenario: Negative Test - Post Entities Synchronously Without ER Collection
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 without an ER Collection "1" time(s)
	Then validate the IMS User gets "400" Bad Request error code for the post(s)
		And validate IMS User cannot get the entity without ER Collection
		And validate IMS User cannot delete the entity without the ER Collection

Scenario: Negative Test - Post Entities Synchronously with an empty list in an ER Collection
	When the IMS User posts empty list "0" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets "400" Bad Request error code for the post(s)
		And validate IMS User gets "403" Forbidden error code for get all entities request
		And validate IMS User gets "403" Forbidden error code for delete entities request

Scenario: Negative Test - Post Entities Synchronously With Invalid Token
	When the IMS User posts "1" new entity where "async" is "false" With Invalid Token using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets "401" Unauthorized error code for the post(s)	
		And validate IMS User gets "0" entities from the ER Collection

@BulkPost
Scenario: Negative Test - Post Entities in Bulk Synchronously in an ER Collection
	When the IMS User posts "101" new entities in bulk where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets "413" Request Entity Too Large error code for the post(s)
		And validate IMS User gets "403" Forbidden error code for get all entities request
		And validate IMS User gets "403" Forbidden error code for delete entities request

Scenario: Negative Test - Post Entities Asynchronously Without Required Fields in an ER Collection
	When the IMS User posts "1" new entity where "async" is "true" without the "entityId" property using V2.0 in an ER Collection "1" time(s)
	    And the IMS User posts "1" new entity where "async" is "true" without the "entityType" property using V2.0 in an ER Collection "1" time(s)
	    And the IMS User posts "1" new entity where "async" is "true" without the "entityName" property using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets "400" Bad Request error code for the post(s)    
		And validate IMS User gets "0" entities from the ER Collection

Scenario: Negative Test - Post Entities Asynchronously with an empty list in an ER Collection
	When the IMS User posts empty list "0" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets "400" Bad Request error code for the post(s)
		And validate IMS User gets "403" Forbidden error code for get all entities request
		And validate IMS User gets "403" Forbidden error code for delete entities request

@BulkPost
Scenario: Negative Test - Post Entities in Bulk Asynchronously in an ER Collection
	When the IMS User posts "10001" new entities in bulk where "async" is "true" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets "413" Request Entity Too Large error code for the post(s)
		And validate IMS User gets "403" Forbidden error code for get all entities request
		Then validate the IMS User gets "404" Not Found error


Scenario: Negative Test - Get Entities Without ER Collection
	When the IMS User posts "1" new entity where "async" is "true" using V2.0 without an ER Collection "1" time(s)
	Then validate the IMS User gets "400" Bad Request error code for the post(s)
	    And validate IMS User cannot get the entity without ER Collection
	    And validate IMS User cannot delete the entity without the ER Collection

Scenario: Negative Test - Get Entity By EntityId that doesn't exist from an ER Collection
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	And the IMS User posts Collection and response status Code as OK
		And the IMS User gets a specific entity by entity Id that doesn't exist from the ER Collection
	Then validate the IMS User gets "404" Not Found error
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection 

#@PartialUpdate
#Scenario: Partially Update the Entities Synchronously Without the Required and Invalid Fileds in an ER Collection
#	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
#		#And the IMS User updates the entities where "async" is "false" and "allowPartialUpdate" is "true" without the "entityType" property using V2.0 in the ER Collection
#		And the IMS User updates the entities where "async" is "false" and "allowPartialUpdate" is "true" without the "entityId" property using V2.0 in the ER Collection
#		And the IMS User updates the entities where "async" is "false" and "allowPartialUpdate" is "true" with invalid property name "InvalidProp" and value "Test-01" in the ER Collection using V2.0
#	Then validate the IMS User gets "500" Internal server error code for the post(s)
#		And IMS User deletes the entity from the ER Collection
#		And validate IMS User deletes the entity from the ER Collection

@PartialUpdate
Scenario: Post Entities Synchronously in an ER Collection using V2.0 with allowPartialUpdate, with Invalid Schema	
	When the IMS User posts "1" new entity where "async" is "false" and "allowPartialUpdate" is "true" using V2.0 in the ER Collection "1" time(s) with Invalid Schema
	Then validate the IMS User gets "failure" responses message "Failed-InvalidSchema" while updating entity
		And validate IMS User gets "0" entities from the ER Collection

@PartialUpdate
Scenario: Partially Update the Entities Synchronously With Invalid property Fileds and validate response message NoAdditionalPropertiesAllowed
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User updates the entities where "async" is "false" and "allowPartialUpdate" is "true" with invalid property name "InvalidProp" and value "Test-01" in the ER Collection using V2.0
	Then validate the IMS User gets "failure" responses message "NoAdditionalPropertiesAllowed" while updating entity
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection

@PartialUpdate
Scenario: Partially Update the Entities Synchronously Without the Required field Entity Id and validate response message Entity id is missing
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)		
		And the IMS User updates the entities where "async" is "false" and "allowPartialUpdate" is "true" without the "entityId" property using V2.0 in the ER Collection
	Then validate the IMS User gets "failure" responses message "Entity id is missing" while updating entity
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection

Scenario: Post Entity Synchronously in an ER Collection using V2.0 and Updating Entity Type for this Entity is Not Allowed
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And  the IMS User updates entity type for the entity where "async" is "false" using V2.0 in the ER Collection
	Then validate the IMS User gets "failure" responses message "Restricted property entityType is not allowed to change" while updating entity
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection
