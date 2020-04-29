#-------------------------------------------------------------------------------------
#
# © Copyright 2018 Johnson Controls, Inc.
# Use or Copying of all or any part of this program, except as
# permitted by License Agreement, is prohibited.
#
#--------------------------------------------------------------------------------------

Feature: ERStatus Management


Scenario: Get ER Status 
	Given an IMS User "apollo.er.testuser1" with an access token
	Then the IMS User gets ER Status
		And validate the status message points to "erapi" with a version number
