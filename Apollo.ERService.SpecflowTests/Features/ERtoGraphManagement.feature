#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: ER to Graph Management

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Delete Relationships Synchronously in an ER Collection From ER API Should Delete Relationships From Graph API
     When the IMS User posts "2" new entities where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		 And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
     Then IMS User deletes the relationship(s) from the ER Collection
		 And validate IMS User deletes the relationship(s) from the ER Collection using Graph API
		 And IMS User deletes the entities from the ER Collection
		 And validate IMS User deletes the entities from the ER Collection

Scenario: Delete Relationships Asynchronously in an ER Collection From ER API Should Delete Relationships From Graph API
      When the IMS User posts "2" new entities where "async" is "true" using V2.0 in an ER Collection "1" time(s)
	     And the IMS User posts "1" new relationship(s) between the entities where "async" is "true" using V2.0 in the ER Collection "1" time(s)
	  Then IMS User deletes the relationship(s) from the ER Collection
         And validate IMS User deletes the relationship(s) from the ER Collection using Graph API
         And IMS User deletes the entities from the ER Collection
         And validate IMS User deletes the entities from the ER Collection
