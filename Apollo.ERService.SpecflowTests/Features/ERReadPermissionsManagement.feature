#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: ER Read Permissions Management

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities and Relationships Synchronously in an ER Collection by Primary User, Primary User Sets Secondary User With ReadOnly Permissions, Secondary User Can Get But Cannot Post or Delete Entities and Relationships in the ER Collection
	When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	    And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
		And the IMS User sets "readonly" permissions for Secondary User "apollo.er.testuser2"
	Then validate the Secondary User with "readonly" permissions cannot post "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And validate the Secondary User with "readonly" permissions cannot post "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
		And validate the Secondary User with "readonly" permissions gets "403" Forbidden error code 
	    And validate the Secondary User with "readonly" permissions can get all relationships from the ER Collection
		And validate the Secondary User with "readonly" permissions can get all entities in an ER Collection
		And validate the Secondary User with "readonly" Permissions cannot delete the relationships from the ER Collection
		And validate the Secondary User with "readonly" permissions cannot delete the entities from the ER Collection
		And the IMS User deletes "readonly" permissions from the Secondary User
		And IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection