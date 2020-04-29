#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: EntityType Management - NegativeTests

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Get an empty list when entities of a specific entity type doesn't exist in an ER Collection
	When the IMS User posts "1" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User gets all entities of specific entity type that doesn't exist in the ER Collection
	Then validate the IMS User gets "0" entities of the entity type with "200" Ok status code from the ER Collection
		And IMS User deletes the entity from the ER Collection
		And validate IMS User deletes the entity from the ER Collection