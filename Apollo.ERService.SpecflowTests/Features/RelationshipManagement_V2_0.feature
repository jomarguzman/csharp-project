#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: Relationship Management V2.0

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities and Relationships Synchronously in an ER Collection, Get All Relationships from the ER Collection, and Delete Relationships and Entities
    When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
	Then validate the IMS User gets all relationships from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously in an ER Collection, Get A Specific Relationship By Relationship Id from the ER Collection, and Delete Relationships and Entities
    When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
	Then validate the IMS User gets a specific relationship by relationship Id from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously in an ER Collection, Get All Versions Of A Specific Relationship By Relationship Id from the ER Collection, and Delete Relationships and Entities
    When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s) 
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "2" time(s)
	Then validate the IMS User gets all "2" versions of specific relationship by relationship Id from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Synchronously in an ER Collection, Get All Versions Of A Specific Relationship By Relationship Id using Page Size and Continuation Token from the ER Collection, and Delete Relationships and Entities
    When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s) 
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "2" time(s)
	Then validate the IMS User gets "1" version of specific relationship by relationship Id and "continuationToken" when page size is "1" 
		And validate the IMS User gets "1" versions of a specific relationship by relationship Id using the "continuationToken"
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships ASynchronously in an ER Collection, Get All Versions Of A Specific Relationship By Relationship Id from the ER Collection, and Delete Relationships and Entities
    When the IMS User posts "2" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s) 
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "true" using V2.0 in the ER Collection "2" time(s)
	Then validate the IMS User gets all "2" versions of specific relationship by relationship Id from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities and Relationships Asynchronously in an ER Collection, Get All Relationships from the ER Collection, and Delete Relationships and Entities
    When the IMS User posts "2" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "5" new relationship(s) between the entities where "async" is "true" using V2.0 in the ER Collection "1" time(s)
	Then validate the IMS User gets all relationships from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

@PartialUpdate
Scenario: Post Reationships Synchronously in an ER Collection using V2.0, Partially Update the Reationships Synchronously, and Delete Reationships and Entities
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
	    And the IMS User updates the relationship(s) where "async" is "false" and "allowPartialUpdate" is "true" using V2.0 in the ER Collection
	Then validate the IMS User updates the relationship(s)
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

@PartialUpdate
Scenario: Post Reationships Asynchronously in an ER Collection using V2.0, Partially Update the Reationships Synchronously, and Delete Reationships and Entities
	When the IMS User posts "2" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "true" using V2.0 in the ER Collection "1" time(s)
	    And the IMS User updates the relationship(s) where "async" is "true" and "allowPartialUpdate" is "true" using V2.0 in the ER Collection
	Then validate the IMS User updates the relationship(s)
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

@PartialUpdate
Scenario: Post Reationships Synchronously in an ER Collection using V2.0 with allowPartialUpdate, Get All Relationships from the ER Collection, and Delete Reationships and Entities
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" and "allowPartialUpdate" is "true" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all relationships from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

@PartialUpdate
Scenario: Post Reationships Synchronously in an ER Collection using V2.0, Partially Update the relationship ID Synchronously, and Delete Reationships and Entities
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
	    And the IMS User updates the relationship ID where "async" is "false" and "allowPartialUpdate" is "true" using V2.0 in the ER Collection
	Then validate the IMS User updates the relationship(s)
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

@PostRelationshipsSyncAcrossERCollections
Scenario: Post Entities Synchronously in different ER Collections and Reationships across the ER Collections using V2.0, Get All Relationships from Source ER Collection, and Delete Relationships and Entities
	When the IMS User posts "1" new entity each in "2" ER Collections where "async" is "false" using V2.0 "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities across the ERCollections where "async" is "false" using V2.0
	Then validate the IMS User gets all relationships from source ER Collection(s)
		And IMS User deletes the relationship(s) from source ER Collection(s)
		And IMS User deletes the entities across all the ER Collections
		And validate IMS User deletes the relationship(s) from source ER Collection(s)
		And validate IMS User deletes the entities across all the ER Collections

@PostRelationshipsAsyncAcrossERCollections
Scenario: Post Entities and Relationships Asynchronously in different ER Collections using V2.0, Post Relationships Synchronously Across ER Collections, Get All Relationships from Source ER Collection, and Delete Relationships and Entities
	When the IMS User posts "1" new entity each in "2" ER Collections where "async" is "true" using V2.0 "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities across the ERCollections where "async" is "true" using V2.0
	Then validate the IMS User gets all relationships from source ER Collection(s)
		And IMS User deletes the relationship(s) from source ER Collection(s)
		And IMS User deletes the entities across all the ER Collections
		And validate IMS User deletes the relationship(s) from source ER Collection(s)
		And validate IMS User deletes the entities across all the ER Collections
