#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: RelationshipType Management

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities and Relationships Synchronously in an ER Collection, Get All Relationships Of A Specific Relationship Type from the ER Collection, and Delete Relationships and Entities
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
	Then validate the IMS User gets all relationships of specific relationship type from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Asynchronously in an ER Collection, Get All Relationships Of A Specific Relationship Type from the ER Collection, and Delete Relationships and Entities
	When the IMS User posts "2" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "true" using V2.0 in the ER Collection "1" time(s)
	Then validate the IMS User gets all relationships of specific relationship type from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously in an ER Collection, Get All Relationships Of A Specific Relationship Type using Page Size and Continuation Token from the ER Collection, and Delete Relationships and Entities
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "2" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
	Then validate the IMS User gets "1" relationship(s) and "continuationToken" when page size is "1" 
		And validate the IMS User gets "1" relationship(s) using the "continuationToken"
	    And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously in an ER Collection, Get All Versions Of A Specific Relationship Type from the ER Collection, and Delete Relationships and Entities
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "2" time(s)
	Then validate the IMS User gets all "2" versions of specific relationship by relationship Type from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously in an ER Collection, Get All Versions Of A Specific Relationship By Relationship Type using Page Size and Continuation Token from the ER Collection, and Delete Relationships and Entities
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "2" time(s)
	Then validate the IMS User gets "1" version of specific relationship by relationship type and "continuationToken" when page size is "1" 
		And validate the IMS User gets "1" versions of a specific relationship by relationship type using the "continuationToken"
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Asynchronously in an ER Collection, Get All Versions Of A Specific Relationship Type from the ER Collection, and Delete Relationships and Entities
	When the IMS User posts "2" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "true" using V2.0 in the ER Collection "2" time(s)
	Then validate the IMS User gets all "2" versions of specific relationship by relationship Type from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection