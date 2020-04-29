#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: ERSchema Refresh Management

Background:
	Given an IMS User "apollo.er.testuser1" with an access token


Scenario: Refresh Entity And Relationship Schemas
	When IMS User refreshes "BRICK_0_4__Equipment" entity type schema
		And IMS User refreshes "BRICK_0_4__isFedBy_Equipment_Equipment" relationship type schema
	Then validate the IMS User gets 200 Ok status code for the refreshed schemas


