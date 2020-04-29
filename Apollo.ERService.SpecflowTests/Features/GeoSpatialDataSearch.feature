#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: GeoSpatial Data Search

Background:
	Given an IMS User "apollo.er.testuser1" with an access token
	When the IMS User posts Collection and response status Code as OK

Scenario: Post Entities Synchronously with GeoBoundary and GeoLocation in an ER Collection, Get All Entities whose GeoLocation matches EXACTLY with the Query Point from the ER Collection, and Delete Entities
	When the IMS User posts "3" new entities with GeoBoundary and GeoLocation where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities whose GeoLocation matches exactly with the Query Point "(31.6,-4.7)" from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection
		
Scenario: Post Entities Synchronously with GeoBoundary and GeoLocation in an ER Collection, Get All Entities whose GeoBoundary contain Query Point from the ER Collection, and Delete Entities
	When the IMS User posts "3" new entities with GeoBoundary and GeoLocation where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities whose GeoBoundary contain Query Point "(31.9,-4.8)" from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities Synchronously with GeoBoundary and GeoLocation in an ER Collection, Get All Entities whose GeoLocation(Point) is contained within the Query Polygon from the ER Collection, and Delete Entities
	When the IMS User posts "3" new entities with GeoBoundary and GeoLocation where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities whose GeoLocation(Point) is contained within the Query Polygon "(31.8,-5),(32,-5),(32,-4.7),(31.8,-4.7),(31.8,-5)" from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities Synchronously with GeoBoundary and GeoLocation in an ER Collection, Get All Entities whose GeoBoundary contain Invalid Query Point from the ER Collection, and Delete Entities
	When the IMS User posts "3" new entities with GeoBoundary and GeoLocation where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User does not get all entities whose GeoBoundary contain Invalid Query Point "(1.9,119.8)" from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection

Scenario: Post Entities Synchronously with GeoBoundary and GeoLocation in an ER Collection, Get All Entities whose GeoLocation does not match EXACTLY with the Query Point from the ER Collection, and Delete Entities
	When the IMS User posts "3" new entities with GeoBoundary and GeoLocation where "async" is "false" using V2.0 in an ER Collection "1" time(s)
	Then validate the IMS User gets all entities whose GeoLocation does not match exactly with the Query Point "(1.9,119.8)" from the ER Collection
		And IMS User deletes the entities from the ER Collection
		And validate IMS User deletes the entities from the ER Collection