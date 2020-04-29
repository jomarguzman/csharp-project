#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------
#Split the Permission Tests to reduce Execution Time
Feature: ER No Permissions Management - ER Stats
	
Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities and Relationships Synchronously in an ER Collection using V2.0 by Primary User, Secondary User With No Permissions Cannot Get ER Stats By ER Collection, and Delete Relationships and Entities
	When the IMS User posts "4" new entities of types "Brick__Building,Brick__Floor,Brick__Room,Brick__Equipment" where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "2" new relationship(s) of type(s) 'Brick__hasPart,Brick__isPartOf' between the entities where "async" is "false" using V2.0 in the ER Collection using ER API
	Then validate the Secondary User with "No" permissions cannot get stats by ER Collection 
	 	And validate the Secondary User with "No" permissions gets "403" Forbidden error code
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection
		And validate IMS User deletes the entities from the ER Collection
