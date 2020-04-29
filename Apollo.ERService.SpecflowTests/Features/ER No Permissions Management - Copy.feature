#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------
#Split the Permission Tests of ER Specflow in Separate Feature File to reduce Execution Time
#--------------------------------------------------------------------------------------
Feature: ER No Permissions Management - Copy

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities and Relationships Synchronously in an ER Collection using V2.0 by Primary User, Secondary User With No Permissions Cannot Copy Entities and Relationships from Source ER Collection to Destination ER Collection and Delete Relationships and Entities
	When the IMS User posts "2" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
	Then the Secondary User with "No" permissions Cannot copies entities and relationships from this ER Collection to another ER Collection
	 	And validate the Secondary User with "No" permissions gets "403" Forbidden error code
		And IMS User deletes the relationship(s) from the ER Collection
		And IMS User deletes the entities from the ER Collection	
		And validate IMS User deletes the relationship(s) from the ER Collection 
		And validate IMS User deletes the entities from the ER Collection
