#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------
#Split the Permission Tests of ER Specflow in Separate Feature File to reduce Execution Time
#--------------------------------------------------------------------------------------

Feature: ER No Permissions Management - Labels

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

@GetByLabels
Scenario: Post Entities Synchronously in an ER Collection by Primary User, Secondary User With No Permissions Cannot Get All Entities by specific Label from the ER Collection, and Delete Entities
	When the IMS User posts "3" new entities with "1" label(s) where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the Secondary User with "No" permissions cannot get all entities by specific label from the ER Collection
		And validate the Secondary User with "No" permissions gets "403" Forbidden error code
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection
