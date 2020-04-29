#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: Labels, Assets, And Spaces Management

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

@GetByLabels
Scenario: Post Entities Synchronously in an ER Collection, Get All Entities by specific Label from the ER Collection, and Delete Entities
	When the IMS User posts "3" new entities with "1" label(s) where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities by specific label from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

@GetByLabels
Scenario: Post Entities Synchronously in an ER Collection, Get All Entities by Multiple Labels with OR condition from the ER Collection, and Delete Entities
	When the IMS User posts "3" new entities with "1" label(s) where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities by multiple labels with OR condition from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

@GetByLabels
Scenario: Post Entities Synchronously in an ER Collection, Get All Entities by Multiple Labels with AND condition from the ER Collection, and Delete Entities
	When the IMS User posts "3" new entities with "2" label(s) where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities by multiple labels with AND condition from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

@AssetsAndSpaces
Scenario: Post Entities Synchronously in an ER Collection, Get list of Assets and Spaces within the ER Collection and Delete Entities 
	When the IMS User posts "2" new entities of types "Brick__Equipment,Brick__Building" where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets the list of Assets within the ER Collection
		And validate the IMS User gets the list of Spaces within the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection