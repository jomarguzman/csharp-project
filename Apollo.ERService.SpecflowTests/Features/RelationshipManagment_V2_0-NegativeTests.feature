#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: Relationship Managment V2.0 - NegativeTests

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities and Relationships Synchronously Without Required Field Relationship Id and validate response message Relationship id is missing
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
    	And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" without "relationshipId" property using V2.0 in the ER Collection
	Then validate the IMS User gets "failure" responses message "Relationship id is missing" while updating relationship(s)
		And validate IMS User cannot get the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously Without Required Field Relationship Type and validate response message Relationship Type is missing
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
    	And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" without "relationshipType" property using V2.0 in the ER Collection 
	Then validate the IMS User gets "failure" responses message "Relationship type is missing" while updating relationship(s)
		And validate IMS User cannot get the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously Without Required Field Source Entity ID and validate response message sourceEntityId:PropertyRequired
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
    	And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" without "sourceEntityId" property using V2.0 in the ER Collection
	Then validate the IMS User gets "failure" responses message "sourceEntityId:PropertyRequired" while updating relationship(s)
		And validate IMS User cannot get the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously Without Required Field Destination Entity ID and validate response message destinationEntityId:PropertyRequired
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
    	And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" without "destinationEntityId" property using V2.0 in the ER Collection
	Then validate the IMS User gets "failure" responses message "destinationEntityId:PropertyRequired" while updating relationship(s)
		And validate IMS User cannot get the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously Without Required Field Relationship Name and validate response message relationshipName:PropertyRequired
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
    	And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" without "relationshipName" property using V2.0 in the ER Collection
	Then validate the IMS User gets "failure" responses message "relationshipName:PropertyRequired" while updating relationship(s)
		And validate IMS User cannot get the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Post Relationships Synchronously With Invalid SourceEntityId and DestinationEntityId in an ER Collection
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	    And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" with invalid sourceEntityId using V2.0 in the ER Collection
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" with invalid destinationEntityId using V2.0 in the ER Collection
	Then validate the IMS User gets "412" precondition failed error code for the post(s)
		And validate IMS User cannot get the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User cannot delete the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously With An Empty List in an ER Collection
	When the IMS User posts "0" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection
	Then validate the IMS User gets "400" Bad Request error code for the post(s)
		And validate IMS User cannot get the relationship(s) from the ER Collection
		And validate IMS User cannot delete the relationship(s) from the ER Collection

Scenario: Post Entities and Relationships Synchronously With Invalid Token in an ER Collection
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" With Invalid Token using V2.0 in the ER Collection "1" time(s)
	Then validate the IMS User gets "401" Unauthorized error code for the post(s)
		And validate IMS User cannot get the relationship(s) from the ER Collection

@BulkPost
Scenario: Post Entities Async and Relationships Sync in Bulk in an ER Collection
	When the IMS User posts "68" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "102" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection
	Then validate the IMS User gets "413" Payload too large error code for the post(s)
		And validate the IMS User gets all entities from the ER Collection
		And validate IMS User cannot get the relationship(s) from the ER Collection

Scenario: Post Entities and Post Relationships Synchronously Without ER Collection
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	    And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 without the ER Collection
	Then validate the IMS User gets "400" Bad Request error code for the post(s)
	   And validate IMS User cannot get the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User cannot delete the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Get Relationship By Relationship Id that doesn't exist from an ER Collection
	 When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
		And IMS User gets a specific relationship by relationship Id that doesn't exist from the ER Collection
	Then validate the IMS User gets "404" Not Found error
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

#@PartialUpdate
#Scenario: Partially Update the Relationships Synchronously Without the Required and Invalid Fields in an ER Collection
#	 When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
#		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
#		And the IMS User updates the relationships where "async" is "false" and "allowPartialUpdate" is "true" without the "relationshipType" property using V2.0 in the ER Collection
#		And the IMS User updates the relationships where "async" is "false" and "allowPartialUpdate" is "true" without the "relationshipId" property using V2.0 in the ER Collection
#		And the IMS User updates the relationships where "async" is "false" and "allowPartialUpdate" is "true" with invalid property name "InvalidProp" and value "Test-01" in the ER Collection using V2.0
#	Then validate the IMS User gets "500" Internal server error code for the post(s)
#		And IMS User deletes the relationship(s) from the ER Collection
#		And IMS User deletes the entities from the ER Collection	
#		And validate IMS User deletes the relationship(s) from the ER Collection
#		And validate IMS User deletes the entities from the ER Collection

@PartialUpdate
Scenario: Post Reationships Synchronously in an ER Collection using V2.0 with allowPartialUpdate, with Invalid Schema
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" and "allowPartialUpdate" is "true" using V2.0 in an ER Collection "1" time(s) with Invalid Schema
	Then validate the IMS User gets "failure" responses message "Failed-Invalid Schema" while updating relationship(s)
		And validate IMS User cannot get the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

@PartialUpdate
Scenario: Partially Update the Relationships Synchronously With Invalid property Fileds and validate response message NoAdditionalPropertiesAllowed
	 When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
		And the IMS User updates the relationships where "async" is "false" and "allowPartialUpdate" is "true" with invalid property name "InvalidProp" and value "Test-01" in the ER Collection using V2.0
	Then validate the IMS User gets "failure" responses message "NoAdditionalPropertiesAllowed" while updating relationship(s)
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

@PartialUpdate
Scenario: Partially Update the Relationships Synchronously Without the Required field Relationship Id and validate response message Relationship id is missing
	 When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
		And the IMS User updates the relationships where "async" is "false" and "allowPartialUpdate" is "true" without the "relationshipId" property using V2.0 in the ER Collection
	Then validate the IMS User gets "failure" responses message "Relationship id is missing" while updating relationship(s)
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Relationship Synchronously in an ER Collection using V2.0 and Updating Relationship Type for this Relationship is Not Allowed
	When the IMS User posts "2" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
		And the IMS User updates the relationship type for the posted relationship(s) where "async" is "false" using V2.0 in the ER Collection
	Then validate the IMS User gets "failure" responses message "Restricted property relationshipType is not allowed to change" while updating relationship(s)
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection