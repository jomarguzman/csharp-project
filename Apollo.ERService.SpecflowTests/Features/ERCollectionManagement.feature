#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: ERCollection Management

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities and Relationships Synchronously in an ER Collection using V2.0, Copy Entities and Relationships from Source ER Collection to Destination ER Colletion, Get All Entities And Relationships from the ER Collection, and Delete Relationships and Entities
	When the IMS User posts "2" new entity where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "1" new relationship(s) between the entities where "async" is "false" using V2.0 in the ER Collection "1" time(s)
		And the IMS User posts Collection and response status Code as OK
		And the IMS User copies entities and relationships from this ER Collection to another ER Collection
	 Then validate the IMS User gets all entities from the destination ER Collection
		And validate the IMS User gets all relationships from the destination ER Collection
		And IMS User deletes the relationship(s) from source & destination ER Collections
		And IMS User deletes the entities from the source & destination ER Collections
		And validate IMS User deletes the relationship(s) from source & destination the ER Collections
		And validate IMS User deletes the entities from the source & destination ER Collections

Scenario: Post Entities and Relationships Synchronously in an ER Collection using V2.0, Get ER Stats By ER Collection, and Delete Relationships and Entities
	When the IMS User posts "4" new entities of types "Brick__Building,Brick__Floor,Brick__Room,Brick__Equipment" where "async" is "false" using V2.0 in an ER Collection "1" time(s)
		And the IMS User posts "2" new relationship(s) of type(s) 'Brick__hasPart,Brick__isPartOf' between the entities where "async" is "false" using V2.0 in the ER Collection using ER API
	 Then validate the IMS User gets stats by ER Collection 
	     And IMS User deletes the relationship(s) from the ER Collection
		 And IMS User deletes the entities from the ER Collection	
		 And validate IMS User deletes the relationship(s) from the ER Collection
		 And validate IMS User deletes the entities from the ER Collection
