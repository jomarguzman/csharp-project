#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------
#Split the Permission Tests of ER Specflow in Separate Feature File to reduce Execution Time
#--------------------------------------------------------------------------------------

Feature: ER Read Permissions Management - Label

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

@GetByLabels
Scenario: Post Entities Synchronously in an ER Collection by Primary User, Primary User Sets Secondary User With ReadOnly Permissions, Secondary User Can Get All Entities by specific Label from the ER Collection, and Delete Entities
	When the IMS User posts "3" new entities with "1" label(s) where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User sets "readonly" permissions for Secondary User "apollo.er.testuser2"
	Then validate the Secondary User with "readonly" permissions gets all entities by specific label from the ER Collection
		And the IMS User deletes "readonly" permissions from the Secondary User
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection
