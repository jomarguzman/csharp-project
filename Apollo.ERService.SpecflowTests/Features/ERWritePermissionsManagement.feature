#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: ER Write Permissions Management

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities and Relationships Synchronously in an ER Collection by Primary User, Primary User Sets Secondary User With Readwrite Permissions, Secondary User Can Get, Post and Delete Entities and Relationships Synchronously in the ER Collection
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	    And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
		And the IMS User sets "readwrite" permissions for Secondary User "apollo.er.testuser2"	    
	Then validate the Secondary User with "readwrite" permissions can post "2" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And validate the Secondary User with "readwrite" permissions can post "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
		And validate the Secondary User with "readwrite" permissions can get all entities in an ER Collection
		And validate the Secondary User with "readwrite" permissions can get all relationships from the ER Collection
		And the Secondary User with "readwrite" permissions can delete the relationship(s) from the ER Collection
		And validate the Secondary User with "readwrite" permissions can delete the relationship(s) from the ER Collection
		And the Secondary User with "readwrite" permissions can delete the entities from the ER Collection
		And validate the Secondary User with "readwrite" permissions can delete the entities from the ER Collection
		And the IMS User deletes "readwrite" permissions from the Secondary User