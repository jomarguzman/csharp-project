#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: ER No Permissions Management

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities Synchronously in an ER Collection by Primary User, Secondary User With No Permissions Cannot Post, Get, and Delete Entities and Relationships in the ER Collection
		When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	    And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)		
	Then validate the Secondary User with "No" permissions cannot post "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And validate the Secondary User with "No" permissions cannot post "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
		And validate the Secondary User with "No" permissions gets "403" Forbidden error code
	    And validate the Secondary User with "No" permissions cannot get all relationships from the ER Collection
		And validate the Secondary User with "No" permissions cannot get all entities in an ER Collection
		And validate the Secondary User with "No" Permissions cannot delete the relationships from the ER Collection
		And validate the Secondary User with "No" permissions cannot delete the entities from the ER Collection
		And IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection